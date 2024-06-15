using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PayloadController : MonoBehaviour, IDamageable
{
    public PayloadStats payloadStats;
    public PayloadWeapon Weapon;
    [SerializeField] public float movementSpeed;
    [SerializeField] public float rotationSpeed;
    [SerializeField] public Vector3 Direction;
    [SerializeField] public bool canMove;
    [SerializeField] public bool startRotating;
    [SerializeField] public bool isRotating;
    [SerializeField] public bool isRotatingRight;
    
    public static bool IsPayloadDestoried;
    

    void Update()
    {
        if (canMove)
        {
            MovePayload();
        }
     else if (startRotating) 
        {
         if (isRotatingRight)
            { RotatePayloadRight();}
        else { RotatePayloadLeft(); }
        }
        
    }


    public void MovePayload()
    {
        transform.Translate(Direction * movementSpeed * Time.deltaTime);
    }

    public void RotatePayloadRight()
    {
        if (!isRotating)
        {
            isRotating = true;
            Quaternion targetRotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + 90, transform.rotation.eulerAngles.z);
            // Start the coroutine to rotate over time
            StartCoroutine(RotateUntilTarget(targetRotation, rotationSpeed));
        }
    }
    public void RotatePayloadLeft()
    {
        if (!isRotating)
        {
            isRotating = true;
            Quaternion targetRotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y - 90, transform.rotation.eulerAngles.z);
            // Start the coroutine to rotate over time
            StartCoroutine(RotateUntilTarget(targetRotation, rotationSpeed));
        }
    }

    private IEnumerator RotateUntilTarget(Quaternion targetRotation, float duration)
    {
        Quaternion startRotation = transform.rotation;
        float time = 0;

        while (time < duration)
        {
            transform.rotation = Quaternion.Lerp(startRotation, targetRotation, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        startRotating = false;
        isRotating = false;
        isRotatingRight = false;
        transform.rotation = targetRotation;
        canMove = true;
    }

    
    public void TakeMeleeDamage(int damage)
    {
        payloadStats.TakeMeleeDamage(damage);
    }
    public void TakeRangedDamage(int damage)
    {
        payloadStats.TakeRangeDamage(damage);
    }
    public void DestroyPayload()
    {
        Destroy(this);
        SceneManager.LoadScene(0);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, Direction);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "path" && !startRotating)
        {
             canMove = true;
        }
        else if (other.tag == "right")
        {
            canMove = false;
            startRotating = true;
            isRotatingRight = true;
        }
        else if (other.tag == "left")
        {
            canMove = false;
            startRotating = true;
            isRotatingRight = false;
        }
    }

}
