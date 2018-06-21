using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bgGenerater : MonoBehaviour {
    public int type; // 0 == top  1 == down
    private float randomAngle;
    private float currentAngle;
    private float finalAngle;
    private int randomArea;
    private int layerNum;

    private int nextArea;

    public int NextArea()
    {
        return nextArea;
    }
    // Use this for initialization
    void Start () {

    }

    // Update is called once per frame
    void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            // set all random value when collide
            randomAngle = Random.Range(4, 20);
            randomArea = (int)Random.Range(0, 3);
            currentAngle = 0;
            finalAngle = 0;
            layerNum = 0;

            switch (type)
            {
                case 0:
                    currentAngle = 180;
                    finalAngle = 360;
                    break;
                case 1:
                    currentAngle = 0;
                    finalAngle = 180;
                    break;
                default:
                    break;
            }


            // Remove oldBackground
            RemoveOldBG();
            // GenerateBackground
            GenerateBackground();

        }
    }

    private void GenerateBackground()
    {
        nextArea = randomArea;
        GameObject.Find("Cloudus 456").GetComponent<obstacleGenerator>().ChangedArea(nextArea);
        while (currentAngle < finalAngle)
        {
            float placeRandom = 0;
            if (nextArea >= 14)
            {
                placeRandom = 0.7f;
            }
            else if (nextArea >= 8)
            {
                placeRandom = 0.3f;
            }else
            {
                placeRandom = 0.2f;
            }
            if (Random.value > placeRandom)   // random place or not
            {
                //x = cos(a) * r;
                //y = sin(a) * r;
                Vector2 position = new Vector2(Mathf.Cos(currentAngle * Mathf.Deg2Rad) * 15, Mathf.Sin(currentAngle * Mathf.Deg2Rad) * 15);
                GameObject bg = GameObject.Instantiate(RandomSprite(randomArea)) as GameObject;
                //Sprite bgImg = RandomSprite(randomArea);
                bg.tag = "bg" + type;

                bg.transform.RotateAround(bg.transform.position, new Vector3(0,0,1), currentAngle);
                bg.transform.Rotate(new Vector3(0, 0, -90 ));
                bg.transform.position = new Vector2(position.x, position.y);
                bg.transform.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = layerNum%2;
                layerNum++;

            }

            currentAngle += randomAngle;
        }
    }
    private void RemoveOldBG()
    {
        GameObject[] lastbgs = GameObject.FindGameObjectsWithTag("bg" + type);
        for(int i = 0; i < lastbgs.Length; i++)
        {
            Destroy(lastbgs[i]);
        }
    }

    private GameObject RandomSprite(int currentArea)
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
                return null;
        }
        GameObject[] newSprite = Resources.LoadAll<GameObject>("Background/"+areaName);
        int randomSprite = (int)Random.Range(0, newSprite.Length);

        return newSprite[randomSprite];
    }
    

}
