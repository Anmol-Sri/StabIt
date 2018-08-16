using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;

public class Adscript : MonoBehaviour
{
     private BannerView bannerView;
    InterstitialAd interstitial;
    GameManager game;
    private RewardBasedVideoAd rewardBasedVideo;

    GameManager obj;


    // Use this for initialization
    void Start()
    {
        game = GameManager.Instance;
#if UNITY_ANDROID
        string appId = "ca-app-pub-3698875051105618~6819407412";
#elif UNITY_IPHONE
            string appId = "ca-app-pub-3940256099942544~1458002511";
#else
            string appId = "unexpected_platform";
#endif

        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize(appId);


        obj = new GameManager();
        this.RequestBanner();
        this.RequestInterstitial();
        this.rewardBasedVideo = RewardBasedVideoAd.Instance;

        this.RequestRewardBasedVideo();

        

    }

   

    private void Update()
    {
        if (obj.isPaused == true)
        {
            interstitial.Show();
            rewardBasedVideo.Show();

            if (interstitial.IsLoaded())
            {
                interstitial.Show();
            }


            if (rewardBasedVideo.IsLoaded())
            {
                rewardBasedVideo.Show();
            }
        }

        if (game.lifepoint == 0)
        {
            interstitial.Show();
            rewardBasedVideo.Show();

            if (interstitial.IsLoaded())
            {
                interstitial.Show();
            }


            if (rewardBasedVideo.IsLoaded())
            {
                rewardBasedVideo.Show();
            }
        }
    }


    void OnApplicationQuit()
    {
        if (interstitial.IsLoaded())
        {
            interstitial.Show();
        }
        if (rewardBasedVideo.IsLoaded())
        {
            rewardBasedVideo.Show();
        }

    }

    private void GameOver()
    {

        if (interstitial.IsLoaded())
        {
            interstitial.Show();
        }

        if (rewardBasedVideo.IsLoaded())
        {
            rewardBasedVideo.Show();
        }
    }



    private void RequestBanner()
    {
        #if UNITY_ANDROID
            string adUnitId = "ca-app-pub-3698875051105618/2304597273";
        #elif UNITY_IPHONE
            string adUnitId = "ca-app-pub-3940256099942544/2934735716";
        #else
            string adUnitId = "unexpected_platform";
        #endif

        // Create a 320x50 banner at the top of the screen.
        bannerView = new BannerView(adUnitId, AdSize.Banner, AdPosition.Bottom);

        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();

        // Load the banner with the request.
        bannerView.LoadAd(request);
    }

    private void RequestInterstitial()
    {
        #if UNITY_ANDROID
        string adUnitId = "ca-app-pub-3698875051105618/2955758990";
        #elif UNITY_IPHONE
        string adUnitId = "ca-app-pub-3940256099942544/4411468910";
        #else
        string adUnitId = "unexpected_platform";
        #endif

        

        // Initialize an InterstitialAd.
        interstitial = new InterstitialAd(adUnitId);

        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the interstitial with the request.
        interstitial.LoadAd(request);

        
    }

    private void RequestRewardBasedVideo()
    {
#if UNITY_ANDROID
        string adUnitId = "ca-app-pub-3698875051105618/6998536666";
#elif UNITY_IPHONE
            string adUnitId = "ca-app-pub-3940256099942544/1712485313";
#else
            string adUnitId = "unexpected_platform";
#endif

        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the rewarded video ad with the request.
        this.rewardBasedVideo.LoadAd(request, adUnitId);
    }

    



}
