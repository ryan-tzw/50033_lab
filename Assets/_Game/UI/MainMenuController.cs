using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    private const string GameSceneName = "ExperimentScene";

    [SerializeField] private GameObject mainPanel;
    [SerializeField] private GameObject optionsPanel;

    private void Awake()
    {
        Time.timeScale = 1;
        ShowMainMenu();
    }

    public void StartGame()
    {
        SceneManager.LoadScene(GameSceneName);
    }

    public void ShowOptions()
    {
        mainPanel.SetActive(false);
        optionsPanel.SetActive(true);
    }

    public void ShowMainMenu()
    {
        mainPanel.SetActive(true);
        optionsPanel.SetActive(false);
    }
}