using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PayloadStats : MonoBehaviour
{
    [SerializeField] PayloadController _payloadController;
    [SerializeField] GamePlayUIManager _levelUIManager;
    [SerializeField] NewUpgradeSpawner _newUpgradeSpawner;
    [SerializeField] GameObject Ballista;

    public int MaxHP = 1000;
    public int CurrentHP;
    public bool IsBallistaActive = false; 

    private void Awake()
    {
        CurrentHP = MaxHP;
        _levelUIManager.UpdatePayloadHP(CurrentHP, MaxHP);
    }
    
    public void TakeMeleeDamage(int damage)
    {
        CurrentHP -= damage;
        if (CurrentHP <= 0)
        {
            CurrentHP = 0;
            _payloadController.DestroyPayload();
        }
        _levelUIManager.UpdatePayloadHP(CurrentHP, MaxHP);
    }
    public void TakeRangeDamage(int damage)
    {
        CurrentHP -= damage;
        if (CurrentHP <= 0)
        {
            CurrentHP = 0;
            _payloadController.DestroyPayload();
        }
        _levelUIManager.UpdatePayloadHP(CurrentHP, MaxHP);
    }

    public void ActiveBallista()
    {
        Ballista.SetActive(true);
        
        IsBallistaActive = true;
    }
    public void IncreaseDamage(int value)
    {
        _payloadController.Weapon.BulletDamage += value;
    }
    public void IncreaseHealth(int value)
    {
        MaxHP += value;
        CurrentHP += value;
        _levelUIManager.UpdatePayloadHP(CurrentHP, MaxHP);
    }
    public void UpdateFireRate(float value)
    {
        _payloadController.Weapon.FireRate -= value;
    }

    public void AddSheild(int value, GameObject shields)
    {
        shields.transform.parent = transform;
        shields.transform.position = transform.position;
        //StartCoroutine(RemoveShield(value, shields));
    }
    public void StartFlee(int value)
    {
        _payloadController.MovementSpeed += value;
        StartCoroutine(StopFlee(value));
    }



    public IEnumerator RemoveShield(int value)
    {
        new WaitForSecondsRealtime(value);

        yield return null;

    }

    public IEnumerator StopFlee(int value)
    {
        new WaitForSecondsRealtime(5);
        _payloadController.MovementSpeed -= value;
        yield return null;
    }
}
