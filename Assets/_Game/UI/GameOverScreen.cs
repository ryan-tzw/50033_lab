using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private IntVariable runScore;
    
    public void Show()
    {
        gameObject.SetActive(true);
        scoreText.text = $"{runScore.Value}";
    }

    public void RestartButton()
    {
        Time.timeScale = 1f;
        runScore.ResetValue();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
