using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonScript : MonoBehaviour
{
    public GameObject pauseUI;
    private void Start()
    {
        ZombieApocalypse.DatabaseStatus.isPaused = (Time.timeScale == 0);

    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            pauseUpdate();
        }
    }

    
    private void pauseUpdate()
    {
        Time.timeScale = (Time.timeScale == 0) ? 1 : 0;
        ZombieApocalypse.DatabaseStatus.isPaused = (Time.timeScale == 0);
        if (ZombieApocalypse.DatabaseStatus.isPaused)
            Cursor.visible = true;
        pauseUI.SetActive(Time.timeScale == 0);
    }
    public void onButtonClick(string name)
    {
        if (name == "RestartButton")
        {
            pauseUpdate();
            SceneManager.LoadScene("GameScene", LoadSceneMode.Single);
        }
        else if (name == "ResumeButton")
            pauseUpdate();
        else if (name == "MainMenuButton")
        {
            Time.timeScale = 1;
            SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
        }
            
    }
}
