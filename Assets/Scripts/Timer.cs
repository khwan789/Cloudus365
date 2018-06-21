using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour {

    Text timer;
    public float time = 0;
  //  GameObject player;

    public float GetTime
    {
        get
        {
            return time;
        }
        set
        {
            time = value;
        }
    }
    
    // Use this for initialization
    void Start () {
        //player = GameObject.Find("Spaceman");

        timer = gameObject.GetComponent<Text>();
        timer.text = "" + time;
	}
	
	// Update is called once per frame
	void Update () {

                time += Time.deltaTime;
        timer.text = "" + (int)time;

       // this.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -4);
        //this.transform.rotation = new Quaternion(player.transform.rotation.x, player.transform.rotation.y, 0, this.transform.rotation.w);
    }
}
