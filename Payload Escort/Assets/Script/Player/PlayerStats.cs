using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] PlayerController _playerController;
    [SerializeField] GamePlayUIManager _levelUIManager;
    [SerializeField] NewUpgradeSpawner _newUpgradeSpawner;
    //[SerializeField] UpgradeSpawner _upgradeSpawner;

    private HealthBar _healthBar;
    GameObject _healthBarObject;

    public int MaxHP = 1000;
    public int CurrentHP;
    public int LevelCoins = 0;
    public int PlayerLevel = 1;
    public int PlayerLevelXP = 0;
    public int PlayerLevelMaxXP;

    public bool IsBloodLust = false;
    public bool IsDoubleShot = false;

    public static bool IsInstantKill = false;


    private void Awake()
    {
        CurrentHP = MaxHP;
        PlayerLevelMaxXP = 25;
        _healthBarObject = Instantiate(Resources.Load("HealthBar"), FindObjectOfType<Canvas>().transform) as GameObject;
        _healthBar = _healthBarObject.GetComponent<HealthBar>();
        _healthBar.Initialize(transform);
        _healthBar.UpdateHealth(CurrentHP, MaxHP);
        _levelUIManager.UpdatePlayerLevel(PlayerLevel);
        _levelUIManager.UpdatePlayerXP(PlayerLevelXP, PlayerLevelMaxXP);
        _levelUIManager.UpdatePlayerCoins(LevelCoins);
    }

    public void TakeMeleeDamage(int damage)
    {
        CurrentHP -= damage;
        if (CurrentHP <= 0)
        {
            _playerController.RespawnPlayer();
        }
        _healthBar.UpdateHealth(CurrentHP, MaxHP);
    }
    public void TakeRangeDamage(int damage)
    {
        CurrentHP -= damage;
        if (CurrentHP <= 0)
        {
            _playerController.RespawnPlayer();
        }
        _healthBar.UpdateHealth(CurrentHP, MaxHP);
    }
    public void UpdatePlayerLevel()
    {
        if (PlayerLevelXP >= PlayerLevelMaxXP)
        {
            PlayerLevel++;
            PlayerLevelXP -= PlayerLevelMaxXP;
            PlayerLevelMaxXP += 25;
            _newUpgradeSpawner.OpenUpgradeUI();
        }
        _levelUIManager.UpdatePlayerLevel(PlayerLevel);
        _levelUIManager.UpdatePlayerXP(PlayerLevelXP, PlayerLevelMaxXP);
    }
    public void IncreaseDamage(int value)
    {
        _playerController.Weapon.BulletDamage += value;
    }
    public void IncreaseHealth(int value)
    {
        MaxHP += value;
        CurrentHP += value;
        _healthBar.UpdateHealth(CurrentHP, MaxHP);
    }
    public void BloodLustHealth(int value)
    {
        if (CurrentHP < MaxHP)
        {
            CurrentHP += value;
            if (CurrentHP >= MaxHP) 
            {
                CurrentHP = MaxHP;
            }
        }
        _healthBar.UpdateHealth(CurrentHP, MaxHP);
    }
    public void UpdateFireRate(float value)
    {
        _playerController.Weapon.FireRate -= value;
    }
    public void UpdatePlayerCoins()
    {
        _levelUIManager.UpdatePlayerCoins(LevelCoins);
    }

    public void AddSheild(int value, GameObject shields)
    {
        shields.transform.parent = transform;
        shields.transform.position = transform.position;
        //StartCoroutine(RemoveShield(value, shields));
    }

    public void BloodLust()
    {
        IsBloodLust = true;
    }
    public void DoubleShot()
    {
        IsDoubleShot = true;
        _playerController.Weapon.BulletDamage -= (_playerController.Weapon.BulletDamage * 25 / 100);
    }

    public void InstantKill(int value)
    {
        IsInstantKill = true;
        StartCoroutine(StopInstantKill(value));
    }

    public void UpdatePlayerMetaCoins()
    {
        Meta.Coins += LevelCoins;
    }

    public IEnumerator RemoveShield(int value, GameObject shields)
    {
        new WaitForSecondsRealtime(value);
        Destroy(shields);
        
        yield return null;
    }

    public IEnumerator StopInstantKill(int value)
    {
        new WaitForSecondsRealtime(value);
        IsInstantKill = false;
        yield return null;
        
    }
}
