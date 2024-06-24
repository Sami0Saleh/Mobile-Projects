using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PayloadStats : MonoBehaviour
{
    [SerializeField] PayloadController _payloadController;
    [SerializeField] GamePlayUIManager _levelUIManager;
    [SerializeField] NewUpgradeSpawner _newUpgradeSpawner;

    public int MaxHP = 500;
    public int CurrentHP;

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
}
