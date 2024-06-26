using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PayloadIndicator : MonoBehaviour
{
    [SerializeField] private Camera uiCamera;
    [SerializeField] private float screenBorderSize = 150f;
    [SerializeField] private float distanceFromPlanetToHide = 10f;
    [SerializeField] private RectTransform pointerRectTransform;
    [SerializeField] private Vector2 distanceSizeConstraints;
    [SerializeField] private float distanceSizeDivider = 80;

    private Vector3 _targetPosition;
    private Camera _mainCamera;
    private bool _hasTarget;

    private void Awake()
    {
        _mainCamera = Camera.main;
    }

    private void Update()
    {
        if (!_hasTarget)
        {
            return;
        }

        var toPosition = _targetPosition;
        var fromPosition = _mainCamera.transform.position;
        fromPosition.z = 0;

        var dir = (toPosition - fromPosition).normalized;
        var angle = GetAngleFromVectorFloat(dir);

        pointerRectTransform.localEulerAngles = new Vector3(0, 0, angle);
        var extendedTargetPosition = fromPosition +
                                     (dir * (Vector3.Distance(fromPosition, _targetPosition) -
                                             distanceFromPlanetToHide));

        var targetPositionScreenPoint = _mainCamera.WorldToScreenPoint(extendedTargetPosition);
        var isOffScreen = targetPositionScreenPoint.x <= screenBorderSize ||
                          targetPositionScreenPoint.x >= Screen.width ||
                          targetPositionScreenPoint.y <= screenBorderSize ||
                          targetPositionScreenPoint.y >= Screen.height;

        if (isOffScreen)
        {
            ShowPointer();
            var cappedTargetScreenPosition = targetPositionScreenPoint;
            if (cappedTargetScreenPosition.x <= screenBorderSize)
                cappedTargetScreenPosition.x = screenBorderSize;

            if (cappedTargetScreenPosition.x >= Screen.width - screenBorderSize)
                cappedTargetScreenPosition.x = Screen.width - screenBorderSize;

            if (cappedTargetScreenPosition.y <= screenBorderSize)
                cappedTargetScreenPosition.y = screenBorderSize;

            if (cappedTargetScreenPosition.y >= Screen.height - screenBorderSize)
                cappedTargetScreenPosition.y = Screen.height - screenBorderSize;

            var pointerWorldPosition = uiCamera.ScreenToWorldPoint(cappedTargetScreenPosition);

            pointerRectTransform.position = pointerWorldPosition;

            var localPosition = pointerRectTransform.localPosition;
            localPosition = new Vector3(localPosition.x,
                localPosition.y, 0f);
            pointerRectTransform.localPosition = localPosition;

            var distance = Vector3.Distance(toPosition, fromPosition);
            var newSize = Mathf.Lerp(distanceSizeConstraints.y, distanceSizeConstraints.x,
                distance / distanceSizeDivider);
            newSize = Mathf.Min(Mathf.Max(newSize, distanceSizeConstraints.x), distanceSizeConstraints.y);

            pointerRectTransform.localScale = new Vector3(newSize, newSize, 1);
        }
        else
        {
            HidePointer();
        }
    }

    public void SetTarget(Vector3 targetPosition)
    {
        _targetPosition = targetPosition;
        _hasTarget = true;
    }

    public void ClearTarget()
    {
        _targetPosition = Vector3.zero;
        _hasTarget = false;
        HidePointer();
    }

    private void HidePointer()
    {
        pointerRectTransform.gameObject.SetActive(false);
    }

    private void ShowPointer()
    {
        pointerRectTransform.gameObject.SetActive(true);
    }

    private float GetAngleFromVectorFloat(Vector3 dir)
    {
        dir = dir.normalized;
        var n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (n < 0) n += 360;

        return n;
    }
}
