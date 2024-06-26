using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class NewUpgradeSpawner : MonoBehaviour
{
    [SerializeField] Canvas UpgradeUI;

    public List<UpgradeItem> UpgradesSO; 
    [SerializeField] Button[] newUpgradeButtons;
    public List<Button> oldUpgradeButtons;

    [SerializeField] PlayerStats _playerStats;
    [SerializeField] PayloadStats _payloadStats;

    private int index;
    private int _index1 = -1;
    private int _index2 = -1;

    public void SetUpgradesInUI()
    {

        if (_payloadStats.IsBallistaActive)
        {
            index = Random.Range(1, oldUpgradeButtons.Count);
        }
        else if (_playerStats.IsBloodLust)
        {
            index = Random.Range(0, oldUpgradeButtons.Count - 1);
        }
        else if (_payloadStats.IsBallistaActive && _playerStats.IsBloodLust)
        {
            index = Random.Range(1, oldUpgradeButtons.Count - 1);
        }
        else
        {
            index = Random.Range(0, oldUpgradeButtons.Count);
        }
        _index1 = index;
        foreach (var button in newUpgradeButtons)
        {
            SetUpgrade(index);
            button.image.sprite = UpgradesSO[index].UpgradeSprite;
            button.onClick.RemoveAllListeners();
            button.onClick = oldUpgradeButtons[index].onClick;
            button.onClick.AddListener(CloseUpgradeUI);
            index = Random.Range(0, oldUpgradeButtons.Count);
            while (index == _index1 || index == _index2)
            {
                if (_payloadStats.IsBallistaActive)
                {
                    index = Random.Range(1, oldUpgradeButtons.Count);
                }
                else if (_playerStats.IsBloodLust)
                {
                    index = Random.Range(0, oldUpgradeButtons.Count - 1);
                }
                else if (_payloadStats.IsBallistaActive && _playerStats.IsBloodLust)
                {
                    index = Random.Range(1, oldUpgradeButtons.Count - 1);
                }
                else
                {
                    index = Random.Range(0, oldUpgradeButtons.Count);
                }
            }
            _index2 = index;
        }

    }

    public void SetUpgrade(int index)
    {
        UpgradesSO[index].SetPlayer(_playerStats);
        UpgradesSO[index].SetPayload(_payloadStats);
        UpgradesSO[index].SetUIManager(this);
    }

    public void OpenUpgradeUI()
    {
        SetUpgradesInUI();
        UpgradeUI.gameObject.SetActive(true);
        Time.timeScale = 0;
  
    }

    public void CloseUpgradeUI()
    {
        UpgradeUI.gameObject.SetActive(false);
        Time.timeScale = 1;
    }

    public void SetUpgradeOnButton()
    {


    }


}
