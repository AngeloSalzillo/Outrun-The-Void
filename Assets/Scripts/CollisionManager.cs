using System.Runtime.InteropServices;
using UnityEngine;

public class CollisionManager : MonoBehaviour
{
    public GameObject Player;
    public GameObject Ground;
    public GameObject Camera;
    void OnCollisionEnter(Collision collisionInfo)
    { 
        if(collisionInfo.collider.tag == "WallObstacle")
        {
            GameObject wallPiece = collisionInfo.collider.gameObject;
            GameObject wallObstacle = wallPiece.transform.parent.gameObject;
            wallObstacle.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        }

        if(collisionInfo.collider.tag == "Obstacle" || collisionInfo.collider.tag == "WallObstacle")
        {
            Player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            Player.GetComponent<PlayerMovement>().enabled = false;
            
            Ground.GetComponent<SpawnGround>().stopGround = true;
            Ground.GetComponent<ObstacleGenerator>().enabled = false;
        }
    }
}
