using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallDetection : MonoBehaviour {

    Collider2D objDetected;

    GameObject player;

    float waitTime = 0.0f;

    TouchInput playerScript;

    // Use this for initialization
    void Start () {
        player = GameObject.Find("Spaceman");
	}
	
	// Update is called once per frame
	void Update () {

        playerScript = player.GetComponent<TouchInput>();

        objDetected = Physics2D.OverlapBox(transform.position, new Vector2(GetComponent<Collider2D>().bounds.size.x + 2, GetComponent<Collider2D>().bounds.size.y + 2), transform.eulerAngles.z);

        if(objDetected.gameObject.name == "Spaceman")
        {

            if (Time.time > waitTime)
            {
                if (playerScript.IsInPerimeter == false)
                {
                    playerScript.IsInPerimeter = true;

                    waitTime = Time.time + 4.2f;
                }

            }

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        playerScript.IsInPerimeter = false;
    }
}
