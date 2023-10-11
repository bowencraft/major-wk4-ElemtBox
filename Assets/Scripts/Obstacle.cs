using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{

    public int id;
    public bool durabillty;
    public bool stopable = true;
    public Vector2Int gridPosition;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void IsHittedFrom(Vector2Int direction)
    {
        if (id == 1)
        {
            return;
        } else if (id == 2)
        {
            Destroy(this.gameObject);
            FindObjectOfType<GridManager>().GetComponent<GridManager>().ClearCellAt(gridPosition);
        } else if (id == 3)
        {
            FindObjectOfType<GridManager>().GetComponent<GridManager>().PlayerWin();
            foreach (PlayerController item in FindObjectsOfType<PlayerController>())
            {
                item.remainingMoves = 10000;
            }
        }
    }

    public bool WillStopPlayer()
    {
        return stopable;
    }
}
