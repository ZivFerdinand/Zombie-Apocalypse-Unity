using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonScript : MonoBehaviour
{
    public GameObject pauseUI;
    public GameObject overlay;
    private List<Vector2> mainScChildInitPos;
    private void Start()
    {
        mainScChildInitPos = new List<Vector2>();
        for (int i = 0; i < pauseUI.transform.childCount; i++)
        {
            mainScChildInitPos.Add(pauseUI.transform.GetChild(i).transform.localPosition);
        }
        ZombieApocalypse.DatabaseStatus.isPaused = (Time.timeScale == 0);
        for (int i = 0; i < pauseUI.transform.childCount; i++)
        {
            pauseUI.transform.GetChild(i).transform.localPosition = new Vector2(0, -1000);
        }
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {

            pauseCheck();
        }
    }

    private void pauseCheck()
    {
        if (!ZombieApocalypse.DatabaseStatus.isPaused)
        {
            pauseUI.SetActive(true);
            StartCoroutine(Fade(0, 0.5f));
            for (int i = 0; i < pauseUI.transform.childCount; i++)
            {
                if (i == pauseUI.transform.childCount - 1)
                    LeanTween.moveLocal(pauseUI.transform.GetChild(i).gameObject, mainScChildInitPos[i], 1).setEaseOutBack().setOnComplete(() => { pauseUpdate(); });
                else
                    LeanTween.moveLocal(pauseUI.transform.GetChild(i).gameObject, mainScChildInitPos[i], 1).setEaseOutBack();
            }
        }
        else
        {
            Debug.Log("y");
            //pauseUI.SetActive(false);
            pauseUpdate();
            StartCoroutine(Fade(0.5f, 0f));
            for (int i = 0; i < pauseUI.transform.childCount; i++)
            {
                    LeanTween.moveLocal(pauseUI.transform.GetChild(i).gameObject, new Vector2(0, -1000), 1).setEaseOutBack();
            }
        }
    }
    private void pauseUpdate()
    {
        

        Time.timeScale = (Time.timeScale == 0) ? 1 : 0;
        ZombieApocalypse.DatabaseStatus.isPaused = (Time.timeScale == 0);
        if (ZombieApocalypse.DatabaseStatus.isPaused)
            Cursor.visible = true;
    }
    public void onButtonClick(string name)
    {
        if (name == "RestartButton")
        {
            pauseCheck();
            SceneManager.LoadScene("GameScene", LoadSceneMode.Single);
        }
        else if (name == "ResumeButton")
            pauseCheck();
        else if (name == "MainMenuButton")
        {
            Time.timeScale = 1;
            SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
        }
            
    }
    private float m_timerCurrent;
    private float m_fadeDuration = 0.5f;
    public AnimationCurve m_smoothCurve = new AnimationCurve(new Keyframe[] { new Keyframe(0f, 0f), new Keyframe(1f, 1f) });
    private readonly WaitForSeconds m_skipFrame = new WaitForSeconds(0.01f);

    private IEnumerator Fade(float start, float end)
    {
        m_timerCurrent = 0f;

        while (m_timerCurrent <= m_fadeDuration)
        {
            m_timerCurrent += Time.deltaTime;
            Color c = overlay.GetComponent<Image>().color;
            overlay.GetComponent<Image>().color = new Color(c.r, c.g, c.b, Mathf.Lerp(start, end, m_smoothCurve.Evaluate(m_timerCurrent / m_fadeDuration)));
            yield return m_skipFrame;
        }
    }
}
