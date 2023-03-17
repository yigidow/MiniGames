using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace YY_Games_Scripts
{
    public class SinglePlaySettingsMenu : MonoBehaviour
    {
        #region Single Player Settings Variables
        //Singleton
        public static SinglePlaySettingsMenu instance;

        [Header("Single PLayer Settings Components")]
        [SerializeField] private Slider gameDifficultySlider;
        [SerializeField] private Slider densitySlider;
        [SerializeField] private Slider gameSpeedSlider;

        [SerializeField] private Button playButton;
        [SerializeField] private Button backButton;

        [Header("Single PLayer Settings Values")]
        public bool delayStart = true;

        public int gameDifficulty;
        public int densityOfBoard;
        public float gameSpeed;
        #endregion
        #region Single Player Settings Functionns
        public void OnGameDifficultyScrollValueChanged()
        {
            gameDifficulty = (int) gameDifficultySlider.value;
        }
        public void OnDensityOfBoardScrollValueChanged()
        {
            densityOfBoard = (int) densitySlider.value;
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
        private void SetGameVariables()
        {
            PlayerPrefs.SetInt("gameDifficulty", gameDifficulty);
            PlayerPrefs.SetInt("densityOfBoard", densityOfBoard);
            PlayerPrefs.SetFloat("gameSpeed", gameSpeed);
        }
        private void GetGameVariables()
        {
            PlayerPrefs.GetInt("gameDifficulty");
            PlayerPrefs.GetInt("densityOfBoard");
            PlayerPrefs.GetFloat("gameSpeed");
            gameDifficultySlider.value = gameDifficulty;
            densitySlider.value = densityOfBoard;
            gameSpeedSlider.value = gameSpeed;

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
            GetGameVariables();
            backButton.onClick.AddListener(OnSinglePlayerMenuBackButtonClicked);
            playButton.onClick.AddListener(OnSinglePlayerMenuPlayButtonClicked);
            playButton.onClick.AddListener(SetGameVariables);
        }
        #endregion
    }
}
