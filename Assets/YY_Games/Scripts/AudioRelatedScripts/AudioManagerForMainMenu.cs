using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace YY_Games_Scripts
{
    public class AudioManagerForMainMenu : MonoBehaviour
    {
        #region References and objects
        //Singleton
        public static AudioManagerForMainMenu instance;
        [Header("For Sfx")]
        [SerializeField] private AudioSource ClickSfx;

        [Header("For Music")]
        [SerializeField] private AudioSource mainMenuMusic;

        [Header("Audio Clips")]
        [SerializeField] private AudioClip[] musicClips;
        #endregion

        #region Audio Manager Functions
        //Changing Background music when scene changed
        private void ChangeMusicForScenes(Scene newScene, LoadSceneMode newSceneMode)
        {
            int sceneIndex = newScene.buildIndex;

            switch (sceneIndex)
            {
                // Music for login / Register Menu
                case 0:
                    mainMenuMusic.Stop();
                    mainMenuMusic.clip = musicClips[0];
                    mainMenuMusic.Play();
                    break;
                // Music for Main Menu
                case 1:
                    mainMenuMusic.Stop();
                    mainMenuMusic.clip = musicClips[1];
                    mainMenuMusic.Play();
                    break;
            }
        }

        //PLaying the Click SFX When a button clicked
        public void PlayClickSound()
        {
            ClickSfx.Play();
        }

        #endregion

        #region Unity Functions

        private void Awake()
        {
            if(instance == null)
            {
                instance = this;
            }
            else
            {
                if(instance != this)
                {
                    Destroy(instance.gameObject);
                    instance = this; 
                }
            }
            DontDestroyOnLoad(gameObject);
        }
        void Start()
        {
            SceneManager.sceneLoaded += ChangeMusicForScenes;
        }


        void Update()
        {

        }

        #endregion

    }
}

