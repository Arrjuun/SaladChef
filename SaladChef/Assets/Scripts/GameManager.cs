using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public bool[] CustomersPresent = new bool[5];
    float[] XTransforms = { -1f, -0.5f, 0f, 0.5f, 1f };

    public Transform CustomerHolder;
    public GameObject CustomerPrefab;

    public PlayerMovement Player1;
    public PlayerMovement Player2;

    public GameObject[] PowerUpBoosters;

    public GameObject MenuPanel;

    public GameObject WinnerText;

    public GameObject HighScorePanel;

    public Transform HighScoreHolder;

    public GameObject HighScorePrefab;

    void Update()
    {
        if(!Constants.GameOver)
        {
            PopulateCustomers();
            isTimeOver();
        }
    }

    void PopulateCustomers()
    {
        for (int i = 0; i < CustomersPresent.Length; i++)
        {
            if (!CustomersPresent[i])
            {
                GameObject customerObject = Instantiate(CustomerPrefab, CustomerHolder);
                customerObject.name = $"Customer{i}";
                customerObject.transform.localPosition = new Vector3(XTransforms[i], customerObject.transform.localPosition.y);
                Customer customer = customerObject.GetComponent<Customer>();
                customer.CustomerIndex = i;
                customer.gameManager = this;
                CustomersPresent[i] = true;
            }
        }
    }

    void isTimeOver()
    {
        if(Player1.TimeLeft <= 0 && Player2.TimeLeft <= 0)
        {
            Constants.GameOver = true;
            CustomersPresent = new bool[5];
            MenuPanel.SetActive(true);
            Text winnerAnnouncementText = WinnerText.GetComponent<Text>();
            if(Player1.score > Player2.score)
            {
                winnerAnnouncementText.text = $"Player1 won";
            }
            else if(Player1.score < Player2.score)
            {
                winnerAnnouncementText.text = $"Player2 won";
            }
            else
            {
                winnerAnnouncementText.text = $"It's a draw";
            }
            AddHighScore(Player1.score, Player2.score);
            WinnerText.SetActive(true);
        }
    }

    public void SpawnBooster(string playerName)
    {
        GameObject powerUpObject = Instantiate(PowerUpBoosters[Random.Range(0,3)]);
        powerUpObject.transform.position = new Vector3(Random.Range(-5, 5), Random.Range(-1, 1));
        PowerUp powerUp = powerUpObject.GetComponent<PowerUp>();
        powerUp.PlayerName = playerName;
    }

    public void PlayGame()
    {
        Player1.ResetPlayer();
        Player2.ResetPlayer();
        Constants.GameOver = false;
        MenuPanel.SetActive(false);
        WinnerText.SetActive(false);
    }

    public void ShowHighScore()
    {
        GameObject heading = Instantiate(HighScorePrefab, HighScoreHolder);
        Text[] headingTexts = heading.GetComponentsInChildren<Text>();
        headingTexts[0].text = "Score";
        headingTexts[0].fontStyle = FontStyle.BoldAndItalic;
        headingTexts[1].text = "Player";
        headingTexts[1].fontStyle = FontStyle.BoldAndItalic;

        HighScorePanel.SetActive(true);

        if (PlayerPrefs.HasKey("HighScores"))
        {
            string json = PlayerPrefs.GetString("HighScores");
            HighScores highScores = JsonUtility.FromJson<HighScores>(json);

            foreach (HighScore hs in highScores.highScores)
            {
                GameObject HighScoreInst = Instantiate(HighScorePrefab, HighScoreHolder);
                Text[] texts = HighScoreInst.GetComponentsInChildren<Text>();
                texts[0].text = hs.Score.ToString();
                texts[1].text = hs.PlayerName.ToString();
            }
        }
    }


    void AddHighScore(int player1Score, int player2Score)
    {
        if(PlayerPrefs.HasKey("HighScores"))
        {
            string json = PlayerPrefs.GetString("HighScores");
            HighScores highScores = JsonUtility.FromJson<HighScores>(json);
            var allHighScores = highScores.highScores.OrderByDescending(x => x.Score).ToList();

            if(allHighScores.Count < 10)
            {
                allHighScores.Add(new HighScore() { Score = player1Score, PlayerName = "Player1" });
                allHighScores = allHighScores.OrderByDescending(x => x.Score).ToList();
            }
            else
            {
                if(player1Score >= allHighScores[0].Score)
                {
                    allHighScores.Insert(0, new HighScore() { Score = player1Score, PlayerName = "Player1" });
                    allHighScores.RemoveAt(10);
                }
            }

            if (allHighScores.Count < 10)
            {
                allHighScores.Add(new HighScore() { Score = player2Score, PlayerName = "Player2" });
                allHighScores = allHighScores.OrderByDescending(x => x.Score).ToList();
            }
            else
            {
                if (player2Score >= allHighScores[0].Score)
                {
                    allHighScores.Insert(0, new HighScore() { Score = player2Score, PlayerName = "Player2" });
                    allHighScores.RemoveAt(10);
                }
            }

            HighScores newHighScores = new HighScores() { highScores = allHighScores };
            string newJson = JsonUtility.ToJson(newHighScores);

            PlayerPrefs.SetString("HighScores", newJson);

        }

        else
        {
            HighScore p1 = new HighScore() { PlayerName = "Player1", Score = player1Score };
            HighScore p2 = new HighScore() { PlayerName = "Player2", Score = player2Score };

            List<HighScore> hs = new List<HighScore>();
            hs.Add(p1);
            hs.Add(p2);
            hs = hs.OrderByDescending(x => x.Score).ToList();
            HighScores newHighScores = new HighScores() { highScores = hs };
            string newJson = JsonUtility.ToJson(newHighScores);
            PlayerPrefs.SetString("HighScores", newJson);
        }
    }

    public void CloseHighScorePanel()
    {
        HighScorePanel.SetActive(false);
        foreach(Transform highScores in HighScoreHolder)
        {
            Destroy(highScores.gameObject);
        }
    }
}
