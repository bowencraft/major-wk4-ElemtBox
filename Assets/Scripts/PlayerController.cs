using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float moveSpeed = 5f;
    public GridManager gridManager;

    public Vector2Int goalPlayerPosition;
    public Vector3 goalPlayerPosVector3;

    private float tolerance = 0.1f;

    private float t = 1f;
    public int remainingMoves = 10;

    public TextMeshPro hudText;

    void Start()
    {
        gridManager = FindObjectOfType<GridManager>();
        moveSpeed = gridManager.moveSpeed;
        tolerance = gridManager.tolerance;
        remainingMoves = gridManager.remainingMoves;
        hudText = gridManager.remainHudText;
    }
    void Update()
    {
        if (t < 1f)
        {
            t += Time.deltaTime * moveSpeed;
            transform.position = Vector3.Lerp(transform.position, goalPlayerPosVector3, t);
        }
        if (remainingMoves == 0)
        {
            hudText.text = ("Remain Step:\n" + remainingMoves + "\nYou Failed.");
            //Debug.Log("You lose.");
            return; // print you lose
        } else if (remainingMoves < 0)
        {
            return; // restart
        } else
        {
            hudText.text = ("Remain Step:\n" + remainingMoves);
        }

        Vector2Int moveDirection = Vector2Int.zero;

        if (Input.GetKeyDown(KeyCode.W)) moveDirection = new Vector2Int(-1, 0);
        if (Input.GetKeyDown(KeyCode.S)) moveDirection = new Vector2Int(1, 0);
        if (Input.GetKeyDown(KeyCode.A)) moveDirection = new Vector2Int(0, -1);
        if (Input.GetKeyDown(KeyCode.D)) moveDirection = new Vector2Int(0, 1);

        //Debug.Log(moveDirection);

        if (moveDirection != Vector2Int.zero && (Vector3.Distance(goalPlayerPosVector3, this.transform.position) < tolerance))
        {
            Debug.Log(moveDirection);
            goalPlayerPosition = gridManager.TryMovePlayer(moveDirection,goalPlayerPosition);
            goalPlayerPosVector3 = gridManager.GetWorldPositionFromGridPosition(goalPlayerPosition.x, goalPlayerPosition.y);
            t = 0f; 
            remainingMoves--;
        }

    }


    public void StopMovement()
    {
        return;
    }
}
