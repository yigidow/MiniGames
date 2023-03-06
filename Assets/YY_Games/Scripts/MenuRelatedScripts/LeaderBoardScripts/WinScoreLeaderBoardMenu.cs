 using PlayFab;
using PlayFab.ClientModels;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


namespace YY_Games_Scripts
{
    //Loading data from playfab server and controlling buttons
    public class WinScoreLeaderBoardMenu : MonoBehaviour
    {
        
        #region Button References
        [Header("Button References")]

        [SerializeField] private Button playerRankButton;
        [SerializeField] private Button friendRankButton;
        [SerializeField] private Button globalRankButton;
        [SerializeField] private Button winLeaderBoardMenuBackButton;
        #endregion

        #region Object and Component References
        [Header("Object and Component References")]
        [SerializeField] private GameObject winLeaderBoardScrollViewRef;
        //Message texts to inform the player about connection
        [SerializeField] private TextMeshProUGUI winLeaderBoardMessageTextRef;
        // Leaderboard Prefab Ref
        [SerializeField] private GameObject leaderBoardEntryPrefabRef;
        //To store current PlayfabID
        private string currentPlayerPlayFabId = "";

        #endregion

        #region Button Functions
        private void DisableButtonInteractions()
        {
            playerRankButton.interactable = false;
            friendRankButton.interactable = false;
            globalRankButton.interactable = false;
            winLeaderBoardMenuBackButton.interactable = false;
        }
        private void EnableButtonInteractions()
        {
            playerRankButton.interactable = true;
            friendRankButton.interactable = true;
            globalRankButton.interactable = true;
            winLeaderBoardMenuBackButton.interactable = true;
        }

        private void SetUpWinScoreLeaderBoardButtons()
        {
            globalRankButton.onClick.AddListener(LoadWinsGlobalLeaderBoard);
            playerRankButton.onClick.AddListener(LoadPlayerWinRankLeaderBoard);
            friendRankButton.onClick.AddListener(LoadWinsFriendListlLeaderBoard);
        }
        #endregion

