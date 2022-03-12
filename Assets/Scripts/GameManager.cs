using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{


    public GridManager board;
    public BallManager ballManager;
    public bool isLost = false;

    public int point = 0;
    public TextMeshProUGUI pointText;

    public TextMeshProUGUI highScoreText;
    public TextMeshProUGUI YourScore;

    public GameObject newHighScoreText;
    public GameObject newHighScoreText2;

    private void Awake()
    {
        board.GenerateGrid();
    }

    private void Start()
    {
        ballManager.Setup(board);
        highScoreText.SetText("High Score: " + PlayerPrefs.GetInt("Scores").ToString());
    }

    public void LoadMenuScene()
    {
        SceneManager.LoadScene(0);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(1);
    }

    private void Update()
    {
        pointText.text = point.ToString();

        if (Input.GetKeyDown(KeyCode.A))
        {
            ballManager.Setup(board);
        }
    }

    public void SaveHighScore()
    {
        YourScore.SetText("Your Score: " + point.ToString());
        if (PlayerPrefs.GetInt("Scores") < point)
        {
            PlayerPrefs.SetInt("Scores", point);
            highScoreText.SetText("High Score: " + PlayerPrefs.GetInt("Scores").ToString());
            newHighScoreText.SetActive(true);
            newHighScoreText2.SetActive(true);
        }


    }
}
 