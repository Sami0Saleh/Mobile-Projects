using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldUpgradeRotation : MonoBehaviour
{
    private float _rotationSpeed = 10;
    private Vector3 _rotationDirection = new Vector3(0,1,0);

    void Update()
    {
        transform.Rotate(_rotationDirection * _rotationSpeed * Time.deltaTime);
    }
}
