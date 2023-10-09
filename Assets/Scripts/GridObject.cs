using UnityEngine;

public class GridObject : MonoBehaviour
{
    public virtual void OnPlayerEnter(PlayerController player)
    {
        // 默认行为：玩家撞到物体时停下
        player.StopMovement();
    }
}

public class DestructibleWall : GridObject
{
    public override void OnPlayerEnter(PlayerController player)
    {
        // 玩家撞到墙时停下，然后销毁墙
        player.StopMovement();
        Destroy(gameObject);
    }
}

public class MovableWall : GridObject
{
    public override void OnPlayerEnter(PlayerController player)
    {
        // 玩家撞到墙时停下，然后移动墙
        player.StopMovement();
        // 移动墙的逻辑...
    }
}
