using UnityEngine;
using System.Collections;

public class ObstacleGenerator : MonoBehaviour
{
    public GameObject Obstacle1;
    public GameObject WallObstacle1;
    public GameObject WallObstacle2;
    public GameObject Ground;
    public float SpawnInterval = 1f;
    float distanceX;
    float distanceZ;
    float distanceY;
    float wait;
    int count = 0;

    void Start()
    {
        StartCoroutine(SpawnObstacle());
    }

    
    IEnumerator SpawnObstacle()
    {
        while(true)
        {
            wait = SpawnInterval;

            yield return new WaitForSeconds(wait);
            
            if(Time.time < 10f)
                SpawnObstacle1();
            else
            {
                if(count % 5 == 0)
                {
                    SpawnWallObstacle1();
                    count = 0;
                }
                else if(count % 6 == 1)
                {
                    SpawnWallObstacle2Left();
                }
                else if(count % 8 == 4)
                {
                    SpawnWallObstacle2Right();
                }
                else
                    SpawnObstacle1();

                count++;
            }
        }
    }

    void SpawnObstacle1()
    {
        distanceX = UnityEngine.Random.Range(Ground.transform.localScale.x/2 - 6f, -Ground.transform.localScale.x/2 + 6f);
        distanceY = Ground.transform.position.y + Obstacle1.transform.localScale.y/2 + 0.5f;
        distanceZ = Ground.transform.localScale.z*0.5f;
        Instantiate(Obstacle1, new Vector3(distanceX, distanceY, distanceZ), Quaternion.identity); 
    }

    void SpawnWallObstacle1()
    {
        distanceX = 0f;
        distanceY = Ground.transform.position.y - WallObstacle1.transform.localScale.y/2 + 0.5f;
        distanceZ = Ground.transform.localScale.z*0.5f;
        Instantiate(WallObstacle1, new Vector3(distanceX, distanceY, distanceZ), Quaternion.identity);
    }

    void SpawnWallObstacle2Left()
    {
        distanceX = Ground.transform.position.x - (Ground.transform.localScale.x/2 - WallObstacle2.transform.localScale.x/2);
        distanceY = Ground.transform.position.y + WallObstacle2.transform.localScale.y/2 + 0.5f;
        distanceZ = Ground.transform.localScale.z*0.5f;
        Instantiate(WallObstacle2, new Vector3(distanceX, distanceY, distanceZ), Quaternion.identity);
    }

    void SpawnWallObstacle2Right()
    {
        distanceX = Ground.transform.position.x + (Ground.transform.localScale.x/2 - WallObstacle2.transform.localScale.x/2);
        distanceY = Ground.transform.position.y + WallObstacle2.transform.localScale.y/2 + 0.5f;
        distanceZ = Ground.transform.localScale.z*0.5f;
        Instantiate(WallObstacle2, new Vector3(distanceX, distanceY, distanceZ), Quaternion.identity);
    }
}
    