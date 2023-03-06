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
    public class TimeScoreLeaderBoardMenu : MonoBehaviour
    {
        #region Button References
        [Header("Button References")]

        [SerializeField] private Button globalRankButton;
        [SerializeField] private Button playerRankButton;
        [SerializeField] private Button friendRankButton;

        [SerializeField] private Button showLevel1TimeScoreButton;
        [SerializeField] private Button showLevel2TimeScoreButton;
        [SerializeField] private Button timeLeaderBoardMenuBackButton;
        #endregion

        #region Object and Component References
        [Header("Object and Component References")]
        [SerializeField] private GameObject timeLeaderBoardScrollViewRef;
        //Message texts to inform the player about connection
        [SerializeField] private TextMeshProUGUI timeLeaderBoardMessageTextRef;
        // Leaderboard Prefab Ref
        [SerializeField] private GameObject leaderBoardEntryPrefabRef;

        private string currentPlayerPlayFabId = "";
        #endregion

        #region Button Functions
        private void DisableButtonInteractions()
        {
            playerRankButton.interactable = false;
            friendRankButton.interactable = false;
            globalRankButton.interactable = false;
            timeLeaderBoardMenuBackButton.interactable = false;
        }
        private void EnableButtonInteractions()
        {
            playerRankButton.interactable = true;
            friendRankButton.interactable = true;
            globalRankButton.interactable = true;
            timeLeaderBoardMenuBackButton.interactable = true;
        }

        private void SetTimeScoreButtons()
        {
            globalRankButton.onClick.AddListener(LoadTimeForLevel1GlobalLeaderBoard);
            playerRankButton.onClick.AddListener(LoadTimeForLevel1AroundPlayerlLeaderBoard);
            friendRankButton.onClick.AddListener(LoadTimeForLevel1FriendListlLeaderBoard);
        }
        private void SetTimeScoreButtonsForTimeChangesGlobal()
        {
            showLevel1TimeScoreButton.onClick.RemoveAllListeners();
            showLevel2TimeScoreButton.onClick.RemoveAllListeners();

            showLevel1TimeScoreButton.onClick.AddListener(LoadTimeForLevel1GlobalLeaderBoard);
            showLevel2TimeScoreButton.onClick.AddListener(LoadTimeForLevel2GlobalLeaderBoard);
        }
        private void SetTimeScoreButtonsForTimeChangesPlayer()
        {
            showLevel1TimeScoreButton.onClick.RemoveAllListeners();
            showLevel2TimeScoreButton.onClick.RemoveAllListeners();

            showLevel1TimeScoreButton.onClick.AddListener(LoadTimeForLevel1AroundPlayerlLeaderBoard);
            showLevel2TimeScoreButton.onClick.AddListener(LoadTimeForLevel2AroundPlayerlLeaderBoard);
        }
        private void SetTimeScoreButtonsForTimeChangesFriendList()
        {
            showLevel1TimeScoreButton.onClick.RemoveAllListeners();
            showLevel2TimeScoreButton.onClick.RemoveAllListeners();

            showLevel1TimeScoreButton.onClick.AddListener(LoadTimeForLevel1FriendListlLeaderBoard);
            showLevel2TimeScoreButton.onClick.AddListener(LoadTimeForLevel2FriendListlLeaderBoard);
        }
        #endregion

        #region Functions for Loading LeaderBoard
        //Request to fetch Level 1 Score LeaderBoard from PlayFab Server
        private void LoadTimeForLevel1GlobalLeaderBoard()
        {
            DestroyAllPreviousLeaderBoardEntries();
            DisableButtonInteractions();
            SetTimeScoreButtonsForTimeChangesGlobal();

            var leaderBoardFetchRequest = new GetLeaderboardRequest
            {
                StatisticName = "Level 1 Record Times",
                MaxResultsCount = 10
            };

            PlayFabClientAPI.GetLeaderboard(
                leaderBoardFetchRequest,
                resultCallback: result =>
                {
                    //Storing Leaderboard List
                    var leaderBoardResultList = result.Leaderboard;
                    leaderBoardResultList.Reverse();

                    foreach (PlayerLeaderboardEntry playerLeaderboardEntry in leaderBoardResultList)
                    {
                        //Creating a Leaderboard Entry
                        var createdLeaderBoardEntry = Instantiate(leaderBoardEntryPrefabRef, timeLeaderBoardScrollViewRef.transform);
                        // Filling the info from server
                        LeaderBoardPrefabForScroll createdLeaderBoardEntryScript = createdLeaderBoardEntry.GetComponent<LeaderBoardPrefabForScroll>();

                        //Converting StatValue into mins and seconds
                        var statVal = playerLeaderboardEntry.StatValue;
                        var minutesInStat = statVal / 60;
                        var secondsInStat = statVal % 60;

                        //To make ranks start from 1 not 0, set the values and rearange the order
                        int positionOfPlayer = leaderBoardResultList[0].Position - playerLeaderboardEntry.Position;
                        var currentPlayerPos = positionOfPlayer + 1;
           
                        createdLeaderBoardEntryScript.SetValuesForLeaderBoard(
                        playerLeaderboardEntry.DisplayName,
                        currentPlayerPos.ToString(),
                        $"{minutesInStat} : {secondsInStat}"
                        );
                    }
                    EnableButtonInteractions();
                    timeLeaderBoardMessageTextRef.text = ("Global Level 1 Time Records");
                },
                errorCallback: error =>
                {
                    StartCoroutine(DisplayLeaderBoardMessages(error.ErrorMessage));
                    print(error.ErrorMessage);
                    EnableButtonInteractions();
                }
            );

        }
        //Request to fetch Level 2 Score LeaderBoard from PlayFab Server
        private void LoadTimeForLevel2GlobalLeaderBoard()
        {
            DestroyAllPreviousLeaderBoardEntries();
            DisableButtonInteractions();

            var leaderBoardFetchRequest = new GetLeaderboardRequest
            {
                StatisticName = "Level 2 Record Times",
                MaxResultsCount = 10
            };

            PlayFabClientAPI.GetLeaderboard(
                leaderBoardFetchRequest,
                resultCallback: result =>
                {
                    //Storing Leaderboard List
                    var leaderBoardResultList = result.Leaderboard;
                    leaderBoardResultList.Reverse();

                    foreach (PlayerLeaderboardEntry playerLeaderboardEntry in leaderBoardResultList)
                    {
                        //Creating a Leaderboard Entry
                        var createdLeaderBoardEntry = Instantiate(leaderBoardEntryPrefabRef, timeLeaderBoardScrollViewRef.transform);
                        // Filling the info from server
                        LeaderBoardPrefabForScroll createdLeaderBoardEntryScript = createdLeaderBoardEntry.GetComponent<LeaderBoardPrefabForScroll>();


                        //Converting StatValue into mins and seconds
                        var statVal = playerLeaderboardEntry.StatValue;
                        var minutesInStat = statVal / 60;
                        var secondsInStat = statVal % 60;

                        //To make ranks start from 1 not 0, set the values and rearange the order
                        int positionOfPlayer = leaderBoardResultList[0].Position - playerLeaderboardEntry.Position;
                        var currentPlayerPos = positionOfPlayer + 1;

                        createdLeaderBoardEntryScript.SetValuesForLeaderBoard(
                        playerLeaderboardEntry.DisplayName,
                        currentPlayerPos.ToString(),
                        $"{minutesInStat} : {secondsInStat}"
                        );
                    }
                    EnableButtonInteractions();
                    timeLeaderBoardMessageTextRef.text = ("Global Level 2 Time Records");
                },
                errorCallback: error =>
                {
                    StartCoroutine(DisplayLeaderBoardMessages(error.ErrorMessage));
                    print(error.ErrorMessage);
                    EnableButtonInteractions();
                }
            );

        }
        private void LoadTimeForLevel1AroundPlayerlLeaderBoard()
        {
            DestroyAllPreviousLeaderBoardEntries();
            DisableButtonInteractions();
            SetTimeScoreButtonsForTimeChangesPlayer();

            var leaderBoardFetchRequest = new GetLeaderboardAroundPlayerRequest
            {
                StatisticName = "Level 1 Record Times",
                MaxResultsCount = 10
            };

            PlayFabClientAPI.GetLeaderboardAroundPlayer(
                leaderBoardFetchRequest,
                resultCallback: result =>
                {
                    //Storing Leaderboard List
                    var leaderBoardResultList = result.Leaderboard;
                    leaderBoardResultList.Reverse();

                    foreach (PlayerLeaderboardEntry playerLeaderboardEntry in leaderBoardResultList)
                    {
                        //Creating a Leaderboard Entry
                        var createdLeaderBoardEntry = Instantiate(leaderBoardEntryPrefabRef, timeLeaderBoardScrollViewRef.transform);
                        // Filling the info from server
                        LeaderBoardPrefabForScroll createdLeaderBoardEntryScript = createdLeaderBoardEntry.GetComponent<LeaderBoardPrefabForScroll>();

                        //Converting StatValue into mins and seconds
                        var statVal = playerLeaderboardEntry.StatValue;
                        var minutesInStat = statVal / 60;
                        var secondsInStat = statVal % 60;

                        //To make ranks start from 1 not 0, set the values and rearange the order
                        int positionOfPlayer = leaderBoardResultList[0].Position - playerLeaderboardEntry.Position;
                        var currentPlayerPos = positionOfPlayer + 1;

                        createdLeaderBoardEntryScript.SetValuesForLeaderBoard(
                        playerLeaderboardEntry.DisplayName,
                        currentPlayerPos.ToString(),
                        $"{minutesInStat} : {secondsInStat}"
                        );
                    }
                    EnableButtonInteractions();
                    timeLeaderBoardMessageTextRef.text = ("Player Level 1 Time Records");
                },
                errorCallback: error =>
                {
                    StartCoroutine(DisplayLeaderBoardMessages(error.ErrorMessage));
                    print(error.ErrorMessage);
                    EnableButtonInteractions();
                }
            );

        }
        private void LoadTimeForLevel2AroundPlayerlLeaderBoard()
        {
            DestroyAllPreviousLeaderBoardEntries();
            DisableButtonInteractions();

            var leaderBoardFetchRequest = new GetLeaderboardAroundPlayerRequest
            {
                StatisticName = "Level 2 Record Times",
                MaxResultsCount = 10
            };

            PlayFabClientAPI.GetLeaderboardAroundPlayer(
                leaderBoardFetchRequest,
                resultCallback: result =>
                {
                    //Storing Leaderboard List
                    var leaderBoardResultList = result.Leaderboard;
                    leaderBoardResultList.Reverse();

                    foreach (PlayerLeaderboardEntry playerLeaderboardEntry in leaderBoardResultList)
                    {
                        //Creating a Leaderboard Entry
                        var createdLeaderBoardEntry = Instantiate(leaderBoardEntryPrefabRef, timeLeaderBoardScrollViewRef.transform);
                        // Filling the info from server
                        LeaderBoardPrefabForScroll createdLeaderBoardEntryScript = createdLeaderBoardEntry.GetComponent<LeaderBoardPrefabForScroll>();

                        //Converting StatValue into mins and seconds
                        var statVal = playerLeaderboardEntry.StatValue;
                        var minutesInStat = statVal / 60;
                        var secondsInStat = statVal % 60;

                        //To make ranks start from 1 not 0, set the values and rearange the order
                        int positionOfPlayer = leaderBoardResultList[0].Position - playerLeaderboardEntry.Position;
                        var currentPlayerPos = positionOfPlayer + 1;


                        createdLeaderBoardEntryScript.SetValuesForLeaderBoard(
                        playerLeaderboardEntry.DisplayName,
                        currentPlayerPos.ToString(),
                        $"{minutesInStat} : {secondsInStat}"
                        );
                    }
                    EnableButtonInteractions();
                    timeLeaderBoardMessageTextRef.text = ("Player Level 2 Time Records");
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
        private void LoadTimeForLevel1FriendListlLeaderBoard()
        {
            SetTimeScoreButtonsForTimeChangesFriendList();
            var listOfConfirmedFriends = new List<string>();
            var friendListRequest = new GetFriendsListRequest();

            PlayFabClientAPI.GetFriendsList(
                friendListRequest,
                resultCallback: result =>
                {

                    foreach (FriendInfo friendInfo in result.Friends)
                    {
                        if (friendInfo.Tags[0] == "confirmed")
                        {
                            listOfConfirmedFriends.Add(friendInfo.FriendPlayFabId);
                        }
                    }
                    listOfConfirmedFriends.Add(currentPlayerPlayFabId);
                    LoadConfirmedFriendListForLevel1LeaderBoard(listOfConfirmedFriends);
                },
                errorCallback: error =>
                {
                    StartCoroutine(DisplayLeaderBoardMessages(error.ErrorMessage));
                    print(error.ErrorMessage);
                    EnableButtonInteractions();
                }
            );
        }
        private void LoadConfirmedFriendListForLevel1LeaderBoard(List<string> confirmedFriendList)
        {
            DestroyAllPreviousLeaderBoardEntries();
            DisableButtonInteractions();

            var friendLeaderboardRequest = new GetFriendLeaderboardRequest
            {
                StatisticName = "Level 1 Record Times",
            };
            PlayFabClientAPI.GetFriendLeaderboard(
            friendLeaderboardRequest,
            resultCallback: result =>
            {
                //Storing Leaderboard List
                var leaderBoardResultList = result.Leaderboard;
                leaderBoardResultList.Reverse();

                foreach (PlayerLeaderboardEntry playerLeaderboardEntry in result.Leaderboard)
                {
                    if (confirmedFriendList.Contains(playerLeaderboardEntry.PlayFabId) == false) continue;

                    var createdLeaderBoardEntry = Instantiate(leaderBoardEntryPrefabRef, timeLeaderBoardScrollViewRef.transform);
                    LeaderBoardPrefabForScroll createdLeaderBoardEntryScript = createdLeaderBoardEntry.GetComponent<LeaderBoardPrefabForScroll>();

                    //Converting StatValue into mins and seconds
                    var statVal = playerLeaderboardEntry.StatValue;
                    var minutesInStat = statVal / 60;
                    var secondsInStat = statVal % 60;

                    //To make ranks start from 1 not 0, set the values and rearange the order
                    int positionOfPlayer = leaderBoardResultList[0].Position - playerLeaderboardEntry.Position;
                    var currentPlayerPos = positionOfPlayer + 1;


                    createdLeaderBoardEntryScript.SetValuesForLeaderBoard(
                    playerLeaderboardEntry.DisplayName,
                    currentPlayerPos.ToString(),
                    $"{minutesInStat} : {secondsInStat}"
              
                    );
               
                }
                EnableButtonInteractions();
                timeLeaderBoardMessageTextRef.text = ("Friend List Level 1 Time Records");
            },
            errorCallback: error =>
            {
                StartCoroutine(DisplayLeaderBoardMessages(error.ErrorMessage));
                print(error.ErrorMessage);
                EnableButtonInteractions();
            }
        );
        }
        private void LoadTimeForLevel2FriendListlLeaderBoard()
        {
            var listOfConfirmedFriends = new List<string>();
            var friendListRequest = new GetFriendsListRequest();

            PlayFabClientAPI.GetFriendsList(
                friendListRequest,
                resultCallback: result =>
                {

                    foreach (FriendInfo friendInfo in result.Friends)
                    {
                        if (friendInfo.Tags[0] == "confirmed")
                        {
                            listOfConfirmedFriends.Add(friendInfo.FriendPlayFabId);
                        }
                    }
                    listOfConfirmedFriends.Add(currentPlayerPlayFabId);
                    LoadConfirmedFriendListForLevel2LeaderBoard(listOfConfirmedFriends);
                },
                errorCallback: error =>
                {
                    StartCoroutine(DisplayLeaderBoardMessages(error.ErrorMessage));
                    print(error.ErrorMessage);
                    EnableButtonInteractions();
                }
            );
        }
        private void LoadConfirmedFriendListForLevel2LeaderBoard(List<string> confirmedFriendList)
        {
            DestroyAllPreviousLeaderBoardEntries();
            DisableButtonInteractions();

            var friendLeaderboardRequest = new GetFriendLeaderboardRequest
            {
                StatisticName = "Level 2 Record Times",
            };
            PlayFabClientAPI.GetFriendLeaderboard(
            friendLeaderboardRequest,
            resultCallback: result =>
            {
                //Storing Leaderboard List
                var leaderBoardResultList = result.Leaderboard;
                leaderBoardResultList.Reverse();

                foreach (PlayerLeaderboardEntry playerLeaderboardEntry in result.Leaderboard)
                {
                    if (confirmedFriendList.Contains(playerLeaderboardEntry.PlayFabId) == false) continue;

                    var createdLeaderBoardEntry = Instantiate(leaderBoardEntryPrefabRef, timeLeaderBoardScrollViewRef.transform);
                    LeaderBoardPrefabForScroll createdLeaderBoardEntryScript = createdLeaderBoardEntry.GetComponent<LeaderBoardPrefabForScroll>();

                    //Converting StatValue into mins and seconds
                    var statVal = playerLeaderboardEntry.StatValue;
                    var minutesInStat = statVal / 60;
                    var secondsInStat = statVal % 60;

                    //To make ranks start from 1 not 0, set the values and rearange the order
                    int positionOfPlayer = leaderBoardResultList[0].Position - playerLeaderboardEntry.Position;
                    var currentPlayerPos = positionOfPlayer + 1;


                    createdLeaderBoardEntryScript.SetValuesForLeaderBoard(
                    playerLeaderboardEntry.DisplayName,
                    currentPlayerPos.ToString(),
                    $"{minutesInStat} : {secondsInStat}"

                    );

                }
                EnableButtonInteractions();
                timeLeaderBoardMessageTextRef.text = ("Friend List Level 2 Time Records");
            },
            errorCallback: error =>
            {
                StartCoroutine(DisplayLeaderBoardMessages(error.ErrorMessage));
                print(error.ErrorMessage);
                EnableButtonInteractions();
            }
        );
        }
        //To destroy already created prefabs to not load same prefab
        private void DestroyAllPreviousLeaderBoardEntries()
        {
            if (timeLeaderBoardScrollViewRef.transform.childCount == 0)
            {
                return;
            }
            for (int i = 0; i < timeLeaderBoardScrollViewRef.transform.childCount; i++)
            {
                Destroy(timeLeaderBoardScrollViewRef.transform.GetChild(i).gameObject);
            }
        }
        #endregion
        #region Message Functions
        //Message To show when login or register is failed or succeeds  
        private IEnumerator DisplayLeaderBoardMessages(string messageToDisplay)
        {
            timeLeaderBoardMessageTextRef.gameObject.SetActive(true);
            timeLeaderBoardMessageTextRef.text = messageToDisplay;
            yield return new WaitForSeconds(3f);
            timeLeaderBoardMessageTextRef.gameObject.SetActive(false);
        }
        #endregion
        #region Unity Functions
        void Start()
        {
            SetTimeScoreButtons();
        }
        private void OnEnable()
        {
            LoadTimeForLevel1GlobalLeaderBoard();
            LoadAndStoreCurrentPlayerData();
        }
        #endregion
    }
}