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
    public UpgradeTarget upgradeTarget;
    public UpgradeType upgradeType;
    public GameObject prefab;
    public float value;
    private PlayerStats _playerStats;
    private PayloadStats _payloadStats;
    private NewUpgradeSpawner _newUpgradeSpawner;

    public void ApplyUpgrade()
    {
        switch(upgradeTarget)
        {
            case UpgradeTarget.Player:
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
                    case UpgradeType.AttackSpeed:
                        Debug.Log("Upgrading AttackSpeed");
                        _playerStats.UpdateFireRate(value);
                        _newUpgradeSpawner.CloseUpgradeUI();
                        break;
                    case UpgradeType.BloodLust:
                        Debug.Log("BloodLust");
                        _playerStats.BloodLust();
                        _newUpgradeSpawner.CloseUpgradeUI();
                        break;
                    case UpgradeType.InstantKill:
                        Debug.Log("InstantKill");
                        _playerStats.InstantKill((int)value);
                        _newUpgradeSpawner.CloseUpgradeUI();
                        break;
                    case UpgradeType.Shield:
                        Debug.Log("Shield");
                        GameObject shields = Instantiate(prefab);
                        _playerStats.AddSheild((int)value, shields);
                        _newUpgradeSpawner.CloseUpgradeUI();
                        break;
                    case UpgradeType.DoubleShot:
                        Debug.Log("DoubleShot");
                        _playerStats.DoubleShot();
                        _newUpgradeSpawner.CloseUpgradeUI();
                        break;
                    default:
                        Debug.LogWarning("Unknown upgrade type: " + upgradeType);
                        break;
                }
                break;
            case UpgradeTarget.Payload:
                switch (upgradeType)
                {
                    case UpgradeType.Ballista:
                        Debug.Log("Activating Payload Weapon");
                        _payloadStats.ActiveBallista();
                        _newUpgradeSpawner.CloseUpgradeUI();
                        break;
                    case UpgradeType.Shield:
                        Debug.Log("Shield");
                        GameObject shields = Instantiate(prefab);
                        _payloadStats.AddSheild((int)value, shields);
                        _newUpgradeSpawner.CloseUpgradeUI();
                        break;
                    case UpgradeType.Flee:
                        Debug.Log("Flee");
                        _payloadStats.StartFlee(value);
                        _newUpgradeSpawner.CloseUpgradeUI();
                        break;
                    default:
                        Debug.LogWarning("Unknown upgrade type: " + upgradeType);
                        break;
                }
                break;
            case UpgradeTarget.Both:
                switch (upgradeType)
                {
                    case UpgradeType.Damage:
                        Debug.Log("Upgrading Both Damage");
                        _playerStats.IncreaseDamage((int)value);
                        _payloadStats.IncreaseDamage((int)value);
                        _newUpgradeSpawner.CloseUpgradeUI();
                        break;
                    case UpgradeType.Health:
                        Debug.Log("Upgrading Both Health");
                        _playerStats.IncreaseHealth((int)value);
                        _payloadStats.IncreaseHealth((int)value);
                        _newUpgradeSpawner.CloseUpgradeUI();
                        break;
                    case UpgradeType.AttackSpeed:
                        Debug.Log("Upgrading Both AttackSpeed");
                        _playerStats.IncreaseDamage((int)value);
                        _payloadStats.IncreaseDamage((int)value);
                        _newUpgradeSpawner.CloseUpgradeUI();
                        break;
                    default:
                        Debug.LogWarning("Unknown upgrade type: " + upgradeType);
                        break;
                }
                break;
            default:
                Debug.LogWarning("Unknown upgrade target: " + upgradeTarget);
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
    AttackSpeed,
    BloodLust,
    InstantKill,
    Shield,
    Ballista,
    Flee,
    DoubleShot
}
public enum UpgradeTarget
{
    Player,
    Payload,
    Both
}