using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.AI;

public class Ball : MonoBehaviour
{
    [HideInInspector] public Color color = Color.clear;
    
    BallManager ballManager;
    Tile currentTile = null;
    GameManager gameManager;
    [SerializeField] private GameObject _highlight;
    public bool isFirstClick = true;
    //int currentWaypoint = 0;
    //Rigidbody2D rigidB;
    [SerializeField] float moveSpeed = 10;

    List<GameObject> path = new List<GameObject>();
    public bool isMoving = false;

    public virtual void Setup(Color newColor, Color32 newSpriteColor, BallManager newBallManager)
    {
        ballManager = newBallManager;
        color = newColor;
        GetComponent<SpriteRenderer>().color = newSpriteColor;
    }

    public void Place(Tile newTile)
    {
        transform.position = newTile.transform.position;
        currentTile = newTile;
        currentTile.currentBall = this;
    }

    public void RemoveBall()
    {
        gameManager.point += 1;
        ballManager.Explode(this);
        GameObject.Destroy(this.gameObject);

    }

    Transform nextPos;
    int nextPosIndex;
    private void Start()
    {
        //rigidB = GetComponent<Rigidbody2D>();
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        ballManager = GameObject.FindGameObjectWithTag("BallManager").GetComponent<BallManager>();

        
    }

    //private void Update()
    //{
    //    Movement();
    //}

    //void Movement()
    //{
    //    if(isMoving == true)
    //    {
    //        //

    //    }


    //}

    //public void AddPaths(List<GameObject> path)
    //{
    //    this.path.Clear();
    //    this.path = path;
    //    for (int i = path.Count - 1; i > 0; i--)
    //    {

    //        Debug.Log(this.path[i].transform.position);
    //    }
    //    isMoving = true;       
    //}





}
