using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using PlayFab.ClientModels;
using PlayFab;
using Newtonsoft.Json.Linq;

namespace YY_Games_Scripts
{
    public class FriendRequestPrefabForScroll : MonoBehaviour
    {
        #region Referances
        [Header("Friend Request Prefab References")]
        [SerializeField] private TextMeshProUGUI userNameRef;
        [SerializeField] private Button acceptFriendRequestButton;
        [SerializeField] private Button rejectFriendRequestButton;
        [SerializeField] private TextMeshProUGUI messageText;
        private string friendPlayFabId;
        #endregion

        #region Friend Request Functions
        //Assigning Buttons
        private void OnAcceptFriendRequestButtonClick()
        {
            //Accept Friend Request
            //Request to execute cloud script
            var executeCloudScriptRequest = new ExecuteCloudScriptRequest
            {
                FunctionName = "AcceptFriendRequest",
                FunctionParameter = new { FriendId = friendPlayFabId },
                GeneratePlayStreamEvent = true
            };
            PlayFabClientAPI.ExecuteCloudScript(executeCloudScriptRequest,
                resultCallback: result =>
                {
                    // Getting Result From Server
                    var serilizedResult = JObject.Parse(PluginManager.GetPlugin<ISerializerPlugin>(PluginContract.PlayFab_Serializer).SerializeObject(result.FunctionResult));

                    print(serilizedResult["message"]);
                    StartCoroutine(DisplayMessages(serilizedResult["message"]?.ToString()));

                    //Destroy Entry as friend confirmed
                    Destroy(gameObject);
                },
                errorCallback: error =>
                {
                    StartCoroutine(DisplayMessages(error.ErrorMessage));
                    acceptFriendRequestButton.interactable = true;
                    rejectFriendRequestButton.interactable = true;
                }
                );

        }
        private void OnRejectFriendRequestButtonClick()
        {
            //Request to execute cloud script
            var executeCloudScriptRequest = new ExecuteCloudScriptRequest
            {
                FunctionName = "RejectFriendRequest",
                FunctionParameter = new { FriendId = friendPlayFabId },
                GeneratePlayStreamEvent = true
            };
            PlayFabClientAPI.ExecuteCloudScript(executeCloudScriptRequest,
                resultCallback: result =>
                {
                    // Getting Result From Server
                    var serilizedResult = JObject.Parse(PluginManager.GetPlugin<ISerializerPlugin>(PluginContract.PlayFab_Serializer).SerializeObject(result.FunctionResult));

                    print(serilizedResult["message"]);
                    StartCoroutine(DisplayMessages(serilizedResult["message"]?.ToString()));

                    //Destroy Entry as friend rejected
                    Destroy(gameObject);
                },
                errorCallback: error =>
                {
                    StartCoroutine(DisplayMessages(error.ErrorMessage));
                    acceptFriendRequestButton.interactable = true;
                    rejectFriendRequestButton.interactable = true;
                }
                );
        }
        public void SetFriendNameAndPlayfabID(string friendName, string playFabId)
        {
            userNameRef.text = friendName;
            friendPlayFabId = playFabId;
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
        private void Start()
        {
            acceptFriendRequestButton.onClick.AddListener(OnAcceptFriendRequestButtonClick);
            rejectFriendRequestButton.onClick.AddListener(OnRejectFriendRequestButtonClick);
        }
        #endregion

    }

}
