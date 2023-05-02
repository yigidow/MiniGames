using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

namespace YY_Games_Scripts
{
    public class GameManager : MonoBehaviour
    {
        #region Variables and References
        public static GameManager instance;
        [Header("UI Elemenets")]
        [SerializeField] private TMP_Text gameTimer;
        [SerializeField] private Button backButton;

        [Header("Variables for stage clearence")]
        [SerializeField] private GameObject winScreen;
        [SerializeField] private TMP_Text gameWonTimeShown;
        public int winTimeRecordInSeconds;

        [Header("Variables for stage lost")]
        [SerializeField] private GameObject loseScreen;


        #endregion

        #region Functions for Game Manager Buttons, Win Lose Screens, Timer
        private void OnSinglePlayerLevelBackButtonClicked()
        {
            SceneManager.LoadScene(2, LoadSceneMode.Single);
        }
        private void SetGameTimer()
        {
            int minutes = Mathf.FloorToInt((Time.time) / 60);
            int seconds = Mathf.FloorToInt((Time.time) % 60f);
            gameTimer.text = minutes.ToString("00") + " : " + seconds.ToString("00");
            winTimeRecordInSeconds = Mathf.FloorToInt(Time.time);
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
        }
        void Start()
        {
            backButton.onClick.AddListener(OnSinglePlayerLevelBackButtonClicked);
        }

        void Update()
        {
            if(Board.instance.isStageCleared == false && Board.instance.isGameLost == false)
            {
                SetGameTimer();
            }
            else if (Board.instance.isStageCleared == true)
            {
                winScreen.SetActive(true);
                gameWonTimeShown.text = "Clear time: " + gameTimer.text;
            }
            else if (Board.instance.isGameLost == true)
            {
                loseScreen.SetActive(true);
            }
        }
        #endregion
    }
}
