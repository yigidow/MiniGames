using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SinglePlayerLevelScreenManager : MonoBehaviour
{
    [SerializeField] private Button backButton;
    private void OnSinglePlayerLevelBackButtonClicked()
    {
        SceneManager.LoadScene(2, LoadSceneMode.Single);
    }
    void Start()
    {
        backButton.onClick.AddListener(OnSinglePlayerLevelBackButtonClicked);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
