using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


namespace YY_Games_Scripts
{
    public class MainMenuObjectControl : MonoBehaviour
    {
        #region References to Objects, Scripts and Screen Containers
        [Header("Objects, Scripts and Screen Containers")]
        [SerializeField] private GameObject mainMenuRef;
        [SerializeField] private GameObject leaderBoardSelectionMenuRef;
        [SerializeField] private GameObject leaderBoardWinRecordMenuRef;
        [SerializeField] private GameObject leaderBoardTimeRecordMenuRef;
        [SerializeField] private GameObject friendListMenuRef;
        [SerializeField] private GameObject playerFriendListMenuRef;
        [SerializeField] private GameObject friendRequetsMenuRef;
        [SerializeField] private GameObject searchPlayerToAddMenuRef;
        [Header("Main Menu Buttons")]
        [SerializeField] private Button goOnlineButton;
        [SerializeField] private Button leaderBoardSelectionButton;
        [SerializeField] private Button friendListButton;
        [SerializeField] private Button quitButton;
        [Header("LeaderBoard Buttons")]
        [SerializeField] private Button leaderBoardSelectionBackButton;
        [Header("Win LeaderBoard Buttons")]
        [SerializeField] private Button leaderBoardSelectionWinRecordButton;
        [SerializeField] private Button leaderBoardSelectionWinRecordBackButton;
        [Header("Time LeaderBoard Buttons")]
        [SerializeField] private Button leaderBoardSelectionTimeRecordButton;
        [SerializeField] private Button leaderBoardSelectionTimeRecordBackButton;
        [Header("Friend List Buttons")]
        //[SerializeField] private Button playerFriendListButton;
        [SerializeField] private Button friendListMenuBackButton;
        [Header("Friend Request Menu Buttons")]
        [SerializeField] private Button playerFriendRequestButton;
        [SerializeField] private Button friendRequetsMenuBackButton;
        [Header("Add Friend Menu Buttons")]
        [SerializeField] private Button searchPlayerToAddMenuButton;
        [SerializeField] private Button searchPlayerToAddMenuBackButton;
        #endregion
        #region Main Menu Button Functions
        private void OnMainMenuGoOnlineButtonClick()
        {
            SceneManager.LoadScene(2, LoadSceneMode.Single);
            AudioManagerForMainMenu.instance.PlayClickSound();
        }
        private void OnMainMenuLeaderBoardButtonClick()
        {
            leaderBoardSelectionMenuRef.SetActive(true);
            mainMenuRef.SetActive(false);
            AudioManagerForMainMenu.instance.PlayClickSound();
        }
        private void OnMainMenuFriendListButtonClick()
        {
            friendListMenuRef.SetActive(true);
            mainMenuRef.SetActive(false);
            AudioManagerForMainMenu.instance.PlayClickSound();
        }
        private void SetUpMainMenuButtons()

        {
            //Arrenging Multiplayrt Menu Buttons
            goOnlineButton.onClick.AddListener(OnMainMenuGoOnlineButtonClick);
            //Arrenging LeaderBoard Selection Menu Buttons
            leaderBoardSelectionButton.onClick.AddListener(OnMainMenuLeaderBoardButtonClick);
            leaderBoardSelectionBackButton.onClick.AddListener(OnLeaderboardSelectionMenuBackButtonClick);
            leaderBoardSelectionWinRecordButton.onClick.AddListener(OnLeaderBoardSelectionMenuWinRecordButtonClick);
            leaderBoardSelectionTimeRecordButton.onClick.AddListener(OnLeaderBoardSelectionMenuTimeRecordButtonClick);
            leaderBoardSelectionWinRecordBackButton.onClick.AddListener(OnWinLeaderBoardBackButtonClick);
            leaderBoardSelectionTimeRecordBackButton.onClick.AddListener(OnTimeLeaderBoardBackButtonClick);
            //Arrenging FriendList Buttons
            friendListButton.onClick.AddListener(OnMainMenuFriendListButtonClick);
            friendListMenuBackButton.onClick.AddListener(OnFriendListMenuBackButtonClick);
            playerFriendRequestButton.onClick.AddListener(OnFriendListMenuFriendRequestsButtonClick);
            friendRequetsMenuBackButton.onClick.AddListener(OnFriendRequestMenuBackButtonClick);
            searchPlayerToAddMenuButton.onClick.AddListener(OnFriendListMenuSearchForPlayerToAddButtonClick);
            searchPlayerToAddMenuBackButton.onClick.AddListener(OnFriendListMenuSearchForPlayerToAddBackButtonClick);
            //Quit Button
            quitButton.onClick.AddListener(Application.Quit);
        }

