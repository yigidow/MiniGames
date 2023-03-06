using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using PlayFab;
using PlayFab.ClientModels;
using Newtonsoft.Json.Linq;

namespace YY_Games_Scripts
{
    public class AddFriendMenu : MonoBehaviour
    {
        #region Add Friend Menu References
        [Header("Objects and Containers")]
        [SerializeField] private TMP_InputField searchInputText;
        [SerializeField] private Button sendRequestButton;
        //To display messages to user
        [SerializeField] private TextMeshProUGUI messageText;
        private string friendUserName;
        #endregion

        #region Add Friend Menu Functions
        //Get the changed value of inputField
        public void OnAddFriendMenuSearchInputValueChanged(string value)
        {
            friendUserName = value;
        }
        private void OnAddFriendMenuSendRequestButtonClick()
        {
            sendRequestButton.interactable = false;
            //To control Input Field is not emptu
            if (string.IsNullOrEmpty(friendUserName)) 
            {
                StartCoroutine(DisplayMessages("Please enter a username"));
                sendRequestButton.interactable = true;
                return;
            }

            //API Request to search for friend

            var searchRequest = new GetAccountInfoRequest
            {
                Username = friendUserName
            };
            PlayFabClientAPI.GetAccountInfo(searchRequest,
                resultCallback: result =>
                {
                    DisplayMessages("Processing request");
                    var playFabId = result.AccountInfo.PlayFabId;
                    SendRequestToFoundUser(playFabId);
                   
                },
                errorCallback: error =>
                {
                    StartCoroutine(DisplayMessages(error.ErrorMessage));
                    sendRequestButton.interactable = true;
                }
                );
        }
        #endregion

        #region CloudScript Functions
        //If a user succesfully finds a player and wants a friend request to the user
        private void SendRequestToFoundUser(string playersPlayFabId)
        {
            //Request to execute cloud script
            var executeCloudScriptRequest = new ExecuteCloudScriptRequest
            {
                FunctionName = "ProcessFriendRequest",
                FunctionParameter = new {FriendId = playersPlayFabId},
                GeneratePlayStreamEvent = true
            };
            PlayFabClientAPI.ExecuteCloudScript(executeCloudScriptRequest,
                resultCallback: result =>
                {
                    sendRequestButton.interactable = true;
                    try
                    {
                        // Getting Result From Server
                        var serilizedResult = JObject.Parse(PluginManager.GetPlugin<ISerializerPlugin>(PluginContract.PlayFab_Serializer).SerializeObject(result.FunctionResult));

                        print(serilizedResult["message"]);

                        StartCoroutine(DisplayMessages(serilizedResult["message"]?.ToString()));
                    }
                    catch
                    {
                        var serilizedResult = JObject.Parse(PluginManager.GetPlugin<ISerializerPlugin>(PluginContract.PlayFab_Serializer).SerializeObject(result.Logs?[0].Data));
                        StartCoroutine(DisplayMessages(serilizedResult["apiError"]?["errorMessage"]?.ToString()));
                    }
                },
                errorCallback: error =>
                {
                    StartCoroutine(DisplayMessages(error.ErrorMessage));
                    sendRequestButton.interactable = true;
                }
                );
        }

        #endregion

        #region Message Functions
        //Message To show when login or register is failed or succeeds  
        private IEnumerator DisplayMessages(string messageToDisplay)
        {
            messageText.gameObject.SetActive(true);
            messageText.text = messageToDisplay;
            yield return new WaitForSeconds(3f);
            messageText.gameObject.SetActive(false);
        }
        #endregion

        #region Unity Functions
        void Start()
        {
            sendRequestButton.onClick.AddListener(OnAddFriendMenuSendRequestButtonClick);
        }
        #endregion
    }
}

