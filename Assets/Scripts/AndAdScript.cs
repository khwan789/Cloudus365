using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;

public class AndAdScript : MonoBehaviour
{
    private bool playedOnce;
    public static int nums = 1;

    // Use this for initialization
    void Start()
    {
        
        string ID = "ca-app-pub-3614489525260801~6559714706";

#if UNITY_ANDROID
        string appId = ID;
#else
            string appId = "unexpected_platform";
#endif

        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize(appId);
        
        if(nums == 1)
        {
            RequestInterstitial();
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (Popup.dead == true)
        {
            if (!playedOnce && nums == 0)
            {
                playedOnce = true;
                nums = 2;
                ShowAd();
            }
        }
    }

    public void ShowAd()
    {
        if (interstitial.IsLoaded())
        {
            interstitial.Show();
            Debug.Log("showad");
        }
    }

    InterstitialAd interstitial;
    public void RequestInterstitial() // public으로 고쳐서 앱실행시1회 호출...(전면 초기화 및 1회 부름)
    {
        string adID = "ca-app-pub-3614489525260801/1115816333";

#if UNITY_ANDROID
        string adUnitId = adID;
#else
                string adUnitId = "unexpected_platform";
#endif

        interstitial = new InterstitialAd(adUnitId);

        AdRequest request = new AdRequest.Builder().Build();


        //production
        //AdRequest request = new AdRequest.Builder().Build();

        if (interstitial == null)
        {
            interstitial = new InterstitialAd(adUnitId);
            request = new AdRequest.Builder().Build();
        }

        interstitial.OnAdLoaded += HandleOnAdLoaded;
        interstitial.OnAdFailedToLoad += HandleOnAdFailedToLoad;

        // Load the interstitial with the request.

        //interstitial.LoadAd(request); 
        interstitial.LoadAd(request);
    }

    public void HandleOnAdLoaded(object sender, System.EventArgs args) //광고 성공시 불림
    {
        Debug.Log("success load");
    }
    public void HandleOnAdFailedToLoad(object sender, System.EventArgs args) //광고실패시불림
    {
        Debug.Log("failed to load");
    }
}
