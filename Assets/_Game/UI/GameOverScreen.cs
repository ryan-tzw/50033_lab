using UnityEngine;
using TMPro;

public class GameOverScreen : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private IntVariable runScore;
    [SerializeField] private TextMeshProUGUI titleText;

    public void Show()
    {
        gameObject.SetActive(true);
        titleText.text = "Game Over";
        scoreText.text = $"{runScore.Value}";
    }

    public void ShowVictory()
    {
        gameObject.SetActive(true);
        titleText.text = "Victory!";
        scoreText.text = $"{runScore.Value}";
    }
}
