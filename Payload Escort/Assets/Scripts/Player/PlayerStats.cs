using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] PlayerController _playerController;
    [SerializeField] LevelUIManager _levelUIManager;
    [SerializeField] NewUpgradeSpawner _newUpgradeSpawner;


    public int MaxHP = 1000;
    public int CurrentHP;
    public int Damage = 2;
    public int LevelCoins = 0;
    public int PlayerLevel = 1;
    public int PlayerLevelXP = 0;
    public int PlayerLevelMaxXP;


    private void Awake()
    {
        CurrentHP = MaxHP;
        PlayerLevelMaxXP = 25;
        _levelUIManager.UpdatePlayerHP(CurrentHP, MaxHP);
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
        _levelUIManager.UpdatePlayerHP(CurrentHP, MaxHP);
    }
    public void TakeRangeDamage(int damage)
    {
        Debug.Log("hit");
        CurrentHP -= damage;
        if (CurrentHP <= 0)
        {
            _playerController.RespawnPlayer();
        }
        _levelUIManager.UpdatePlayerHP(CurrentHP, MaxHP);
    }
    public void UpdatePlayerLevel()
    {
        if (PlayerLevelXP >= PlayerLevelMaxXP)
        {
            PlayerLevel++;
            PlayerLevelXP -= PlayerLevelMaxXP;
            PlayerLevelMaxXP += 25;
            //_newUpgradeSpawner.OpenUpgrades();
            // _upgradeSpawner.SpawnRandomUpgrades();
            _newUpgradeSpawner.OpenUpgradeUI();
        }
        _levelUIManager.UpdatePlayerLevel(PlayerLevel);
        _levelUIManager.UpdatePlayerXP(PlayerLevelXP, PlayerLevelMaxXP);
    }
    public void IncreaseDamage(int value)
    {
        Damage += value;
    }
    public void IncreaseHealth(int value)
    {
        MaxHP += value;
        CurrentHP += value;
        _levelUIManager.UpdatePlayerHP(CurrentHP, MaxHP);
    }
    public void UpdateFireRate(float value)
    {
        _playerController.Weapon.FireRate -= value;
    }
    public void UpdatePlayerCoins()
    {
        _levelUIManager.UpdatePlayerCoins(LevelCoins);
    }
    public void UpdatePlayerMetaCoins()
    {
        Meta.Coins += LevelCoins;
    }
}
