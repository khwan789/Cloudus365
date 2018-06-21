using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallDetection : MonoBehaviour
{

    Collider2D objDetected;

    GameObject player;

    float waitTime = 0.0f;

    TouchInput playerScript;
    Health pHealth;
    // Use this for initialization
    void Start()
    {
        player = GameObject.Find("Spaceman");
        playerScript = player.GetComponent<TouchInput>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player"){
            playerScript.IsInPerimeter = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player") {
            playerScript.IsInPerimeter = false;
        }
    }
}
