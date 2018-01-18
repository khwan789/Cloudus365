using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetGravity : MonoBehaviour {

    float gravity = 8;

    GameObject player;
    TouchInput playerInput;

    // Use this for initialization
    void Start () {

        player = GameObject.Find("Spaceman");
        playerInput = player.GetComponent<TouchInput>();
    }
	
	// Update is called once per frame
	void Update () {
        
	}

    void FixedUpdate()
    {
        AttractObjects();
    }

    void AttractObjects()
    {
        // Stores every object in an 100 point radius around the sphere
        Collider2D[] allObjects = Physics2D.OverlapCircleAll(this.transform.position, 100f);


        // Loops through all the objects and rotates all of them to their down force.
        for(int i = 0; i < allObjects.Length; i++)
        {
            // Calculates direction of the force
            Vector3 gravityDir = transform.position - allObjects[i].transform.position;

            
            Vector3 gravityForce = gravityDir.normalized * gravity;

            // If the object has a rigid body then apply force downward (So just the player)
            if (allObjects[i].GetComponent<Rigidbody2D>() != null)
            {
                 allObjects[i].GetComponent<Rigidbody2D>().AddForce(gravityForce );
            }

            // Keeps the objects aligned to the down force
            allObjects[i].transform.rotation = Quaternion.FromToRotation(-allObjects[i].transform.up, gravityDir) * allObjects[i].transform.rotation;
        }


    }

}
