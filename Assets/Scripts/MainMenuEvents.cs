using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;

public class MainMenuEvents : MonoBehaviour {
    
    // Use this for initialization
    void Start () {
        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().Build();
        PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.Activate();

        SignIn();
    }

    void SignIn()
    {
        Social.localUser.Authenticate(success => { });
    }

    #region Leaderboards
    public static void AddScoreToLeaderboard(string leaderboardId, int score)
    {
        Social.ReportScore(score, leaderboardId, success => { });
    }

    public static void ShowLeaderboardUI()
    {
        Social.ShowLeaderboardUI();
    }
    #endregion /Leaderboards
   
}
