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
        return;
    }

    public bool WillStopPlayer()
    {
        return stopable;
    }
}
