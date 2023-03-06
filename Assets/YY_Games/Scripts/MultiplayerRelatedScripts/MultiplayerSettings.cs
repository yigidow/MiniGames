using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YY_Games_Scripts
{
    public class MultiplayerSettings : MonoBehaviour
    {
        //Singleton
        public static MultiplayerSettings instance;

        [Header("Multiplayer Settings")]
        //When true, tells the game that this game will uttilize the delay start feature
        public bool delayStart = true;

        public int maxPlayer = 2;

        //References To scenes
        public int mainMenuScene = 1, multiplayerScene = 3;

        #region Unity Functions
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                if (instance != this)
                {
                    Destroy(instance.gameObject);
                    instance = this;
                }
            }
            DontDestroyOnLoad(gameObject);
        }
        #endregion
    }
}
