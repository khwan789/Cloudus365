using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class menuPopup : MonoBehaviour
{

    public AudioClip buttonSound;

    public Button start;
    public Button credits;
    public Button creditClose;

    public GameObject creditPopup;


    

    // Use this for initialization
    void Start()
    {
        creditPopup.SetActive(false);

        Button startB = start.GetComponent<Button>();
        startB.onClick.AddListener(ButtonClickSound);
        startB.onClick.AddListener(StartGame);

        Button creditsB = credits.GetComponent<Button>();
        creditsB.onClick.AddListener(ShowCredit);

        Button closeB = creditClose.GetComponent<Button>();
        closeB.onClick.AddListener(ShowCredit);

        

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

       
    }

    void ButtonClickSound()
    {
        GetComponent<AudioSource>().PlayOneShot(buttonSound);
    }

    void StartGame()
    {
        SceneManager.LoadScene("Stage 1");
    }

    void ShowCredit()
    {
        if (creditPopup.activeSelf == false)
        {
            creditPopup.SetActive(true);
        }
        else
        {
            creditPopup.SetActive(false);
        }
    }

   public void ShowLeaderboard()
    {
        MainMenuEvents.ShowLeaderboardUI();
    }

   
}
