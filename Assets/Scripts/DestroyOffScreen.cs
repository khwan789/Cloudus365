using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOffScreen : MonoBehaviour {

    GameObject player;
	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player");

    }
	
	// Update is called once per frame
	void Update () {
        float distance = Vector2.Distance(player.transform.position, this.gameObject.transform.position);
        
		if(distance >=7)
        {
            Destroy(this.gameObject);

        }
    }
}
