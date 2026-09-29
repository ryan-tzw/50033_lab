using Legacy;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

namespace Legacy
{
    public class GameOverScreen : MonoBehaviour
    {
        public TextMeshProUGUI scoreText;
        [SerializeField] private GameController gameController;
        public void Setup(int score)
        {
            gameObject.SetActive(true);
            scoreText.text = score.ToString() + " points";
        }

        public void RestartButton()
        {
            gameController.ResetValues();
            SceneManager.LoadScene("SampleScene");
        }
    }
}
