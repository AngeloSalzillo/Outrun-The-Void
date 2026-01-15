using UnityEngine;
public class SpawnGround : MonoBehaviour
{
    public GameObject Ground;
    public float BackwardVelocity = 50f;
    GameObject NextGround;
    float PositionZ;
    string GroundNeeded = "None";

    void Start()
    {
        // Spawn the next ground piece
        PositionZ = Ground.transform.position.z + Ground.GetComponent<Renderer>().transform.localScale.z;
        NextGround = Instantiate(Ground, new UnityEngine.Vector3(0, 0, PositionZ), UnityEngine.Quaternion.identity);
        NextGround.GetComponent<SpawnGround>().enabled = false;
        NextGround.GetComponent<ObstacleGenerator>().enabled = false;        
    }
    void FixedUpdate()
    {
       // Move both ground pieces backward
       if(Ground.transform.position.z < NextGround.transform.position.z)
       {
           Ground.GetComponent<Rigidbody>().MovePosition(Ground.transform.position + new UnityEngine.Vector3(0, 0, -BackwardVelocity*Time.fixedDeltaTime));
           NextGround.GetComponent<Rigidbody>().MovePosition(Ground.GetComponent<Rigidbody>().position + new Vector3(0, 0, Ground.GetComponent<Transform>().localScale.z + 30));
       }
       if(NextGround.transform.position.z < Ground.transform.position.z)
       {
           NextGround.GetComponent<Rigidbody>().MovePosition(NextGround.transform.position + new UnityEngine.Vector3(0, 0, -BackwardVelocity*Time.fixedDeltaTime));
           Ground.GetComponent<Rigidbody>().MovePosition(NextGround.GetComponent<Rigidbody>().position + new Vector3(0, 0, Ground.GetComponent<Transform>().localScale.z + 30));
       }
       

       if (Ground.transform.position.z < -270)
           GroundNeeded = "Ground";
    
       if (NextGround.transform.position.z < -270)
           GroundNeeded = "NextGround";

        if (GroundNeeded != "None")
        {
            // Spawn a new ground piece ahead
            if(GroundNeeded == "Ground")
            {   
                GroundNeeded = "None";
                PositionZ = NextGround.transform.position.z + Ground.GetComponent<Renderer>().transform.localScale.z;
                Ground.GetComponent<Rigidbody>().MovePosition(new UnityEngine.Vector3(0, 0, PositionZ));
            }
            if(GroundNeeded == "NextGround")
            {
                GroundNeeded = "None";
                PositionZ = Ground.transform.position.z + NextGround.GetComponent<Renderer>().transform.localScale.z;
                NextGround.GetComponent<Rigidbody>().MovePosition(new UnityEngine.Vector3(0, 0, PositionZ));
            }
        }
    }
}

