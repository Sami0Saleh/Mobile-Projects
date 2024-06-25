using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class GamePlayUIManager : MonoBehaviour
{
    [SerializeField] GameObject _upgrades;
    [SerializeField] TextMeshProUGUI _playerHPText;
    [SerializeField] TextMeshProUGUI _payloadHPText;
    [SerializeField] TextMeshProUGUI _playerLevelText;
    [SerializeField] TextMeshProUGUI _playerCoinsText;
    [SerializeField] Slider _playerHPSlider;
    [SerializeField] Slider _payloadHPSlider;
    [SerializeField] Slider _playerLevelSlider;
    
    public void UpdatePlayerHP(int playerCurrentHP, int playerMaxHP)
    {
        _playerHPText.text = playerCurrentHP.ToString();
        _playerHPSlider.maxValue = playerMaxHP;
        _playerHPSlider.value = playerCurrentHP;
    }
    public void UpdatePayloadHP(int payloadCurrentHP, int payloadMaxHP)
    {
        _payloadHPText.text = payloadCurrentHP.ToString();
        _payloadHPSlider.maxValue = payloadMaxHP;
        _payloadHPSlider.value = payloadCurrentHP;
    }
    public void UpdatePlayerLevel(int playerLevel)
    {
        _playerLevelText.text = playerLevel.ToString();
    }
    public void UpdatePlayerCoins(int playerCoins)
    {
        _playerCoinsText.text = playerCoins.ToString();
    }
    public void UpdatePlayerXP(int playerXP, int playerMaxXP)
    {
        _playerLevelSlider.maxValue = playerMaxXP;
        _playerLevelSlider.value = playerXP;
    }
    public void OpenUpgrades()
    {
        _upgrades.SetActive(true);
        Time.timeScale = 0.0f;
    }
    public void CloseUpgrades()
    {
        _upgrades.SetActive(false);
        Time.timeScale = 1.0f;
    }
}