        private void DeactivateOtherScreensExceptMainMenu()
        {
            mainMenuRef.SetActive(true);
            leaderBoardSelectionMenuRef.SetActive(false);
            leaderBoardWinRecordMenuRef.SetActive(false);
            leaderBoardTimeRecordMenuRef.SetActive(false);
            friendListMenuRef.SetActive(false);
        }

        #endregion
        #region LeaderBoard Menu Button Functions
        private void OnLeaderboardSelectionMenuBackButtonClick()
        {
            leaderBoardSelectionMenuRef.SetActive(false);
            mainMenuRef.SetActive(true);

            AudioManagerForMainMenu.instance.PlayClickSound();
        }
        private void OnLeaderBoardSelectionMenuWinRecordButtonClick()
        {
            leaderBoardSelectionMenuRef.SetActive(false);
            leaderBoardWinRecordMenuRef.SetActive(true);

            AudioManagerForMainMenu.instance.PlayClickSound();
        }
        private void OnLeaderBoardSelectionMenuTimeRecordButtonClick()
        {
            leaderBoardSelectionMenuRef.SetActive(false);
            leaderBoardTimeRecordMenuRef.SetActive(true);

            AudioManagerForMainMenu.instance.PlayClickSound();
        }
        private void OnWinLeaderBoardBackButtonClick()
        {
            leaderBoardSelectionMenuRef.SetActive(true);
            leaderBoardWinRecordMenuRef.SetActive(false);

            AudioManagerForMainMenu.instance.PlayClickSound();
        }
        private void OnTimeLeaderBoardBackButtonClick()
        {
            leaderBoardSelectionMenuRef.SetActive(true);
            leaderBoardTimeRecordMenuRef.SetActive(false);

            AudioManagerForMainMenu.instance.PlayClickSound();
        }
        #endregion
        #region Friend List Menu Button Functions
        private void OnFriendListMenuBackButtonClick()
        {
            friendListMenuRef.SetActive(false);
            mainMenuRef.SetActive(true);

            AudioManagerForMainMenu.instance.PlayClickSound();
        }
        private void OnFriendRequestMenuBackButtonClick()
        {
            playerFriendListMenuRef.SetActive(true);
            friendRequetsMenuRef.SetActive(false);

            AudioManagerForMainMenu.instance.PlayClickSound();
        }

        private void OnFriendListMenuFriendRequestsButtonClick()
        {
            playerFriendListMenuRef.SetActive(false);
            friendRequetsMenuRef.SetActive(true);

            AudioManagerForMainMenu.instance.PlayClickSound();
        }
        private void OnFriendListMenuSearchForPlayerToAddButtonClick()
        {
            playerFriendListMenuRef.SetActive(false);
            searchPlayerToAddMenuRef.SetActive(true);

            AudioManagerForMainMenu.instance.PlayClickSound();
        }
        private void OnFriendListMenuSearchForPlayerToAddBackButtonClick()
        {
            playerFriendListMenuRef.SetActive(true);
            searchPlayerToAddMenuRef.SetActive(false);

            AudioManagerForMainMenu.instance.PlayClickSound();
        }

        #endregion
        #region Unity Functions
        private void Start()
        {
            DeactivateOtherScreensExceptMainMenu();
            SetUpMainMenuButtons();
        }
        #endregion
    }
}
      

