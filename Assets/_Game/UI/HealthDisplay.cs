using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthDisplay : MonoBehaviour
{
    [SerializeField] private Image heartPrefab;
    [SerializeField] private Sprite fullHeart;
    [SerializeField] private Sprite emptyHeart;

    private readonly List<Image> _hearts = new();
    private PlayerHealth _playerHealth;

    public void ReferencePlayer(PlayerHealth playerHealth)
    {
        if (_playerHealth != null)
        {
            _playerHealth.OnHealthChanged -= UpdateHearts;
        }

        _playerHealth = playerHealth;
        _playerHealth.OnHealthChanged += UpdateHearts;

        foreach (var heart in _hearts)
        {
            Destroy(heart.gameObject);
        }
        _hearts.Clear();

        for (var i = 0; i < playerHealth.MaxHealth; i++)
        {
            _hearts.Add(Instantiate(heartPrefab, transform));
        }
        
        UpdateHearts(_playerHealth.CurrentHealth);
    }

    private void UpdateHearts(int hp)
    {
        for (var i = 0; i < _hearts.Count; i++)
        {
            _hearts[i].sprite = i < hp ? fullHeart : emptyHeart;
        }
    }

    private void OnDestroy()
    {
        if (_playerHealth != null)
        {
            _playerHealth.OnHealthChanged -= UpdateHearts;
        }
    }


}
