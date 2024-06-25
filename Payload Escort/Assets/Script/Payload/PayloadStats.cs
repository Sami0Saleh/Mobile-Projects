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
    public static bool IsBallistaActive = false; 

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

    public void AddSheild(int value)
    {

        StartCoroutine(RemoveShield(value));
    }
    public void StartFlee(int value)
    {
        _payloadController.MovementSpeed += 1;
        StartCoroutine(StopFlee(value));
    }



    public IEnumerator RemoveShield(int value)
    {
        yield return new WaitForSecondsRealtime(value);

    }

    public IEnumerator StopFlee(int value)
    {
        yield return new WaitForSecondsRealtime(value);
        _payloadController.MovementSpeed -= 1;
    }
}
