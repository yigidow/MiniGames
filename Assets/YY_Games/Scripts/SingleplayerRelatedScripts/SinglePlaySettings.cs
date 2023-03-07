using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace YY_Games_Scripts
{
    public class SinglePlaySettings : MonoBehaviour
    {
        #region Single Player Settings Variables
        //Singleton
        public static SinglePlaySettings instance;

        [Header("Single PLayer Settings Components")]
        [SerializeField] private Slider densitySlider;
        [SerializeField] private Slider gameSpeedSlider;
        [SerializeField] private Button playButton;
        [SerializeField] private Button backButton;

        [Header("Single PLayer Settings Values")]
        public bool delayStart = true;
        public float densityOfBoard = 1;
        public float gameSpeed = 1f;
        #endregion

        #region Single Player Settings Functionns
        public void OnDensityOfBoardScrollValueChanged()
        {
            densityOfBoard = densitySlider.value;
        }
        public void OnGameSpeedScrollValueChanged()
        {
            gameSpeed = gameSpeedSlider.value;
        }
        private void OnSinglePlayerMenuBackButtonClicked()
        {
            SceneManager.LoadScene(1, LoadSceneMode.Single);
        }
        private void OnSinglePlayerMenuPlayButtonClicked()
        {
            SceneManager.LoadScene(3, LoadSceneMode.Single);
        }
        #endregion
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
        }
        private void Start()
        {
            backButton.onClick.AddListener(OnSinglePlayerMenuBackButtonClicked);
            playButton.onClick.AddListener(OnSinglePlayerMenuPlayButtonClicked);
        }
        #endregion
    }
}
