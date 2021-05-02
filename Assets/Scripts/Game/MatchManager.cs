using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Linq;

public class MatchManager : MonoBehaviour
{
    public static MatchManager i;
    public int maxScore = 10;
    Dictionary<string, Score> scores = new Dictionary<string, Score>();
    public TextMeshProUGUI scoreText;


    void Awake()
    {
        if (i == null)
            i = this;
        else
        {
            Destroy(gameObject);
            return;
        }
        scoreText.text = "";
    }

    public void AddAllPlayers(GameObject[] players)
	{
		foreach (GameObject p in players)
		{
            scores.Add(p.name, new Score(p.name));
		}
        UpdateScores();
	}

    public void AddFrag(string name)
	{
        Score score;
        if (scores.TryGetValue(name, out score))
		{
            score.frags += 1;
		}
        UpdateScores();
    }

    public void AddDeath(string name)
    {
        Score score;
        if (scores.TryGetValue(name, out score))
        {
            score.deaths += 1;
        }
        UpdateScores();
    }

    void UpdateScores()
	{
        string newScoreText = "";
        List<Score> newScoresList = scores.Values.OrderBy(o => o.frags).ToList(); ;
		foreach (Score s in scores.Values)
		{
            if(s.frags >= maxScore)
			{
                Debug.Log(s.name + " won the match!");
                SceneManager.LoadScene("Menu");
            }
            string scoreString = s.name + "\nFRAGS: " + s.frags + "   DEATHS: " + s.deaths + "\n\n";
            newScoreText += scoreString;

		}
        scoreText.text = newScoreText;
	}
}
