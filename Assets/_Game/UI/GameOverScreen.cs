using UnityEngine;
using TMPro;

public class GameOverScreen : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private IntVariable runScore;
    
    public void Show()
    {
        gameObject.SetActive(true);
        scoreText.text = $"{runScore.Value}";
    }
}
