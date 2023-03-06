//using System.Collections;
//using UnityEngine;
//using TMPro;
//using Photon.Pun;
//using Photon.Realtime;
//using UnityEngine.SceneManagement;
//using System.IO;

//namespace YY_Games_Scripts
//{
//    /// <summary>
//    /// Code piece to connect players and handle the delay at start
//    /// </summary>
//    public class PhotonRoom : MonoBehaviourPunCallbacks, IInRoomCallbacks
//    {
//        #region References to Objects, Scripts and Screen Containers
//        //Singleton
//        public static PhotonRoom instance;

//        //Title and messageText
//        //[Header("Title & MessageText")]
//        //[SerializeField] private TextMeshProUGUI title;
//        //[SerializeField] private TextMeshProUGUI messageText;

//        [Header("PhotonView")]
//        [SerializeField] private PhotonView thisPhotonView;

//        [Header("Variables to Control Rooms")]

//        // Level That is loaded
//        public int levelCreated = 0;

//        //Players in room
//        private int playersInRoom;

//        //This players's number in room 
//        private int numberInRoom;

//        //Players in game
//        private int playersInGame;

//        //Starting time
//        private float startingTime = 2f;

//        //Max player count
//        private float maxPlayers;
//        private float lessThenMaxPlayers;

//        //When true multiplayer scene is loaded
//        private bool multiSceneIsLoaded;

//        //Current Scene
//        private int currentScene;

//        //When true game ready to count
//        private bool readyToCountDown;

//        //When true game ready to start
//        private bool readyToStart;

//        //Stores the time to start the game
//        private float timeToStartGame;

//        //List of Photon Players
//        private Player[] photonPlayers;

//        //Ref To spawned photon network player
//        //private PhotonNetworkPlayer photonNetworkPlayer;

//        #endregion
//        #region Room Control Functions

//        //if only 1 player in the room
//        private void RestartTimer()
//        {
//            lessThenMaxPlayers = startingTime;
//            timeToStartGame = startingTime;
//            maxPlayers = 2;
//            readyToCountDown = false;
//            readyToStart = false;
//        }
//        private void SetRoomControlVariables()
//        {
//            readyToCountDown = false;
//            readyToStart = false;
//            lessThenMaxPlayers = startingTime;
//            maxPlayers = 2;
//            timeToStartGame = startingTime;
//            multiSceneIsLoaded = false;
//        } 
//        private void StartGame()
//        {
//            multiSceneIsLoaded = true;

//            if (PhotonNetwork.IsMasterClient ==false)
//            {
//                return;
//            }
//            if(MultiplayerSettings.instance.delayStart == true)
//            {
//                PhotonNetwork.CurrentRoom.IsOpen = false;
//            }

//            PhotonNetwork.LoadLevel(MultiplayerSettings.instance.multiplayerScene);

//        }
//        private void GameStartCountTime()
//        {
//            if(MultiplayerSettings.instance.delayStart == true)
//            {
//                if (PhotonNetwork.InRoom)
//                {
//                    if(readyToCountDown == false)
//                    {
//                        //title.text = "Searching for a game...";
//                    }
//                    if(photonPlayers.Length == MultiplayerSettings.instance.maxPlayer)
//                    {
//                        //title.text = "Game is about to start...";
//                    }
//                }

//                if(playersInRoom == 1)
//                {
//                    RestartTimer();
//                }

//                if(multiSceneIsLoaded == false)
//                {
//                    if(readyToStart == true)
//                    {
//                        maxPlayers -= Time.deltaTime;
//                        lessThenMaxPlayers = maxPlayers;
//                        timeToStartGame = maxPlayers;
//                    }
//                    if(readyToCountDown == true)
//                    {
//                        lessThenMaxPlayers -= Time.deltaTime;
//                        timeToStartGame = lessThenMaxPlayers;
//                        //StartCoroutine(DisplayMessages("Time to Start the game"));
//                    }

