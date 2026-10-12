using TMPro;
using UnityEngine;

public class ScoreDisplay : MonoBehaviour
{
    [SerializeField] private IntVariable runScore;
    private TextMeshProUGUI _scoreText;

    private void Awake()
    {
        _scoreText = GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        runScore.Changed += UpdateDisplay;
        UpdateDisplay(runScore.Value);
    }

    private void OnDisable()
    {
        runScore.Changed -= UpdateDisplay;
    }

    private void UpdateDisplay(int score)
    {
        _scoreText.text = score.ToString();
    }
}

