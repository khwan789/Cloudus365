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

    public AudioClip collideSound;

    // Use this for initialization
    void Start()
    {
        timer = GameObject.Find("timer").GetComponent<Timer>().GetTime;
        ResetSpownTime();

        player = GameObject.FindGameObjectWithTag("Player");
        playerAngle = Vector2.Angle(new Vector2(0, 1), player.transform.position);
        angleDistance = 20;
        obstacleAngle = playerAngle + angleDistance;

        currentArea = 1;
        nextArea = currentArea;

        changedArea = false;
        placeWall = false;
        isMovingLeft = false;
        wallSpowningTime = 50;
    }

    // Update is called once per frame
    void Update()
    {
        timer = GameObject.Find("timer").GetComponent<Timer>().GetTime;
        player = GameObject.FindGameObjectWithTag("Player");
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
        //print("PlayerAngle " + playerAngle);
       // print("obstacleAngle " + obstacleAngle);

        //print("timer" + timer);
        if (timer >= nextSpownTime)
        {
            //print("Generated now " );

            // every few second
            GenerateObstcale();
            ResetSpownTime();
        }
        //print("Current type " + currentArea + " Next type: " + nextArea);

        if ((playerAngle >=175 && playerAngle <= 185)|| (playerAngle >= 345 && playerAngle <= 355)){
            // change current type
            //print("Current type " + currentArea + " Next type: " + nextArea);
            if(currentArea != nextArea) 
            {
                currentArea = nextArea;
            }
        }

        int timInt = (int)timer;

        if(timInt > wallSpowningTime)
        {
            if(timInt% wallSpowningTime == 0 && placeWall == false)
            {
                placeWall = true;
                //print("wall1");
            }
        }
        else if(timInt == wallSpowningTime && placeWall == false)
        {
            placeWall = true;
           // print("wall2");

        }
        //print("Timer in int " + (int)timer );


        
    }
    private void ResetSpownTime()
    {
        print("Timer: " + timer);
        if (timer > 1 && timer <= 30)
        {
            randomSpownTime = Random.Range(5, 7);
            nextSpownTime = timer + randomSpownTime;
            print("haha 1-50  Generated Time " + randomSpownTime);
        }
        else if (timer > 30 && timer <= 60)
        {
            randomSpownTime = Random.Range(3, 5);
            nextSpownTime = timer + randomSpownTime;
            print("50-100  Generated Time " + randomSpownTime);

        }
        else if (timer > 60 && timer <= 90)
        {
            randomSpownTime = Random.Range(2, 4);
            nextSpownTime = timer + randomSpownTime;
            print("100-150  Generated Time " + randomSpownTime);

        }
        else
        {
            randomSpownTime = Random.Range(2, 3);
            nextSpownTime = timer + randomSpownTime;
            print("150-     Generated Time " + randomSpownTime);


        }
    }

    private void GenerateObstcale()
    {
       // print("Generate at " + obstacleAngle);

        Vector2 pos = new Vector2(Mathf.Cos(obstacleAngle * Mathf.Deg2Rad)*15, Mathf.Sin(obstacleAngle * Mathf.Deg2Rad) * 15);
        GameObject ob = GameObject.Instantiate(RandomSprite(currentArea)) as GameObject;
        ob.transform.position = new Vector2(-pos.x, pos.y);
       // print("Position at " + ob.transform.position);

    }


    private GameObject RandomSprite(int currentArea)
    {
        GameObject newSprite;
        if (placeWall == true)
        {
            newSprite = Resources.Load<GameObject>("Obstacles/wall");

            placeWall = false;


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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.name == "Spaceman")
        {
            GetComponent<AudioSource>().PlayOneShot(collideSound);
        }
    }

}
