using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Dan.Main;

public class LeaderBoardScript : MonoBehaviour
{
    public ButtonScript buttonScript;
    [SerializeField]
    private List<TextMeshProUGUI> names;
    [SerializeField]
    private List<TextMeshProUGUI> scores;
    private string publicNormalLeaderboardKey = "2b1fbd7f8e9539d93caaa9cd7471a3b15668b3b0bb66b880439659cf9ca0a0e6";
    private void Start()
    {
        GetLeaderBoard();
    }
    public void GetLeaderBoard()
    {
        LeaderboardCreator.GetLeaderboard(publicNormalLeaderboardKey, ((msg) =>{
            Debug.Log(msg);
            int loopLength=(msg.Length<names.Count)?msg.Length:names.Count;
            for (int i = 0; i < loopLength; i++)
            {
                names[i].text = msg[i].Username;
                scores[i].text = msg[i].Score.ToString();
            }
        }));
    }
    public void SetLeaderboardEntry(string username,int score)
    {
        ZombieApocalypse.GameData.playerName = username;
        LeaderboardCreator.UploadNewEntry(publicNormalLeaderboardKey, username, score, ((_) =>
        {
            GetLeaderBoard();
        }));

        LeaderboardCreator.ResetPlayer();

        buttonScript.onPlayerNameSubmit();
    }
  
}
