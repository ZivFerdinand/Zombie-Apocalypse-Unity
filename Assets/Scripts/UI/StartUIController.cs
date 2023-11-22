using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartUIController : MonoBehaviour
{
    
    public void onClickChangeScene(string name)
    {

        if (name == "PlayButton")
            SceneManager.LoadScene("GameScene", LoadSceneMode.Single);
    }
}
