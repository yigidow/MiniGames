//using Photon.Pun;
//using System.Collections;
//using System.IO;
//using UnityEngine;
//using UnityEngine.SceneManagement;
//using TMPro;

//namespace YY_Games_Scripts
//{
//    public class PhotonNetworkPlayer : MonoBehaviour
//    {
//        #region References and objects
//        [Header("Photon Network Variables")]
//        [SerializeField] private PhotonView thisPhotonView;
//        public int playerNo;
//        public GameObject playerAvatar;
//        [Header("Disconnect Screen Variables")]
//        [SerializeField] private GameObject disconnectionScreen;
//        [Header("Win Loss Screen Variables")]
//        [SerializeField] private GameObject winLoseScreen;
//        [SerializeField] private TextMeshProUGUI winLoseText;
//        [Header("Ready Go Screen Variables")]
//        [SerializeField] private GameObject readyGoScreen;
//        [SerializeField] private TextMeshProUGUI readyGoText;
//        #endregion
//        #region Photon Network Functions
//        private void SpawnPlayerAvatar()
//        {
//            if (thisPhotonView.IsMine == false) return;

//            int currentCharKey = PlayerPrefs.GetInt(CharacterSelector.CharacterKey);

//            // 0 = adventurer , 1 = female, 2 = player, 3 = soldier
//            switch (currentCharKey)
//            {
//                case 0:
//                    playerAvatar = PhotonNetwork.Instantiate(
//                    Path.Combine("PhotonPrefabs", "AdventurerAvatar"),
//                    Vector3.zero,
//                    Quaternion.identity,
//                    0
//                    );
//                    break;
//                case 1:
//                    playerAvatar = PhotonNetwork.Instantiate(
//                    Path.Combine("PhotonPrefabs", "FemaleAvatar"),
//                    Vector3.zero,
//                    Quaternion.identity,
//                    0
//                    );
//                    break;
//                case 2:
//                    playerAvatar = PhotonNetwork.Instantiate(
//                    Path.Combine("PhotonPrefabs", "PlayerAvatar"),
//                    Vector3.zero,
//                    Quaternion.identity,
//                    0
//                    );
//                    break;
//                case 3:
//                    playerAvatar = PhotonNetwork.Instantiate(
//                    Path.Combine("PhotonPrefabs", "SoldierAvatar"),
//                    Vector3.zero,
//                    Quaternion.identity,
//                    0
//                    );
//                    break;

//                default:
//                    playerAvatar = PhotonNetwork.Instantiate(
//                    Path.Combine("PhotonPrefabs", "AdventurerAvatar"),
//                    Vector3.zero,
//                    Quaternion.identity,
//                    0
//                    );
//                    break;
//            }

//            SetScriptToAvatar();
//        }
//        public void SetPlayerNo(int numberToSet)
//        {
//            playerNo = numberToSet;
//            thisPhotonView.RPC("RPC_SetPlayerNoInNetwork", target: RpcTarget.OthersBuffered, playerNo);
//        }
//        [PunRPC]
//        private void RPC_SetPlayerNoInNetwork(int numberToSet)
//        {
//            playerNo = numberToSet;
//        }
//        private void SetScriptToAvatar()
//        {
//            PhotonPlayerAvatar avatarScript = playerAvatar.GetComponent<PhotonPlayerAvatar>();

//            avatarScript.photonNetworkPlayer = this;
//        }
//        #endregion
//        #region Disconnection
//        public void EnableDisconnectionScreen()
//        {
//            if (disconnectionScreen != null)
//            {
//                disconnectionScreen.SetActive(true);
//                StartCoroutine(ReturnToMainMenuWhenAPlayerLeaves());
//            }
//        }
//        private IEnumerator ReturnToMainMenuWhenAPlayerLeaves()
//        {
//            yield return new WaitForSeconds(2.5f);
//            SceneManager.LoadScene(1, LoadSceneMode.Single);
//        }
//        #endregion
//        #region Win Loss Screen
//        public void EnableWinLoseScreen(string winOrLose)
//        {
//            StartCoroutine(Routine_EnableWinLoseScreen(winOrLose));
//        }
//        private IEnumerator Routine_EnableWinLoseScreen(string winOrLose)
//        {
//            if (PhotonNetwork.IsMasterClient == false) yield break;
//            winLoseScreen.SetActive(true);
//            winLoseText.text = winOrLose;
//            yield return new WaitForSeconds(4f);

//            SceneManager.LoadScene(1, LoadSceneMode.Single);
//        }
//        #endregion
//        #region Ready Go Screen
//        //Wait all players to spawn
//        private IEnumerator Routine_EnableReadySetGoScreen()
//        {
//            GameObject[] players = GameObject.FindGameObjectsWithTag("PhotonPlayer");

//            while(players.Length < 2)
//            {
//                players = GameObject.FindGameObjectsWithTag("PhotonPlayer");
//                yield return new WaitForSeconds(0.01f);
//            }

//            foreach(GameObject player in players)
//            {
//                player.GetComponent<PhotonView>().RPC("RPC_EnableReadyGoScreen", RpcTarget.AllBufferedViaServer);
//            }
//            yield return new WaitForSeconds(3f);
//        }

//        [PunRPC]
//        private void RPC_EnableReadyGoScreen()
//        {
//            if (thisPhotonView.IsMine == false) return;
//            readyGoScreen.SetActive(true);
//            StartCoroutine(Routine_ChaneTextToGo());
//        }
//        private IEnumerator Routine_ChaneTextToGo()
//        {
//            yield return new WaitForSeconds(2f);
//            readyGoText.text = "GO!";
//            PhotonPlayerAvatar photonPlayerAvatar = playerAvatar.GetComponent<PhotonPlayerAvatar>();

//            //Starting Race
//            photonPlayerAvatar.SetToIdleStateAndStartLevelTimer();
//            yield return new WaitForSeconds(1f);
//            readyGoScreen.SetActive(false);
//        }

//        #endregion
//        #region Unity Functions
//        void Start()
//        {
//            SpawnPlayerAvatar();
//            StartCoroutine(Routine_EnableReadySetGoScreen());
//        }
//        #endregion
//    }

//}