        #region Functions for Loading LeaderBoard
        //Request to fetch Win Record LeaderBoard from PlayFab Server
        private void LoadWinsGlobalLeaderBoard()
        {
            DestroyAllPreviousLeaderBoardEntries();
            DisableButtonInteractions();

            var leaderBoardFetchRequest = new GetLeaderboardRequest 
            {
                StatisticName = "Wins Record",
                MaxResultsCount = 10
            };

            PlayFabClientAPI.GetLeaderboard(
                leaderBoardFetchRequest,
                resultCallback: result =>
                {
                    //Storing Leaderboard List
                    var leaderBoardResultList = result.Leaderboard;

                    foreach(PlayerLeaderboardEntry playerLeaderboardEntry in leaderBoardResultList)
                    {
                        //Creating a Leaderboard Entry
                        var createdLeaderBoardEntry = Instantiate(leaderBoardEntryPrefabRef, winLeaderBoardScrollViewRef.transform);
                        // Filling the info from server
                        LeaderBoardPrefabForScroll createdLeaderBoardEntryScript = createdLeaderBoardEntry.GetComponent<LeaderBoardPrefabForScroll>();

                        //To make ranks start from 1 not 0 and set the values
                        playerLeaderboardEntry.Position++;
                        createdLeaderBoardEntryScript.SetValuesForLeaderBoard(
                        playerLeaderboardEntry.DisplayName,
                        playerLeaderboardEntry.Position.ToString(),
                        playerLeaderboardEntry.StatValue.ToString()
                        );
                    }
                    EnableButtonInteractions();
                    winLeaderBoardMessageTextRef.text = ("Global Win Records");
                },
                errorCallback: error =>
                {
                    StartCoroutine(DisplayLeaderBoardMessages(error.ErrorMessage));
                    print(error.ErrorMessage);
                    EnableButtonInteractions();
                }
            );
        }
        private void LoadPlayerWinRankLeaderBoard() 
        {
            DestroyAllPreviousLeaderBoardEntries();
            DisableButtonInteractions();

            var leaderBoardFetchRequest = new GetLeaderboardAroundPlayerRequest
            {
                StatisticName = "Wins Record",
                MaxResultsCount = 10
            };

            PlayFabClientAPI.GetLeaderboardAroundPlayer(
                leaderBoardFetchRequest,
                resultCallback: result =>
                {
                    //Storing Leaderboard List
                    var leaderBoardResultList = result.Leaderboard;

                    foreach (PlayerLeaderboardEntry playerLeaderboardEntry in leaderBoardResultList)
                    {
                        //Creating a Leaderboard Entry
                        var createdLeaderBoardEntry = Instantiate(leaderBoardEntryPrefabRef, winLeaderBoardScrollViewRef.transform);
                        // Filling the info from server
                        LeaderBoardPrefabForScroll createdLeaderBoardEntryScript = createdLeaderBoardEntry.GetComponent<LeaderBoardPrefabForScroll>();

                        //To make ranks start from 1 not 0 and set the values
                        playerLeaderboardEntry.Position++;
                        createdLeaderBoardEntryScript.SetValuesForLeaderBoard(
                        playerLeaderboardEntry.DisplayName,
                        playerLeaderboardEntry.Position.ToString(),
                        playerLeaderboardEntry.StatValue.ToString()
                        );
                    }
                    EnableButtonInteractions();
                    winLeaderBoardMessageTextRef.text = ("Player Win Records");
                },
                errorCallback: error =>
                {
                    StartCoroutine(DisplayLeaderBoardMessages(error.ErrorMessage));
                    print(error.ErrorMessage);
                    EnableButtonInteractions();
                }
            );
        }
        private void LoadWinsFriendListlLeaderBoard()
        {
            var listOfConfirmedFriends = new List<string>();
            var friendListRequest = new GetFriendsListRequest();

            PlayFabClientAPI.GetFriendsList(
                friendListRequest,
                resultCallback: result =>
                {

                    foreach (FriendInfo friendInfo in result.Friends)
                    {
                        if(friendInfo.Tags[0] == "confirmed")
                        {
                            listOfConfirmedFriends.Add(friendInfo.FriendPlayFabId);
                        }
                    }
                    listOfConfirmedFriends.Add(currentPlayerPlayFabId);
                    LoadConfirmedFriendListLeaderBoard(listOfConfirmedFriends);
                },
                errorCallback: error =>
                {
                    StartCoroutine(DisplayLeaderBoardMessages(error.ErrorMessage));
                    print(error.ErrorMessage);
                    EnableButtonInteractions();
                }
            );
        }
        private void LoadConfirmedFriendListLeaderBoard(List<string> confirmedFriendList)
        {
            DestroyAllPreviousLeaderBoardEntries();
            DisableButtonInteractions();

            var friendLeaderboardRequest = new GetFriendLeaderboardRequest
            {
                StatisticName = "Wins Record",
            };
               PlayFabClientAPI.GetFriendLeaderboard(
                friendLeaderboardRequest,
                resultCallback: result =>
                {
                    foreach (PlayerLeaderboardEntry playerLeaderboardEntry in result.Leaderboard)
                    {
                        if (confirmedFriendList.Contains(playerLeaderboardEntry.PlayFabId) == false) continue;

                        var createdLeaderBoardEntry = Instantiate(leaderBoardEntryPrefabRef, winLeaderBoardScrollViewRef.transform);
                        LeaderBoardPrefabForScroll createdLeaderBoardEntryScript = createdLeaderBoardEntry.GetComponent<LeaderBoardPrefabForScroll>();

                        //To make ranks start from 1 not 0 and set the values
                        playerLeaderboardEntry.Position++;
                        createdLeaderBoardEntryScript.SetValuesForLeaderBoard(
                        playerLeaderboardEntry.DisplayName,
                        playerLeaderboardEntry.Position.ToString(),
                        playerLeaderboardEntry.StatValue.ToString()
                        );
                    }
                    EnableButtonInteractions();
                    winLeaderBoardMessageTextRef.text = ("Friend List Win Records");
                },
                errorCallback: error =>
                {
                    StartCoroutine(DisplayLeaderBoardMessages(error.ErrorMessage));
                    print(error.ErrorMessage);
                    EnableButtonInteractions();
                }
            );
        }
        private void LoadAndStoreCurrentPlayerData()
        {
            var playerDataRequest = new GetAccountInfoRequest();

            PlayFabClientAPI.GetAccountInfo(
               playerDataRequest,
               resultCallback: result =>
               {
                   currentPlayerPlayFabId = result.AccountInfo.PlayFabId;
               },
               errorCallback: error =>
               {
                   StartCoroutine(DisplayLeaderBoardMessages(error.ErrorMessage));
                   print(error.ErrorMessage);

               }
               );

        }
        //To destroy already created prefabs to not load same prefab
        private void DestroyAllPreviousLeaderBoardEntries()
        {
            if (winLeaderBoardScrollViewRef.transform.childCount == 0)
            {
                return;
            }
            for (int i = 0; i < winLeaderBoardScrollViewRef.transform.childCount; i++)
            {
                Destroy(winLeaderBoardScrollViewRef.transform.GetChild(i).gameObject);
            }
        }
        #endregion

        #region Message Functions
        //Message To show when login or register is failed or succeeds  
        private IEnumerator DisplayLeaderBoardMessages(string messageToDisplay)
        {
            winLeaderBoardMessageTextRef.gameObject.SetActive(true);
            winLeaderBoardMessageTextRef.text = messageToDisplay;
            yield return new WaitForSeconds(3f);
            winLeaderBoardMessageTextRef.gameObject.SetActive(false);
        }
        #endregion
        #region Unity Functions
        void Start()
        {
            SetUpWinScoreLeaderBoardButtons();

        }
        private void OnEnable()
        {
            LoadWinsGlobalLeaderBoard();
            LoadAndStoreCurrentPlayerData();
        }
        #endregion
    }
}

