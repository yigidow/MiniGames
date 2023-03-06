using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

namespace YY_Games_Scripts
{
    public class PhotonLobby : MonoBehaviourPunCallbacks
    {
        /// <summary>
        /// A Script for the Photon Lobby to start the connection to the Photon Multiplayer Server
        /// </summary>
        #region References to Objects, Scripts and Screen Containers
        //Singleton
        public static PhotonLobby instance;
        //Title
        [SerializeField] private TextMeshProUGUI title;
        [SerializeField] private TextMeshProUGUI messageText;
        [Header("Multiplayer Menu Buttons")]
        [SerializeField] private Button joinRoomButton;
        [SerializeField] private Button backButton;
        [SerializeField] private Button charSelectLeft;
        [SerializeField] private Button charSelectRight;
        #endregion

        #region Photon Functions For Connection
        private void ConnectionToPhotonServerAtStart()
        {
            title.text = ("Connecting to Server...");
            PhotonNetwork.ConnectUsingSettings();

            //Disabling buttons 
            joinRoomButton.interactable = false;
            backButton.interactable = false;
            charSelectLeft.interactable = false;
            charSelectRight.interactable = false;

        }
        public override void OnConnectedToMaster()
        {
            title.text = ("Connected to Server");
            PhotonNetwork.AutomaticallySyncScene = true;

            joinRoomButton.interactable = true;
            backButton.interactable = true;
            charSelectLeft.interactable = true;
            charSelectRight.interactable = true;
        }
        public override void OnDisconnected(DisconnectCause cause)
        {
            base.OnDisconnected(cause);
            title.text = ("Disonnected from Server");
            print(cause);
            joinRoomButton.interactable = false;
        }

        #endregion

        #region Create and Join Room functions

        private void CreateRoom()
        {
            int randomRoomNumber = UnityEngine.Random.Range(0, 10000);

            RoomOptions roomOptions = new RoomOptions() { 
                IsVisible = true,
                IsOpen = true,
                MaxPlayers = 4,
                EmptyRoomTtl = 0,
                PlayerTtl = 0,
            };
            PhotonNetwork.CreateRoom("Room" + randomRoomNumber, roomOptions);
        }
        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            base.OnJoinRandomFailed(returnCode, message);
            StartCoroutine(DisplayMessages(message));
            CreateRoom();
        }
        public override void OnCreatedRoom()
        {
            base.OnCreatedRoom();
            StartCoroutine(DisplayMessages("Room  Created"));
        }
        public override void OnCreateRoomFailed(short returnCode, string message)
        {
            base.OnCreateRoomFailed(returnCode, message);
            StartCoroutine(DisplayMessages(message));
            CreateRoom();
        }
        #endregion

        #region Multiplayer Menu Button Functions
        private void OnJoinRoomButtonClick()
        {
            title.text = ("Searching For A Game");
            joinRoomButton.interactable = false;
            backButton.interactable = false;
            charSelectLeft.interactable = false;
            charSelectRight.interactable = false;
            //Try to join a random room
            PhotonNetwork.JoinRandomRoom();
            backButton.interactable = true;
            AudioManagerForMainMenu.instance.PlayClickSound();
        }
        private void OnBackButtonClick()
        {
            SceneManager.LoadScene(1, LoadSceneMode.Single);
            joinRoomButton.interactable = true;
            charSelectLeft.interactable = true;
            charSelectRight.interactable = true;
            AudioManagerForMainMenu.instance.PlayClickSound();
        }
        private void AddButtonFunctions()
        {
            joinRoomButton.onClick.AddListener(OnJoinRoomButtonClick);
            backButton.onClick.AddListener(OnBackButtonClick);
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
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else if (instance != this)
            {
                Destroy(gameObject);
            }
            
            //DontDestroyOnLoad(gameObject);
        }
        private void Start()
        {
            ConnectionToPhotonServerAtStart();
            AddButtonFunctions();
        }
        #endregion
    }
}

