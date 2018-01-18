using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class menuSound : MonoBehaviour {

    public AudioClip buttonSound;

    public Button start;
    public Button instruction;
    public Button exit;


    // Use this for initialization
    void Start () {

        Button startB = start.GetComponent<Button>();
        startB.onClick.AddListener(buttonClickSound);

        Button instB = instruction.GetComponent<Button>();
        instB.onClick.AddListener(buttonClickSound);

        Button exitB = exit.GetComponent<Button>();
        exitB.onClick.AddListener(buttonClickSound);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void buttonClickSound()
    {
        GetComponent<AudioSource>().PlayOneShot(buttonSound);
    }
}
