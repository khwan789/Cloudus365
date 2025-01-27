using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;
using static UnityEngine.GraphicsBuffer;

public class GameManager : MonoBehaviour
{
    public bool isPlaying = false;

    public ObjectPlayer player; // Reference to the player
    public CameraPosition cameraController; // Reference to the camera controller

    public GameObject inGameUI;
    public UnityEngine.UI.Text currentScore_Text_inGame;
    public UnityEngine.UI.Text totalGold_Text_inGame;

    private float scoreIncreaseThreshold = 5000f; // Threshold for speed increase
    private float currentScore = 0f;

    [HideInInspector]
    public Animator playerAnimator;
    [HideInInspector]
    public string playerRunAnimName;

    public GameObject outGameUI;
    public GameObject gamgOver_Popup;
    public float scoreIncrease = 100;
    private float highScore;
    public float totalGold;

    private static GameManager instance;

    private void Awake()
    {
        totalGold = GetTotalGold();

        // Ensure only one instance of GameManager exists
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Make GameManager persist across scenes
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate instances
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(totalGold);
        Debug.Log(GetHighScore());
        StartCoroutine(IncreaseGold(0));
    }

    // Update is called once per frame
    void Update()
    {
    
    }

    public void StartGame()
    {
        isPlaying = true;
        Time.timeScale = 1;
        outGameUI.SetActive(false);
        inGameUI.SetActive(true);
        StartCoroutine(IncreaseScoreTime());
        StartCoroutine(PlayStartAnimation(playerAnimator,playerRunAnimName));
    }

    public void GameOver()
    {
        isPlaying = false;
        Time.timeScale = 0;
        SaveHighScore(currentScore);
        SaveTotalGold(totalGold);
        SetGameOverPopup();
    }

    public IEnumerator PlayStartAnimation(Animator animator, string animName)
    {
        animator.SetBool(animName, true);
        yield return null;
    }

    private bool isUpdatingScore = false; // Flag to prevent simultaneous updates

    public IEnumerator IncreaseScoreItem(float score)
    {
        while (isUpdatingScore) yield return null; // Wait until the other coroutine finishes

        isUpdatingScore = true; // Lock the score update

        float duration = 0.5f; // Duration for the counting animation
        float targetScore = currentScore + score;
        float offset = (targetScore - currentScore) / duration;

        while (currentScore < targetScore)
        {
            currentScore += offset * Time.deltaTime;
            currentScore_Text_inGame.text = ((int)currentScore).ToString();
            yield return null;
        }

        currentScore = targetScore;
        currentScore_Text_inGame.text = ((int)currentScore).ToString();

        isUpdatingScore = false; // Unlock the score update
    }

    public IEnumerator IncreaseScoreTime()
    {
        while (isPlaying)
        {
            yield return new WaitForSeconds(3);

            while (isUpdatingScore) yield return null; // Wait until the other coroutine finishes

            isUpdatingScore = true; // Lock the score update

            float duration = 0.5f; // Duration for the counting animation
            float targetScore = currentScore + scoreIncrease;
            float offset = (targetScore - currentScore) / duration;

            while (currentScore < targetScore)
            {
                currentScore += offset * Time.deltaTime;
                currentScore_Text_inGame.text = ((int)currentScore).ToString();
                yield return null;
            }

            currentScore = targetScore;
            currentScore_Text_inGame.text = ((int)currentScore).ToString();

            isUpdatingScore = false; // Unlock the score update
        }
    }

    private readonly string highScoreKey = "HighScore"; // Key for storing the high score
    // Method to save a new high score
    private void SaveHighScore(float score)
    {
        float currentHighScore = GetHighScore();
        if (score > currentHighScore)
        {
            PlayerPrefs.SetFloat(highScoreKey, score);
            PlayerPrefs.Save(); // Ensure the data is saved to storage
            Debug.Log("New High Score Saved: " + score);
        }
    }
    // Method to get the current high score
    public float GetHighScore()
    {
        // Retrieve the high score, defaulting to 0 if no value exists
        return PlayerPrefs.GetFloat(highScoreKey, 0);
    }

    public IEnumerator IncreaseGold(float gold)
    {
        float duration = 0.5f; // 카운팅에 걸리는 시간 설정. 
        float targetGold = totalGold + gold;
        float offset = (targetGold - totalGold) / duration;

        while (totalGold < targetGold)
        {
            totalGold += offset * Time.deltaTime;
            totalGold_Text_inGame.text = ((int)totalGold).ToString();
            yield return null;
        }
        totalGold = targetGold;
        totalGold_Text_inGame.text = ((int)totalGold).ToString();
    }

    private readonly string totalGoldKey = "TotalGold"; // Key for storing the high score
    // Method to save a new high score
    private void SaveTotalGold(float gold)
    {
        float currentTotalGold = GetTotalGold();
        if (gold > currentTotalGold)
        {
            PlayerPrefs.SetFloat(totalGoldKey, gold);
            PlayerPrefs.Save(); // Ensure the data is saved to storage
            Debug.Log("New Total Gold Saved: " + gold);
        }
    }

    // Method to get the current high score
    public float GetTotalGold()
    {
        // Retrieve the high score, defaulting to 0 if no value exists
        return PlayerPrefs.GetFloat(totalGoldKey, 0);
    }

    public UnityEngine.UI.Text score_GameOver;
    public UnityEngine.UI.Text score_Highest_GaveOver;
    private void SetGameOverPopup()
    {
        score_GameOver.text = ((int)currentScore).ToString();
        score_Highest_GaveOver.text = ((int)GetHighScore()).ToString();
        gamgOver_Popup.SetActive(true);
    }

    public void ToMainMenu()
    {
        SceneManager.LoadScene("Main");
    }

    public void ReplayGame()
    {
        // Subscribe to the sceneLoaded event
        SceneManager.sceneLoaded += OnSceneLoaded;

        // Load the scene
        SceneManager.LoadScene("Main");
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Check if the loaded scene is the correct one
        if (scene.name == "Main")
        {
            StartGame(); // Call StartGame once the scene is loaded
        }

        // Unsubscribe from the event to prevent duplicate calls
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
