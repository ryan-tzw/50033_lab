using UnityEngine;
using UnityEngine.UI;

public class HealthDisplay : MonoBehaviour
{
    public int maxHealth;
    [SerializeField] private Image[] hearts;
    private Player _player;

    public void ReferencePlayer(Player player)
    {
        if (_player != null)
        {
            _player.OnHealthChanged -= UpdateHearts;
        }

        _player = player;
        _player.OnHealthChanged += UpdateHearts;
    }

    private void UpdateHearts(int hp)
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < hp)
            {
                hearts[i].enabled = true;
            }
            else
            {
                hearts[i].enabled = false;
            }
        }
    }

    private void OnDestroy()
    {
        if (_player != null)
        {
            _player.OnHealthChanged -= UpdateHearts;
        }
    }


}
