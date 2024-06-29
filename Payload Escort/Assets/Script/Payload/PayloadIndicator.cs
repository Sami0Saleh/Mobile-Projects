using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PayloadIndicator : MonoBehaviour
{
    [SerializeField] Transform _payloadTransform;
    [SerializeField] Camera _mainCamera;
    [SerializeField] RectTransform _arrowTransform;
    [SerializeField] RectTransform _healthCircleTransform;
    [SerializeField] Image _healthCircleImage;

    [SerializeField] PayloadStats _payloadStats;

    private void Update()
    {
        Vector3 screenPosition = _mainCamera.WorldToScreenPoint(_payloadTransform.position);
        bool isOffScreen = screenPosition.x <= 0 || screenPosition.x >= Screen.width || screenPosition.y <= 0 || screenPosition.y >= Screen.height;

        if (isOffScreen)
        {
            _arrowTransform.gameObject.SetActive(true);
            _healthCircleTransform.gameObject.SetActive(true);
            UpdateIndicator(screenPosition);
        }
        else
        {
            _arrowTransform.gameObject.SetActive(false);
            _healthCircleTransform.gameObject.SetActive(false);
        }
    }

    private void UpdateIndicator(Vector3 screenPosition)
    {
        if (screenPosition.z < 0)
        {
            screenPosition *= -1;
        }

        Vector3 cappedScreenPosition = screenPosition;
        cappedScreenPosition.x = Mathf.Clamp(screenPosition.x, 110, Screen.width - 110);  
        cappedScreenPosition.y = Mathf.Clamp(screenPosition.y, 110, Screen.height - 110);

        _arrowTransform.position = cappedScreenPosition;
        _healthCircleTransform.position = cappedScreenPosition;

        Vector3 direction = _payloadTransform.position - _mainCamera.transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        _arrowTransform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        UpdateHealthCircle();
    }
    private void UpdateHealthCircle()
    {
        float healthPercentage =(float) _payloadStats.CurrentHP / _payloadStats.MaxHP;
        _healthCircleImage.fillAmount = healthPercentage;
    }
}
