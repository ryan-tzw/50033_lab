using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ObjectiveUI : MonoBehaviour
{
    [SerializeField] private IntVariable killCount;
    [SerializeField] private int killTarget = 20;            
    [SerializeField] private TextMeshProUGUI objectiveText;
    [SerializeField] private Image icon;
    [SerializeField] private Sprite enemySprite;
    [SerializeField] private string killLabel = "Enemies defeated";
    [SerializeField] private string bossMessage = "Defeat the boss!";

    private int _lastShown = -1;

    private void Update()
    {
        int kills = killCount.Value;
        if (kills == _lastShown) return;   // only update when the count changes
        _lastShown = kills;

        if (kills >= killTarget)
        {
            objectiveText.text = bossMessage;
            icon.enabled = false;
        }
        else
        {
            objectiveText.text = $"{killLabel}: {kills}/{killTarget}";
            SetIcon(enemySprite);
        }
    }

    private void SetIcon(Sprite sprite)
    {
        if (icon == null) return;

        if (sprite == null)
        {
            icon.enabled = false;
        }
        else
        {
            icon.enabled = true;
            icon.sprite = sprite;
            icon.preserveAspect = true;
        }
    }


}
