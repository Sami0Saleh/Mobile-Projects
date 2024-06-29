using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBar : MonoBehaviour
{
    [SerializeField] Slider _healthSlider;
    [SerializeField] TextMeshProUGUI _healthText;
    private Transform _target;
    private Camera _mainCamera;

    public void Initialize(Transform target)
    {
        _target = target;
        _mainCamera = Camera.main;
    }

    public void UpdateHealth(int currentHealth, int maxHealth)
    {
        _healthText.text = currentHealth.ToString();
        _healthSlider.maxValue = maxHealth;
        _healthSlider.value = currentHealth;
    }

    private void LateUpdate()
    {
        if (_target != null)
        {
            Vector3 screenPosition = _mainCamera.WorldToScreenPoint(new Vector3(_target.position.x, _target.position.y + 0.2f, _target.position.z));
            transform.position = screenPosition;
        }
    }
}
