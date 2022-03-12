using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BallManager : MonoBehaviour
{
    public GameObject ballPrefab;
    Ball[] balls;

    public Tile tile1, tile2, tile3;

    private Ball _selected;

    public Ball selected { get { return _selected; } set { _selected = value; } }

    public GameObject RestartMenu;

    public GameObject explosion;

    GameManager gameManager;

    public Animator camAnim;

    

    private void Awake()
    {
        balls = GetComponentsInChildren<Ball>();
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    void CamShake()
    {
        camAnim.SetTrigger("Shake");
    }

    public void Explode(Ball ball)
    {
        GameObject firework = Instantiate(explosion, ball.transform.position, Quaternion.identity);
        firework.GetComponent<ParticleSystem>().Play();
        CamShake();
    }

    bool isFirstTime = true;
    List<Ball> nextBall = new List<Ball>();

    public void Setup(GridManager board)
    {
        

        if (isFirstTime == true)
        {
            List<Ball> ball = CreateBall(board);
            SpawnBall(ball, board);
            isFirstTime = false;
        }
        else
        {
            //ball = nextBall;
            //Debug.Log(ball.Count);
            SpawnBall(nextBall, board);
            nextBall.Clear();
            
        }

        nextBall = CreateBall(board);
        ShowNextBall(nextBall);
    }

    public void ShowNextBall(List<Ball> newBalls)
    {
        newBalls[0].Place(tile1);
        newBalls[1].Place(tile2);
        newBalls[2].Place(tile3);

    }

    private List<Ball> CreateBall(GridManager board)
    {
        List<Ball> newBalls = new List<Ball>();

        for (int i = 0; i < 3; i++)
        {
            GameObject newBallObject = Instantiate(ballPrefab);
            newBallObject.transform.SetParent(transform);

            // Set scale and position
            newBallObject.transform.localScale = new Vector3(1, 1, 1);
            newBallObject.transform.localRotation = Quaternion.identity;

            Type ballType = typeof(Ball);

            Ball newBall = (Ball)newBallObject.AddComponent(ballType);

            
            newBalls.Add(newBall);
                     
           int j = Random.Range(0, 6);
           Color color;
           Color32 spriteColor;

            if (j == 0)
            {
                color = Color.yellow;
                spriteColor = new Color32(254, 224, 30, 255);
            }
            else if (j == 1)
            {
                color = Color.blue;
                spriteColor = new Color32(80, 124, 159, 255);
            }
            else if (j == 2)
            {
                color = Color.magenta;
                spriteColor = new Color32(196, 0, 255, 255);
            }
            else if (j == 3)
            {
                color = Color.cyan;
                spriteColor = new Color32(166, 254, 30, 255);
            }
            else if (j == 4)
            {
                color = Color.red;
                spriteColor = new Color32(255, 0, 0, 255);
            }
            else
            {
                color = Color.green;
                spriteColor = new Color32(0, 255, 49, 255);

            }

            newBall.Setup(color, spriteColor, this);
        }
        
        return newBalls;
    }


    
    public void SpawnBall(List<Ball> newBalls, GridManager board)
    {

        List<Tile> possibleTiles = board.GetEmptyCell();

        if(possibleTiles.Count < 5)
        {
            gameManager.SaveHighScore();
            RestartMenu.SetActive(true);
            gameManager.isLost = true;
            
        }
        else
        {
            foreach (var ball in newBalls)
            {
                Tile pos = possibleTiles[Random.Range(0, possibleTiles.Count)];
                possibleTiles.Remove(pos);
                pos.isEmpty = false;
                ball.Place(pos);

            }

        }

    }

}
