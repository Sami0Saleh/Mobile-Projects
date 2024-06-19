using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BuildUIManager : MonoBehaviour
{
    [SerializeField] private Canvas StartGameCanvas;
    [SerializeField] private Canvas EndGameCanvas;

    [SerializeField] private PayloadController payload;

    void Start()
    {
        Time.timeScale = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (payload.LastWaypoint)
        {
            EndGameCanvas.gameObject.SetActive(true);
            Time.timeScale = 0;
            
        }
    }

    public void StartGame()
    {
        StartGameCanvas.enabled = false;
        Time.timeScale = 1;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(0);
    }

    public void ExitGame()
    {
        Application.Quit();
    }




}

