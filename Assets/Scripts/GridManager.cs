using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public int rows = 10;
    public int cols = 10;
    public float cellSize = 1.0f;
    public Transform startingObject;
    public Vector3 startingPoint; // 网格的起始位置
    public GameObject[] prefabs; // 存储所有 Prefab 的数组

    private int[,] gridData;

    public TextAsset csvFile; // 指定的.csv文件


    public GameObject playerPrefab; // 玩家Prefab
    private Vector2Int playerPosition; // 玩家在网格中的位置

    private PlayerController player; // 玩家引用

    private Dictionary<Vector2Int, GameObject> activeObstacles = new Dictionary<Vector2Int, GameObject>();

    void Start()
    {
        startingPoint = startingObject.position;

        // 从.csv文件读取网格数据
        LoadGridDataFromCSV();

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


    void LoadGridDataFromCSV()
    {
        // 使用换行符分隔每一行
        string[] lines = csvFile.text.Split('\n');

        rows = lines.Length;
        cols = lines[0].Split(',').Length;

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
                    Vector3 cellPosition = offset + new Vector3(j * cellSize, 0, i * cellSize);
                    GameObject _obstacle = Instantiate(prefabs[prefabIndex], cellPosition, Quaternion.identity, transform);
                    _obstacle.GetComponent<Obstacle>().gridPosition = new Vector2Int(i,j);
                    activeObstacles.Add(new Vector2Int(i,j), _obstacle);
                }
                else if (gridData[i, j] == 1001)
                {
                    Vector3 worldPos = GetWorldPositionFromGridPosition(j, i);
                    player = Instantiate(playerPrefab, worldPos, Quaternion.identity).GetComponent<PlayerController>();
                    player.gridManager = this;

                    gridData[i, j] = 0;
                    playerPosition = new Vector2Int(j, i);
                    player.goalPlayerPosition = playerPosition;
                    player.goalPlayerPosVector3 = worldPos;

                }
            }
        }
    }


    public Vector2Int TryMovePlayer(Vector2Int direction)
    {
        //Vector3 moveDirection = new Vector3(direction.y, 0, direction.x); // 注意：这里的x和y需要交换，因为我们在一个水平的网格上

        //RaycastHit hit;
        //bool hasObstacle = false;

        for (int x = playerPosition.y; x >= 0 && x < rows; x += direction.x)
        {
            for (int y = playerPosition.x; y >= 0 && y < cols; y += direction.y)
            {
                if (gridData[x,y] != 0)
                {

                    if (activeObstacles.TryGetValue(new Vector2Int(x,y), out GameObject obstacle)) {

                        obstacle.GetComponent<Obstacle>().IsHittedFrom(direction);

                        if (obstacle.GetComponent<Obstacle>().WillStopPlayer())
                        {

                            break;
                        }
                    }

                }

                playerPosition = new Vector2Int(x + direction.x, y + direction.y);
            }
        }

        return playerPosition;

    }

}
