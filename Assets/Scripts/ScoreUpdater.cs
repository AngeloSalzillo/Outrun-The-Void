using UnityEngine;
using TMPro;


public class ScoreUpdater : MonoBehaviour
{
    public TMP_Text scoreText;
    public GameObject Player;
    public GameObject Ground;
    public LayerMask obstacleLayer;
    int score = 0;
    bool obstaclePassed = false;
    float rayDistance;
    bool stopCheck = false;

    void Start()
    {
        scoreText.text = " ";
    }

    // Update is called once per frame
    void Update()
    {
        obstaclePassed = checkObstaclePassed();

        if (obstaclePassed == true)
        {
            score += 1;
            scoreText.text = score.ToString();
            obstaclePassed = false;
        }
    }

    bool checkObstaclePassed()
    {
        Vector3 origin = new Vector3(Player.transform.position.x, 1f, 0f);
        Vector3 rightLimit = new Vector3(Ground.GetComponent<Transform>().localScale.x/2, 0f, 0f);
        Vector3 leftLimit = new Vector3(-Ground.GetComponent<Transform>().localScale.x/2, 0f, 0f);

        rayDistance = Mathf.Abs(rightLimit.x - origin.x);
        bool obstacleDetectedRight = Physics.Raycast(origin, Vector3.right, rayDistance, obstacleLayer);
       
        rayDistance = Mathf.Abs(origin.x - leftLimit.x);
        bool obstacleDetectedLeft = Physics.Raycast(origin, Vector3.left, rayDistance, obstacleLayer);


        if(!stopCheck && (obstacleDetectedRight || obstacleDetectedLeft))
        {
            stopCheck = true; 
            return true;    
        }
        else if(stopCheck && !(obstacleDetectedRight || obstacleDetectedLeft))
        {
            stopCheck = false;
            return false;
        }
        else 
            return false;
    }
}
