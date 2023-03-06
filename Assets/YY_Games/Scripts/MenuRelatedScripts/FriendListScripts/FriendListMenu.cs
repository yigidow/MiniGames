using PlayFab.ClientModels;
using System.Collections;
using UnityEngine;
using PlayFab;
using TMPro;

namespace YY_Games_Scripts
{
    public class FriendListMenu : MonoBehaviour
    {
        #region Friend List Menu References
        [Header("Objects and Containers")]
        [SerializeField] private FriendListPrefabForScroll friendListPrefab;
        [SerializeField] private GameObject friendListScrollViewRef;

        //To display messages to user
        [SerializeField] private TextMeshProUGUI messageText;
        #endregion

        #region Add Friend Menu Functions
        private void DestroyAllFromScroll()
        {
            if (friendListScrollViewRef.transform.childCount == 0)
            {
                return;
            }
            for (int i = 0; i < friendListScrollViewRef.transform.childCount; i++)
            {
                Destroy(friendListScrollViewRef.transform.GetChild(i).gameObject);
            }
        }

        private void GetFriendsAndLoad()
        {
            var getFriendsRequest = new GetFriendsListRequest();

            PlayFabClientAPI.GetFriendsList(getFriendsRequest,
                 resultCallback: result =>
                 {
                     //Loading all friends
                     foreach (FriendInfo friendInfo in result.Friends)
                     {
                         //Loading only requester tagged friends
                         if (friendInfo.Tags[0] == "requestee")
                         {
                             var playerFriend = Instantiate(friendListPrefab.gameObject, friendListScrollViewRef.transform).gameObject;

                             var playerFriendScript = playerFriend.GetComponent<FriendListPrefabForScroll>();

                             //Setting the prefab variables 
                             playerFriendScript.SetFriendNamePLayfabIdAndStatus(friendInfo.Username, "Pending", friendInfo.FriendPlayFabId);

                         }
                         if(friendInfo.Tags[0] == "confirmed")
                         {
                             var playerFriend = Instantiate(friendListPrefab.gameObject, friendListScrollViewRef.transform).gameObject;

                             var playerFriendScript = playerFriend.GetComponent<FriendListPrefabForScroll>();

                             //Setting the prefab variables 
                             playerFriendScript.SetFriendNamePLayfabIdAndStatus(friendInfo.Username, "Confirmed",friendInfo.FriendPlayFabId);
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
            DestroyAllFromScroll();
            GetFriendsAndLoad();
        }
        #endregion
    }
}

