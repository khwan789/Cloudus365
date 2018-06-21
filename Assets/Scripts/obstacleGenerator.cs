using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class obstacleGenerator : MonoBehaviour {
    
    private float timer;
    private List<GameObject> obstacles;

    private GameObject player;
    private float playerAngle;
    private float obstacleAngle;
    private float angleDistance;

    private int area;
    private float randomSpownTime;
    private float nextSpownTime;

    private int currentArea;
    private int nextArea;


    private bool placeWall;
    private int wallSpowningTime;
    private bool changedArea;

    private bool isMovingLeft;
    
    // Use this for initialization
    void Start()
    {
        timer = GameObject.Find("timer").GetComponent<Timer>().GetTime;
        ResetSpownTime();

        player = GameObject.FindGameObjectWithTag("Player");

        playerAngle = Vector2.Angle(new Vector2(0, 1), player.transform.position);
        angleDistance = 25;
        obstacleAngle = playerAngle + angleDistance;

        currentArea = 1;
        nextArea = currentArea;

        changedArea = false;
        placeWall = false;
        isMovingLeft = false;
        wallSpowningTime = 10;
    }

    // Update is called once per frame
    void Update()
    {
        timer = GameObject.Find("timer").GetComponent<Timer>().GetTime;

        float tempAngle = Vector2.Angle(new Vector2(-1,0), player.transform.position);


        if (player.transform.position.y >= 0)
        {
            playerAngle = tempAngle;
        }
        else
        {
            playerAngle = 180 + (180 - tempAngle);
        }

       // print("player angle " + playerAngle);

        if (isMovingLeft == false)
        {
            obstacleAngle = playerAngle + angleDistance;

        }
        else
        {
            if(playerAngle <= angleDistance)
            {
                obstacleAngle = 360 + playerAngle - angleDistance;
            }else
            {
                obstacleAngle = playerAngle - angleDistance;

            }
        }

        if (timer >= nextSpownTime)
        {
            // every few second
            GenerateObstcale();
            ResetSpownTime();
        }
        //print("Current type " + currentArea + " Next type: " + nextArea);

        if ((playerAngle >=175 && playerAngle <= 185)|| (playerAngle >= 345 && playerAngle <= 355)){
            // change current type
            if(currentArea != nextArea) 
            {
                currentArea = nextArea;
            }
        }

                
    }
    private void ResetSpownTime()
    {
        if (timer > 1 && timer <= 30)
        {
            randomSpownTime = Random.Range(4, 6);
            nextSpownTime = timer + randomSpownTime;
        }
        else if (timer > 30 && timer <= 60)
        {
            randomSpownTime = Random.Range(3, 5);
            nextSpownTime = timer + randomSpownTime;

        }
        else if (timer > 60 && timer <= 90)
        {
            randomSpownTime = Random.Range(2, 4);
            nextSpownTime = timer + randomSpownTime;

        }
        else
        {
            randomSpownTime = Random.Range(2, 3);
            nextSpownTime = timer + randomSpownTime;
        }
    }

    private void GenerateObstcale()
    {
        Vector2 pos = new Vector2(Mathf.Cos(obstacleAngle * Mathf.Deg2Rad)*15, Mathf.Sin(obstacleAngle * Mathf.Deg2Rad) * 15);
        GameObject ob = GameObject.Instantiate(RandomSprite(currentArea)) as GameObject;
        ob.transform.position = new Vector2(-pos.x, pos.y);

    }


    private GameObject RandomSprite(int currentArea)
    {
        GameObject newSprite;
        if (Random.value > 0.9)
        {
            if (isMovingLeft)
            {
                newSprite = Resources.Load<GameObject>("Obstacles/wall1");
            }
            else
            {
                newSprite = Resources.Load<GameObject>("Obstacles/wall");
            }
        }
        else
        {

            string areaName = "";
            switch (currentArea)
            {
                case 0:
                    areaName = "forest";
                    break;
                case 1:
                    areaName = "desert";
                    break;
                case 2:
                    areaName = "mountain";
                    break;
                default:
                    areaName = "";
                    print("empty obstacle sprite");
                    return null;
            }
            GameObject[] newSpriteBig = Resources.LoadAll<GameObject>("Obstacles/" + areaName + "/big");
            GameObject[] newSpriteSmall = Resources.LoadAll<GameObject>("Obstacles/" + areaName + "/small");
            int randomSprite;
            if (Random.value > 0.8)
            {
                randomSprite = (int)Random.Range(0, newSpriteBig.Length);
                newSprite = newSpriteBig[randomSprite];
            }
            else
            {
                randomSprite = (int)Random.Range(0, newSpriteSmall.Length);
                newSprite = newSpriteSmall[randomSprite];
            }
        }

        return newSprite;
    }

    public void ChangedArea(int nextArea_)
    {
        nextArea = nextArea_;
    }

    public void ChangeDirection(bool isMovingLeftSide)
    {
        isMovingLeft = isMovingLeftSide;
    }

}
