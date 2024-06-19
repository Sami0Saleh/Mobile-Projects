using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuUIManager : MonoBehaviour
{
    [SerializeField] GameObject _play;
    [SerializeField] GameObject _skillTree;
    [SerializeField] GameObject _equipment;
    [SerializeField] TextMeshProUGUI _energyText;

    


    public void StartButton()
    {
        SceneManager.LoadScene(1);
        MainManager.Energy -= 5;
    }

    public void PlayButton()
    {
        _play.SetActive(true);
        _skillTree.SetActive(false);
        _equipment.SetActive(false);
    }
    public void SkillTreeButton()
    {
        _play.SetActive(false);
        _skillTree.SetActive(true);
        _equipment.SetActive(false);
    }
    public void EquepmentButton()
    {
        _play.SetActive(false);
        _skillTree.SetActive(false);
        _equipment.SetActive(true);
    }

    public void UpdateEnergyText()
    {
        _energyText.text = $"{MainManager.Energy} / 20";
    }
}
