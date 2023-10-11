using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GridManager : MonoBehaviour
{
    [Header("=== Grid ===")]
    public int rows = 10;
    public int cols = 10;
    public float cellSize = 1.0f;
    public Transform startingObject;

    private Vector3 startingPoint; // 网格的起始位置

    [Header("=== Prefabs ===")]
    public GameObject[] obstaclePrefabs; // 存储所有 Prefab 的数组
    public GameObject playerPrefab; // 玩家Prefab


    [Header("=== Play Settings ===")]
    public float moveSpeed = 5f;
    public float tolerance = 0.1f;
    public int remainingMoves;

    private int[,] gridData;

    //[Header("=== Data ===")]
    //public TextAsset csvFile; // 指定的.csv文件


    [Header("=== UI ===")]
    public TextMeshPro remainHudText;
    public TextMeshPro winHudText;



    private GameObject gridCanvas;


    private Dictionary<Vector2Int, GameObject> activeObstacles = new Dictionary<Vector2Int, GameObject>();

    void Start()
    {
        startingPoint = startingObject.position;
        remainingMoves = FindObjectOfType<PlayerSettings>().PlayerRemainSteps;

        gridCanvas = GameObject.Find("GridCanvas");

        // 从.csv文件读取网格数据
        //LoadGridDataFromCSV();
        LoadGridDataFromCSV(FindObjectOfType<PlayerSettings>().csvData);

        // 根据网格数据创建网格
        CreateGrid();


    }


    public Vector3 GetWorldPositionFromGridPosition(int x, int y)
    {
        // 计算网格的宽度和长度
        float totalWidth = (cols - 1) * cellSize;
        float totalLength = (rows - 1) * cellSize;

        // 计算起始偏移
        Vector3 offset = startingPoint - new Vector3(totalWidth / 2, 0, totalLength / 2);

        return offset + new Vector3(x * cellSize, 0, y * cellSize);
    }


    void LoadGridDataFromCSV(string csvText)
    {
        string[] lines = csvText.Split('\n');

        rows = lines.Length;
        cols = lines[0].Split(',').Length;

        SpriteRenderer _canvasRenderer = gridCanvas.GetComponent<SpriteRenderer>();
        _canvasRenderer.size = new Vector2(rows, cols);

        gridData = new int[rows, cols];

        for (int i = 0; i < rows; i++)
        {
            int[] values = lines[i].Split(',').Select(int.Parse).ToArray();
            for (int j = 0; j < cols; j++)
            {
                gridData[i, j] = values[j];
            }
        }
    }


    void CreateGrid()
    {
        // 计算整个网格的宽度和长度
        float totalWidth = (cols - 1) * cellSize;
        float totalLength = (rows - 1) * cellSize;

        // 计算第一个格子的位置，使整个网格的中心与配置的起始位置对齐
        Vector3 offset = startingPoint - new Vector3(totalWidth / 2, 0, totalLength / 2);

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                if (gridData[i, j] != 0 && gridData[i, j] != 1001)
                {
                    int prefabIndex = gridData[i, j] - 1; // 因为gridData中的1对应prefabs的第0个元素，2对应第1个元素，以此类推
                    Vector3 cellPosition = offset + new Vector3(i * cellSize, 0, j * cellSize);
                    Obstacle _obstacle = Instantiate(obstaclePrefabs[prefabIndex], cellPosition, Quaternion.identity, transform).GetComponent<Obstacle>();
                    _obstacle.gridPosition = new Vector2Int(i,j);
                    _obstacle.id = gridData[i, j];
                    activeObstacles.Add(new Vector2Int(i,j), _obstacle.gameObject);
                }
                else if (gridData[i, j] == 1001)
                {
                    Vector3 worldPos = GetWorldPositionFromGridPosition(i, j);
                    PlayerController player = Instantiate(playerPrefab, worldPos, Quaternion.identity).GetComponent<PlayerController>();
                    player.gridManager = this;

                    gridData[i, j] = 0;


                    Vector2Int playerPosition = new Vector2Int(i, j);
                    player.goalPlayerPosition = playerPosition;
                    player.goalPlayerPosVector3 = worldPos;

                }
            }
        }
    }


    public Vector2Int TryMovePlayer(Vector2Int direction, Vector2Int playerPosition)
    {
        //Vector3 moveDirection = new Vector3(direction.y, 0, direction.x); // 注意：这里的x和y需要交换，因为我们在一个水平的网格上

        //RaycastHit hit;
        //bool hasObstacle = false;

        Vector2Int currentPosition = playerPosition;
        //gridData[currentPosition.x, currentPosition.y] = 0;

        while (currentPosition.x >= 0 && currentPosition.x < rows && currentPosition.y >= 0 && currentPosition.y < cols)
        {
            if (gridData[currentPosition.x, currentPosition.y] != 0)
            {
                if (activeObstacles.TryGetValue(currentPosition, out GameObject obstacle))
                {
                    obstacle.GetComponent<Obstacle>().IsHittedFrom(direction);

                    if (obstacle.GetComponent<Obstacle>().WillStopPlayer())
                    {
                        break;
                    }
                }
            }

            //gridData[currentPosition.x, currentPosition.y] = 1001;

            playerPosition = currentPosition; // update the player's position
            currentPosition += direction; // move to the next position based on the direction

        }


        return playerPosition;

    }

    public void ClearCellAt(Vector2Int position)
    {
        gridData[position.x, position.y] = 0;
        activeObstacles.Remove(position);
    }

    public void BackToSetting()
    {
        SceneManager.LoadScene(1);
    }

    public void RestartLevel()
    {
        //Debug.Log("Back to settings!");
        SceneManager.LoadScene(2);
    }

    public void PlayerWin()
    {
        remainHudText.gameObject.SetActive(false);
        winHudText.gameObject.SetActive(true);

    }
}