//                    if (timeToStartGame <= 0)
//                    {
//                        StartGame();
//                    }
//                }
                
//            }

//        }
//        private void OnSceneFinishedLoading(Scene scene, LoadSceneMode mode)
//        {
//            currentScene = scene.buildIndex;

//            if(currentScene == MultiplayerSettings.instance.multiplayerScene)
//            {
//                multiSceneIsLoaded = true;

//                //Creating the player here
//                if(MultiplayerSettings.instance.delayStart == true)
//                {
//                    //Send RPC to create a player
//                    thisPhotonView.RPC(methodName: "RPC_MultiplayerSceneLoaded", RpcTarget.All);
//                }
//                else
//                {
//                    //Creating the player instantly
//                    RPC_CreatePlayer();
//                }
//            }
//        }
//        private void HandleJoiningOfThisPlayerInTheRoom()
//        {
//            //StartCoroutine(DisplayMessages("Joined a Room"));

//            //Setting up Room variables
//            photonPlayers = PhotonNetwork.PlayerList;
//            playersInRoom++;
//            playersInGame = photonPlayers.Length;

//            //Setting up this player's number in room
//            numberInRoom = playersInGame;

//            //If delay start setting on
//            if(MultiplayerSettings.instance.delayStart == true)
//            {
//                if(playersInRoom > 1)
//                {
//                    readyToCountDown = true;
//                }
//                if(playersInRoom == MultiplayerSettings.instance.maxPlayer)
//                {
//                    readyToStart = true;

//                    //title.text = "Game is about to start...";

//                    if (PhotonNetwork.IsMasterClient == false)
//                    {
//                        return;
//                    }
//                    if (MultiplayerSettings.instance.delayStart == true)
//                    {
//                        PhotonNetwork.CurrentRoom.IsOpen = false;
//                    }
//                }
//            }
//            else
//            {
//                StartGame();
//            }
//        }
//        private void HandleJoiningOfAnotherPlayerInTheRoom()
//        {
//            //StartCoroutine(DisplayMessages("A Player Joined The Room"));

//            photonPlayers = PhotonNetwork.PlayerList;
//            playersInRoom++;

//            if (MultiplayerSettings.instance.delayStart == true)
//            {
//                if (playersInRoom > 1)
//                {
//                    readyToCountDown = true;
//                }
//                if (playersInRoom == MultiplayerSettings.instance.maxPlayer)
//                {
//                    readyToStart = true;
//                    //title.text = "Game is about to start...";

//                    if (PhotonNetwork.IsMasterClient == false)
//                    {
//                        return;
//                    }
//                    if (MultiplayerSettings.instance.delayStart == true)
//                    {
//                        PhotonNetwork.CurrentRoom.IsOpen = false;
//                    }
//                }
//            }
//        } 
        
//        private void DisplayTheDisconnectionCanvas()
//        {
//            photonNetworkPlayer.EnableDisconnectionScreen();
//        }

//        //This Func going to be called when game ends from server
//        [PunRPC]
//        private void FinishGame()
//        {
//            GameObject finishLine = GameObject.FindGameObjectWithTag("FinishLine");
//            finishLine.gameObject.SetActive(false);

//            GameObject[] allPlayers = GameObject.FindGameObjectsWithTag("Player");

//            foreach(GameObject player in allPlayers)
//            {
//                PhotonPlayerAvatar photonPlayerAvatar = player.GetComponent<PhotonPlayerAvatar>();
//                photonPlayerAvatar.isFinished = true;
//                photonPlayerAvatar.DisplayWinLoseScreen(); 
//            }
//        }
//        #endregion
//        #region Player Network and Setting the Level Functions
//        [PunRPC]
//        private void RPC_CreatePlayer()
//        {
//             GameObject spawnedPhotonNetworkPlayer = PhotonNetwork.Instantiate(
//             Path.Combine ("PhotonPrefabs", "PhotonNetworkPlayer"),
//             new Vector3(x:0,y:0,z:0),
//             Quaternion.identity,group:0
//             );

