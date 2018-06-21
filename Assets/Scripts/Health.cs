using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Health : MonoBehaviour {

    int healthVar;
    int maxHealth = 3;
    float healthbarMaxSize;

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
        if (isInvul)
        {
            Invoke("Bringback", 1.5f);
        }
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "smallOb" || collision.gameObject.tag == "bigOb" || collision.gameObject.tag == "bg1")
        {
            if (healthVar > 1 && isInvul == false)
            {
                // Change Health Var
                isInvul = true;
                // Change alpha of sprite
                Color alphaChange = this.GetComponent<SpriteRenderer>().color;
                alphaChange.a = .5f;
                this.GetComponent<SpriteRenderer>().color = alphaChange;
                GetComponent<AudioSource>().PlayOneShot(hurtSound, 1);
                healthVar--;
                ChangedHealth();
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
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "smallOb" || collision.gameObject.tag == "bigOb" || collision.gameObject.tag == "bg1")
        {
            isInvul = false;
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
    public bool Invul
    {
        get
        {
            return isInvul;
        }
        set
        {
            isInvul = value;
        }
    }

    void Bringback()
    {
        // Change alpha of sprite
        Color alphaChange = this.GetComponent<SpriteRenderer>().color;
        alphaChange.a = 1f;
        this.GetComponent<SpriteRenderer>().color = alphaChange;
    }
}
