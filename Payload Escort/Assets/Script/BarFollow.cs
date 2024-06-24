using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarFollow : MonoBehaviour
{
    [SerializeField] Transform _targetPos;
    private Quaternion originalrotation;

    void Start()
    {
        transform.position =  _targetPos.position + new Vector3(0, 0.5f, 0);
        originalrotation = transform.rotation;
    }
    

    void LateUpdate()
    {
        transform.position = _targetPos.position + new Vector3(0, 0.5f, 0);
        transform.rotation = originalrotation;
    }
}

