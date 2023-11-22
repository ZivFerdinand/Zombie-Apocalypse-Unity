using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public void onPauseButtonClicked()
    {
        pauseUpdate();
    }
    private void pauseUpdate()
    {
        Time.timeScale = (Time.timeScale == 0) ? 1 : 0;
        ZombieApocalypse.DatabaseStatus.isPaused = (Time.timeScale == 0);
        pauseUI.SetActive(Time.timeScale == 0);
    }
}
