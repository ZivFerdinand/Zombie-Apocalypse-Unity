using UnityEngine;
using TMPro;
using UnityEngine.Events;
using Dan.Main;
public class ScoreManagerScript : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI inputScore;
    [SerializeField]
    private TMP_InputField inputName;
    public UnityEvent<string, int> submitscoreEvent;
    public void SubmitScore()
    {
        submitscoreEvent.Invoke(inputName.text, int.Parse(inputScore.text));
    }
}
