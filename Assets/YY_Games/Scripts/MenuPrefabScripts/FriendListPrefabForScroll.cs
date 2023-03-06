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
    public class FriendListPrefabForScroll : MonoBehaviour
    {
        #region Referances
        [Header("Friend List Prefab References")]
        [SerializeField] private TextMeshProUGUI friendNameRef;
        [SerializeField] private TextMeshProUGUI friendStatusRef;
        [SerializeField] private Button removeFriendFromListButton;
        [SerializeField] private TextMeshProUGUI messageText;
        private string friendPlayFabId;
        #endregion
        #region Friend List Functions
        //Assigning Buttons
        private void OnRemoveFriendButtonClick()
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
                    StartCoroutine(DisplayMessages("User Removed"));

                    //Destroy Entry as friend rejected
                    Destroy(gameObject);
                },
                errorCallback: error =>
                {
                    StartCoroutine(DisplayMessages(error.ErrorMessage));
                }
                );

        }
        public void SetFriendNamePLayfabIdAndStatus(string friendName, string friendStatus, string playFabId)
        {
            friendNameRef.text = friendName;
            friendStatusRef.text = friendStatus;
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
        void Start()
        {
            removeFriendFromListButton.onClick.AddListener(OnRemoveFriendButtonClick);
        }
        #endregion
    }
}

