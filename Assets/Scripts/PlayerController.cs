using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public GridManager gridManager;

    public Vector2Int goalPlayerPosition;
    public Vector3 goalPlayerPosVector3;

    private float t = 1f; // 初始化为 1，表示玩家在开始时已经在目标位置


    void Start()
    {
        gridManager = FindObjectOfType<GridManager>();
    }

    void Update()
    {
        Vector2Int moveDirection = Vector2Int.zero;

        if (Input.GetKeyDown(KeyCode.W)) moveDirection = new Vector2Int(1,0);
        if (Input.GetKeyDown(KeyCode.S)) moveDirection = new Vector2Int(-1,0);
        if (Input.GetKeyDown(KeyCode.A)) moveDirection = new Vector2Int(0,-1);
        if (Input.GetKeyDown(KeyCode.D)) moveDirection = new Vector2Int(0,1);

        Debug.Log(moveDirection);

        if (moveDirection != Vector2Int.zero && (goalPlayerPosVector3 == this.transform.position))
        {
            //Debug.Log(moveDirection);
            goalPlayerPosition = gridManager.TryMovePlayer(moveDirection);
            goalPlayerPosVector3 = gridManager.GetWorldPositionFromGridPosition(goalPlayerPosition.x, goalPlayerPosition.y);
            t = 0f; // 重置 t，开始插值
        }

        if (t < 1f)
        {
            t += Time.deltaTime * moveSpeed; // 增加 t
            transform.position = Vector3.Lerp(transform.position, goalPlayerPosVector3, t);
        }
    }


    public void StopMovement()
    {
        return;
    }
}
