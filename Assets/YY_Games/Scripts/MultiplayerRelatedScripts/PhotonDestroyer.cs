//using Photon.Pun;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//namespace YY_Games_Scripts
//{
//    public class PhotonDestroyer : MonoBehaviour
//    {
//        private IEnumerator DisconnectAndDestroyPhotonObjects()
//        {
//            while (PhotonNetwork.InRoom)
//            {
//                PhotonNetwork.LeaveRoom();
//                yield return new WaitForSeconds(0.05f);
//            }

//            while (PhotonNetwork.IsConnected)
//            {
//                PhotonNetwork.Disconnect();
//                yield return new WaitForSeconds(0.05f);
//            }

//            if (PhotonRoom.instance)
//            {
//                Destroy(PhotonRoom.instance.gameObject);
//            }

//            if (MultiplayerSettings.instance)
//            {
//                Destroy(MultiplayerSettings.instance.gameObject);
//            }

//        }   
    
//        void Start() 
//        {
//            StartCoroutine(DisconnectAndDestroyPhotonObjects());
//        }
//    }
//}

