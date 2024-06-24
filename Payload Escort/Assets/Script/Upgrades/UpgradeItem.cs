using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "NewUpgradeItem", menuName = "Upgrade Item")]
public class UpgradeItem : ScriptableObject
{

    [SerializeField] public Sprite UpgradeSprite;
    public string itemName;
    public string description;
    public UpgradeType upgradeType;
    public GameObject prefab;
    public float value;
    private PlayerStats _playerStats;
    private PayloadStats _payloadStats;
    private NewUpgradeSpawner _newUpgradeSpawner;

    public void ApplyUpgrade()
    {
        
        switch (upgradeType)
        {
            case UpgradeType.Damage:
                Debug.Log("Upgrading Damage");
                _playerStats.IncreaseDamage((int)value);
                _newUpgradeSpawner.CloseUpgradeUI();
                break;
            case UpgradeType.Health:
                Debug.Log("Upgrading Health");
                _playerStats.IncreaseHealth((int)value);
                _newUpgradeSpawner.CloseUpgradeUI();
                break;
            case UpgradeType.FireRate:
                Debug.Log("Upgrading FireRate");
                _playerStats.UpdateFireRate(value);
                _newUpgradeSpawner.CloseUpgradeUI();
                break;
            case UpgradeType.PayloadFire:
                Debug.Log("Upgrading Payload Fire");
                _payloadStats.IncreaseDamage((int)value);
                _newUpgradeSpawner.CloseUpgradeUI();
                break;
            default:
                Debug.LogWarning("Unknown upgrade type: " + upgradeType);
                break;
        }
    }
    public void SetPlayer(PlayerStats playerStats)
    {
        _playerStats = playerStats;
    }
    public void SetPayload(PayloadStats payloadStats)
    {
        _payloadStats = payloadStats;
    }
    public void SetUIManager(NewUpgradeSpawner newUpgradeSpawner)
    {
        _newUpgradeSpawner = newUpgradeSpawner;
    }
}
public enum UpgradeType
{
    Damage,
    Health,
    FireRate,
    PayloadFire
}