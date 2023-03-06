using PlayFab.ClientModels;
using System.Collections;
using UnityEngine;
using PlayFab;
using TMPro;


namespace YY_Games_Scripts
{
    public class FriendRequestMenu : MonoBehaviour
    {
        #region Add Friend Menu References
        [Header("Objects and Containers")]
        [SerializeField] private FriendRequestPrefabForScroll friendRequestPrefab;
        [SerializeField] private GameObject friendRequestsScrollViewRef;

        //To display messages to user
        [SerializeField] private TextMeshProUGUI messageText;
        #endregion

        #region Add Friend Menu Functions
        private void DestroyAllFriendRequests()
        {
            if (friendRequestsScrollViewRef.transform.childCount == 0)
            {
                return;
            }
            for (int i = 0; i < friendRequestsScrollViewRef.transform.childCount; i++)
            {
                Destroy(friendRequestsScrollViewRef.transform.GetChild(i).gameObject);
            }
        }

        private void GetFriendsAndLoad()
        {
            var getFriendsRequest = new GetFriendsListRequest();

            PlayFabClientAPI.GetFriendsList(getFriendsRequest,
                 resultCallback: result =>
                 {
                     //Loading all friends
                     foreach(FriendInfo friendInfo in result.Friends)
                     {   
                         //Loading only requester tagged friends
                         if(friendInfo.Tags[0] == "requester")
                         {
                             var friendRequest = Instantiate(friendRequestPrefab.gameObject, friendRequestsScrollViewRef.transform).gameObject;

                             var friendRequestScript = friendRequest.GetComponent<FriendRequestPrefabForScroll>();

                             //Setting the prefab variables 
                             friendRequestScript.SetFriendNameAndPlayfabID(friendInfo.Username, friendInfo.FriendPlayFabId);
                         }
                     }
                      
                 },
                 errorCallback: error =>
                 {
                     StartCoroutine(DisplayMessages(error.ErrorMessage));
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
        private void OnEnable()
        {
            DestroyAllFriendRequests();
            GetFriendsAndLoad();
        }
        #endregion
    }

}
