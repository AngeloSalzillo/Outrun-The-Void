using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Transform player;
    public Vector3 offset;
    void Update()
    {
        transform.position = new Vector3(player.position.z + offset.x, offset.y, player.position.z + offset.z);
    }
}
