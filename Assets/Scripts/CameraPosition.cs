using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPosition : MonoBehaviour {

    GameObject player;
    TouchInput playerInput;

    GameObject planet;
    PlanetGravity planetScript;

    bool freezeY;

    // Use this for initialization
    void Start () {

        player = GameObject.Find("Spaceman");

        playerInput = player.GetComponent<TouchInput>();

        planet = GameObject.Find("Cloudus 456");
        planetScript = planet.GetComponent<PlanetGravity>();
    }
	
	// Update is called once per frame
	void Update () {
        // Calculates direction the character's downward force
        this.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -4);
        this.transform.rotation = new Quaternion(player.transform.rotation.x, player.transform.rotation.y, 0, this.transform.rotation.w);
    }
}
