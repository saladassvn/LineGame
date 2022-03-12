using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GridManager : MonoBehaviour
{
    [SerializeField] private int _width, _height;

    [SerializeField] private GameObject _tilePrefab;

    [SerializeField] private Transform _cam;

    private Dictionary<Vector2, Tile> _tiles;


    List<Tile> tileList;

    public Tile[,] tile = new Tile[9,9];

    public int startX = 0;
    public int startY = 0;

    public int endX = 2;
    public int endY = 2;
    public int scale = 1;

    public int requiredTilesInLine = 5;

    enum tilesCompareState { EMPTY, DIFFERENT, SAME}
   

    public bool findDistance = false;
    public bool moveSucess = false;

    public GameObject[,] tileArray = new GameObject[9, 9];

    public List<GameObject> path = new List<GameObject>();

    public BallManager ballManager;
    public GameManager gameManager;

    private void Awake()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }
    private void Update()
    {

        if (findDistance && gameManager.isLost == false)
        {
            SetDistance();
            SetPath();
            findDistance = false;
        }

    }

    private tilesCompareState compareTiles(Tile A, Tile B)
    {
        if (A.isEmpty || B.isEmpty) return tilesCompareState.EMPTY;
        else if (A.currentBall.color == B.currentBall.color) return tilesCompareState.SAME;
        else return tilesCompareState.DIFFERENT;
    }

    private bool checkPointsRow()
    {
        bool achievedPoints = false;

        for (int y = 0; y < _height; ++y)
        {
            int sameColor = 1, start = 0;

            for (int x = 1; x < _width; ++x)
            {
                tilesCompareState compState = compareTiles(tile[x, y], tile[x - 1, y]);

                if (compState == tilesCompareState.SAME)
                {
                    ++sameColor;

                    if (x == _width - 1 && sameColor >= requiredTilesInLine)
                    {
                        achievedPoints = true;
                        removeRow(y, start, x);
                    }
                }
                else
                {
                    if (sameColor >= requiredTilesInLine)
                    {
                        achievedPoints = true;
                        removeRow(y, start, x - 1);
                    }

                    sameColor = 1;
                    start = x;
                }
            }
        }

        return achievedPoints;
    }



    private bool checkPointsColumn()
    {
        bool achievedPoints = false;

        for (int x = 0; x < _height; ++x)
        {
            int sameColor = 1, start = 0;

            for (int y = 1; y < _height; ++y)
            {
                tilesCompareState compState = compareTiles(tile[x, y], tile[x, y - 1]);

                if (compState == tilesCompareState.SAME)
                {
                    ++sameColor;
                    if (y == _height - 1 && sameColor >= requiredTilesInLine)
                    {
                        achievedPoints = true;
                        removeColumn(x, start, y);
                    }
                }
                else
                {
                    if (sameColor >= requiredTilesInLine)
                    {
                        achievedPoints = true;
                        removeColumn(x, start, y - 1);
                    }

                    sameColor = 1;
                    start = y;
                }
            }
        }

        return achievedPoints;
    }

    private void removeRow(int row, int start, int end)
    {
        Debug.Log(string.Format("Remove row: {0}, start: {1}, end: {2}, total tiles: {3}", row + 1, start + 1, end + 1, end - start + 1));

        //game.addPoints(Mathf.Abs(requiredTilesInLine - (end - start + 1)));

        for (int i = start; i <= end; ++i)
        {
            tile[i, row].RemoveBall();
            tile[i, row].isEmpty = true;
        }

        FindObjectOfType<AudioManager>().Play("GetPoint");

    }

    private void removeColumn(int col, int start, int end)
    {
        Debug.Log(string.Format("Remove column: {0}, start: {1}, end: {2}, total tiles: {3}", col + 1, start + 1, end + 1, end - start + 1));

        //game.addPoints(Mathf.Abs(requiredTilesInLine - (end - start + 1)));

        for (int i = start; i <= end; ++i) {
           
           tile[col, i].RemoveBall();
           tile[col, i].isEmpty = true;
        }

        FindObjectOfType<AudioManager>().Play("GetPoint");
    }

    public void GenerateGrid()
    {
        _tiles = new Dictionary<Vector2, Tile>();
        tileList = new List<Tile>();
        
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                GameObject spawnedTile = Instantiate(_tilePrefab, new Vector3(x, y), Quaternion.identity);

                spawnedTile.transform.SetParent(gameObject.transform);

                spawnedTile.name = $"Tile {x} {y}";

                var isOffset = (x % 2 == 0 && y % 2 != 0) || (x % 2 != 0 && y % 2 == 0);

                spawnedTile.GetComponent<Tile>().Init(isOffset);

         

                spawnedTile.GetComponent<Tile>().x = x;
                spawnedTile.GetComponent<Tile>().y = y;

                //tile[x, y] = spawnedTile;
                tileArray[x, y] = spawnedTile; 
                tile[x, y] = spawnedTile.GetComponent<Tile>();



                tileList.Add(tile[x, y]);
                _tiles[new Vector2(x, y)] = spawnedTile.GetComponent<Tile>();

            }
        }
        _cam.transform.position = new Vector3((float)_width / 2 - 0.5f, (float)_height / 2 - 0.5f, -10);
    }

    public Tile GetTileAtPosition(Vector2 pos)
    {
        if (_tiles.TryGetValue(pos, out var tile)) return tile;
        return null;
    }

    public List<Tile> GetEmptyCell()
    {
        Debug.Log(string.Format("Found {0} empty tiles.", tileList.FindAll(t => t.isEmpty).Count));

        return tileList.FindAll(t => t.isEmpty);
    }

    void SetDistance()
    {
        IntialSetup();
        int x = startX;
        int y = startY;
        int[] testArray = new int[_height * _width];
        for(int step = 1; step < _height * _width; step++)
        {
            foreach(GameObject obj in tileArray)
            {
                if(obj.GetComponent<Tile>().visited == step - 1)
                {
                    TestFourDirections(obj.GetComponent<Tile>().x, obj.GetComponent<Tile>().y, step);

                }
            }
        }
    

    }

    void SetPath()
    {
        int step;
        int x = endX;
        int y = endY;
        List<GameObject> tempList = new List<GameObject>();
        path.Clear();
        if(tileArray[endX,endY] && tileArray[endX,endY].GetComponent<Tile>().visited > 0)
        {
            path.Add(tileArray[x, y]);
            step = tileArray[x, y].GetComponent<Tile>().visited - 1;
            
        }
        else
        {
            PlayRandomDeathSoundEffet();
            tileArray[startX, startY].GetComponent<Tile>().isEmpty = false;
            print("Can't reach");
            startX = -1;
            startY = -1;
            endX = -1;
            endY = -1;
            return;
        }

        for(int i = step; step > -1; step--)
        {
            if (TestDirection(x, y, step, 1))
            {
                tempList.Add(tileArray[x, y + 1]);
            }
            if (TestDirection(x, y, step, 2))
            {
                tempList.Add(tileArray[x + 1, y]);
            }
            if (TestDirection(x, y, step, 3))
            {
                tempList.Add(tileArray[x, y - 1]);
            }
            if (TestDirection(x, y, step, 4))
            {
                tempList.Add(tileArray[x - 1, y ]);
            }

            PlayRandomPointSoundEffect();
            GameObject tempObj = FindClosest(tileArray[endX, endY].transform, tempList);
            path.Add(tempObj);
            x = tempObj.GetComponent<Tile>().x;
            y = tempObj.GetComponent<Tile>().y;
            tempList.Clear();
        }

        //Add path to move
        //tileArray[startX, startY].GetComponent<Tile>().currentBall.AddPaths(path);
        tileArray[startX, startY].GetComponent<Tile>().currentBall.Place(tileArray[endX, endY].GetComponent<Tile>());
        tileArray[endX, endY].GetComponent<Tile>().isEmpty = false;



        //Call next 3 balls
        ballManager.Setup(this);

        startX = -1;
        startY = -1;
        endX = -1;
        endY = -1;

        CheckPoint();

    }

    private void TestFourDirections(int x, int y, int step)
    {
        if (TestDirection(x, y, -1, 1) && tile[x,y].isEmpty == true) SetVisited(x, y + 1, step);
        if (TestDirection(x, y, -1, 2) && tile[x, y].isEmpty == true) SetVisited(x + 1, y, step);
        if (TestDirection(x, y, -1, 3) && tile[x, y].isEmpty == true) SetVisited(x, y - 1, step);
        if (TestDirection(x, y, -1, 4) && tile[x, y].isEmpty == true) SetVisited(x - 1, y, step);
    }

    void IntialSetup()
    {
        //Debug.Log(tileArray.Length);
        foreach(GameObject obj in tileArray)
        {
            
            obj.GetComponent<Tile>().visited = -1;

        }

        tileArray[startX, startY].GetComponent<Tile>().visited = 0;

    }

    bool TestDirection(int x, int y, int step, int direction)
    {
        //int direction  tells which case to use 1 is up, 2 is right, 3 is down, 4 is left
        switch (direction)
        {
            case 1:
                if (y + 1 < _width && tileArray[x, y + 1] && tileArray[x, y + 1].GetComponent<Tile>().visited == step && tileArray[x, y + 1].GetComponent<Tile>().isEmpty == true)
                    return true;
                else
                    return false;
            case 2:
                if (x + 1 < _height && tileArray[x+1,y] && tileArray[x + 1, y].GetComponent<Tile>().visited == step && tileArray[x + 1, y].GetComponent<Tile>().isEmpty == true)
                    return true;
                else
                    return false;
            case 3:
                if (y - 1 > - 1 && tileArray[x, y - 1] && tileArray[x, y - 1].GetComponent<Tile>().visited == step && tileArray[x, y - 1].GetComponent<Tile>().isEmpty == true)
                    return true;
                else
                    return false;
            case 4:
                if (x - 1 > -1 && tileArray[x-1, y] && tileArray[x - 1, y ].GetComponent<Tile>().visited == step && tileArray[x - 1, y].GetComponent<Tile>().isEmpty == true)
                    return true;
                else
                    return false;
        }
        return false;
    }

    
    
    public void CheckPoint()
    {
        bool pointsRow = checkPointsRow();
        bool pointsColumn = checkPointsColumn();
    }

    void SetVisited (int x, int y, int step)
    {
        if (tileArray[x, y])
        {
            tileArray[x, y].GetComponent<Tile>().visited = step;
        }
    }

    GameObject FindClosest(Transform targetLocation, List<GameObject> list)
    {
        float currentDistance = _width * _height;
        int indexNumber = 0;

        for(int i = 0; i < list.Count; i++)
        {
            if(Vector3.Distance(targetLocation.position, list[i].transform.position) < currentDistance)
            {
                currentDistance = Vector3.Distance(targetLocation.position, list[i].transform.position);
                indexNumber = i;

            }
        }
        
        return list[indexNumber];
    }

    void PlayRandomPointSoundEffect()
    {
        int i = Random.Range(0, 6);
        if (i == 0)
        {
            FindObjectOfType<AudioManager>().Play("Do");
        }
        else if (i == 1)
        {
            FindObjectOfType<AudioManager>().Play("Re");
        }
        else if (i == 2)
        {
            FindObjectOfType<AudioManager>().Play("Mi");
        }
        else if (i == 3)
        {
            FindObjectOfType<AudioManager>().Play("Fa");
        }
        else if (i == 4)
        {
            FindObjectOfType<AudioManager>().Play("Sol");
        }
        else if (i == 5)
        {
            FindObjectOfType<AudioManager>().Play("La");
        }
        else
        {
            FindObjectOfType<AudioManager>().Play("Si");
        }
    }
    void PlayRandomDeathSoundEffet()
    {
        int i = Random.Range(0, 2);
        if (i == 0)
        {
            FindObjectOfType<AudioManager>().Play("Death1");
        }
        else if (i == 1)
        {
            FindObjectOfType<AudioManager>().Play("Death2");
        }
        else
        {
            FindObjectOfType<AudioManager>().Play("Death3");
        }
    }

}