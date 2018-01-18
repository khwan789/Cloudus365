using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Health : MonoBehaviour {

    int healthVar;
    int maxHealth = 3;
    float healthbarMaxSize;
    bool isTransperant = false;

    float waitTime = 0.0f;

    bool isInvul = false;

    public bool isGameOver = false;

    public GameObject healthBar;

    public AudioClip hurtSound;

    // Use this for initialization
    void Start () {
        healthVar = maxHealth;

        healthbarMaxSize = healthBar.transform.localScale.x;
    }

    // Update is called once per frame
    void Update () {
        if (isTransperant)
        {
            if (Time.time > waitTime)
            {
                // Change alpha of sprite
                Color alphaChange = this.GetComponent<SpriteRenderer>().color;

                alphaChange.a = 1f;

                this.GetComponent<SpriteRenderer>().color = alphaChange;

                
                isInvul = false;
            }
        }
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "smallOb" || collision.gameObject.tag == "bigOb" || collision.gameObject.tag == "bg1")
        {
            if (healthVar > 1 && isInvul == false)
            {
                // Change Health Var
                healthVar--;
                ChangedHealth();
                // Change alpha of sprite
                Color alphaChange = this.GetComponent<SpriteRenderer>().color;

                alphaChange.a = .5f;

                this.GetComponent<SpriteRenderer>().color = alphaChange;

                isTransperant = true;

                isInvul = true;
                waitTime = Time.time + 1.5f;
                GetComponent<AudioSource>().PlayOneShot(hurtSound);
            }
            else
            {
                // Death Screen
                healthVar--;
                ChangedHealth();
                isGameOver = true;
            }
        }

    }
   
    void ChangedHealth()
    {

        Vector2 newScale = new Vector2((healthbarMaxSize/maxHealth)* healthVar, healthBar.transform.localScale.y);

        healthBar.transform.localScale = newScale;
    }
    public bool Over
    {
        get
        {
            return isGameOver;
        }
        set
        {
            isGameOver = value;
        }
    }
}
