using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public Color _baseColor, _offsetColor;
    [SerializeField] private SpriteRenderer _renderer;
    [SerializeField] private GameObject _highlight;
    GameManager gameManager;

    public Vector2Int boardPosition = Vector2Int.zero;

    public GridManager board = null;

    //public RectTransform rectTransform = null;

    public Ball currentBall = null;

    public bool isEmpty = true;

    public int visited = -1;
    public int x = 0;
    public int y = 0;

    
    GridManager gridManager;

    private void Start()
    {
        gridManager = GameObject.FindGameObjectWithTag("GridManager").GetComponent<GridManager>();
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    private void OnMouseDown()
    {

        if((gridManager.startX == -1 && gridManager.startY == -1))
        {
                if (isEmpty == false)
                {
                    gridManager.startX = x;
                    gridManager.startY = y;
                    isEmpty = true;
                }
        }
        else if((gridManager.endX == -1 && gridManager.endY == -1))
        {
            gridManager.endX = x;
            gridManager.endY = y;

        }


        if((gridManager.startX != -1 && gridManager.startY != -1) && (gridManager.endX != -1 && gridManager.endY != -1))
        {
            gridManager.findDistance = true;
            
        }
      
    }

    public void RemoveBall()
    {
        currentBall.RemoveBall();
    }

    public void Init(bool isOffset)
    {
        _renderer.color = isOffset ? _offsetColor : _baseColor;
    }

    void OnMouseEnter()
    {
        if(gameManager.isLost == false)
        {
            _highlight.SetActive(true);
        }
    }

    void OnMouseExit()
    {
        if (gameManager.isLost == false)
        {
            _highlight.SetActive(false);
        }
        
    }
}