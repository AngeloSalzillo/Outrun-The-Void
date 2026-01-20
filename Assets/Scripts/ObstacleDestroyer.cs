using UnityEngine;

public class ObstacleDestroyer : MonoBehaviour
{
    public GameObject Obstacle;
    public GameObject Ground;

    // Update is called once per frame
    void Update()
    {
        if(Obstacle.transform.position.z < -30f)
           Destroy(Obstacle);
    }
}