//            photonNetworkPlayer = spawnedPhotonNetworkPlayer.GetComponent<PhotonNetworkPlayer>();
//            photonNetworkPlayer.SetPlayerNo(numberInRoom);
//        } 

//        private void SpawnRandomLevel()
//        {
//            int randomLevel = Random.Range(1, 3);
//            thisPhotonView.RPC("RPC_SetLeveLNumber", RpcTarget.All, randomLevel);

//            switch (randomLevel)
//            {
//                case 1 :
//                    PhotonNetwork.Instantiate(
//                    Path.Combine("PhotonPrefabs", "Level_1"),
//                    new Vector3(x: 0, y: 0, z: 0),
//                    Quaternion.identity, group: 0
//                    );
//                    break;
//                case 2:
//                    PhotonNetwork.Instantiate(
//                    Path.Combine("PhotonPrefabs", "Level_2"),
//                    new Vector3(x: 0, y: 0, z: 0),
//                    Quaternion.identity, group: 0
//                    );
//                    break;
//                default:
//                    PhotonNetwork.Instantiate(
//                    Path.Combine("PhotonPrefabs", "Level_1"),
//                    new Vector3(x: 0, y: 0, z: 0),
//                    Quaternion.identity, group: 0
//                    );
//                    break;
//            }
//        }

//        [PunRPC]
//        private void RPC_MultiplayerSceneLoaded()
//        {
//            playersInGame++;
//            if(playersInGame == PhotonNetwork.PlayerList.Length)
//            {
//                if (PhotonNetwork.IsMasterClient)
//                {
//                    SpawnRandomLevel();
//                }
//                thisPhotonView.RPC(methodName: "RPC_CreatePlayer", RpcTarget.All);
//            }
//            Debug.Log("RPC_MultiplayerSceneLoaded");
//        }
//        [PunRPC]
//        private void RPC_SetLeveLNumber(int levelNumber)
//        {
//            levelCreated = levelNumber; 
//        }
//        #endregion
//        #region Photon Override Functions
//        public override void OnEnable()
//        {
//            base.OnEnable();
//            SceneManager.sceneLoaded += OnSceneFinishedLoading;

//            //Add callback target to Photon
//            PhotonNetwork.AddCallbackTarget(target: this);
//        }
//        public override void OnDisable()
//        {
//            base.OnDisable();
//            SceneManager.sceneLoaded -= OnSceneFinishedLoading;

//            //Add callback target to Photon
//            PhotonNetwork.RemoveCallbackTarget(target: this);
//        }
//        public override void OnJoinedRoom()
//        {
//            base.OnJoinedRoom();
//            HandleJoiningOfThisPlayerInTheRoom();
//        }
//        public override void OnPlayerEnteredRoom(Player newPlayer)
//        {
//            base.OnPlayerEnteredRoom(newPlayer);
//            HandleJoiningOfAnotherPlayerInTheRoom();
//        }

//        public override void OnPlayerLeftRoom(Player otherPlayer)
//        {
//            base.OnPlayerLeftRoom(otherPlayer);

//            DisplayTheDisconnectionCanvas();
//        }
//        #endregion
//        //#region Message Functions
//        ////Message To show 
//        //private IEnumerator DisplayMessages(string messageToDisplay)
//        //{
//        //    messageText.gameObject.SetActive(true);
//        //    messageText.text = messageToDisplay;
//        //    yield return new WaitForSeconds(3f);
//        //    messageText.gameObject.SetActive(false);
//        //}
//        //#endregion
//        #region Unity Functions
//        private void Awake()
//        {
//            if (instance == null)
//            {
//                instance = this;
//            }
//            else
//            {
//                Destroy(gameObject);
//            }
//            DontDestroyOnLoad(gameObject);
//        }
//        void Start()
//        {
//            SetRoomControlVariables();
//        }
//        void Update()
//        {
//            GameStartCountTime();
//        }
//        #endregion
//    }
//}
