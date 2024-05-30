using GoogleMobileAds.Api;
using UnityEngine;

public class AD_Manager : MonoBehaviour
{
    public static AD_Manager instance; 

    public string Android_AppID;
    public string Android_Banner;
    public string Android_Intres;
    public string Android_VideoReward;


    public string IOS_AppID;
    public string IOS_Banner;
    public string IOS_Intres;
    public string IOS_VideoReward;

  private BannerView _bannerView;
    private InterstitialAd _interstitialAd;
    private RewardedAd _rewardedAd;

    string _adUnitId_banne, _adUnitId_Intres_, _adUnitId_video;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            DestroyImmediate(gameObject);
        }
    }
    void Start()
    {
         MobileAds.Initialize(initStatus => { });
        _adUnitId_banne = Android_Banner;
        _adUnitId_Intres_ = Android_Intres;
        _adUnitId_video = Android_VideoReward;

        #if UNITY_IOS
        _adUnitId_banne=IOS_Banner;
        _adUnitId_Intres_=IOS_Intres;
        _adUnitId_video=IOS_VideoReward;
        #endif



      //  LoadBannerAd();
        LoadInterstitialAd();
        LoadRewardedAd();

    }

    public void LoadBannerAd()
    {
        // create an instance of a banner view first.
        if (_bannerView == null)
        {
            CreateBannerView();
        }

        // create our request used to load the ad.
        var adRequest = new AdRequest();

        // send the request to load the ad.
       // Debug.Log("Loading banner ad.");
        _bannerView.LoadAd(adRequest);
    }

    public void CreateBannerView()
    {
      //  Debug.Log("Creating banner view");

        // If we already have a banner, destroy the old one.
        if (_bannerView != null)
        {
            DestroyBannerView();
        }

        // Create a 320x50 banner at top of the screen
        _bannerView = new BannerView(_adUnitId_banne, AdSize.Banner, AdPosition.Bottom);
        _bannerView.Show();
    }


    public void DestroyBannerView()
    {
        if (_bannerView != null)
        {
       //     Debug.Log("Destroying banner view.");
            _bannerView.Destroy();
            _bannerView = null;
        }
    }


    public void LoadInterstitialAd()
    {

        // Clean up the old ad before loading a new one.
       if (_interstitialAd != null)
        {
            _interstitialAd.Destroy();
            _interstitialAd = null;
        }

        //Debug.Log("Loading the interstitial ad.");

        // create our request used to load the ad.
        var adRequest = new AdRequest();

        // send the request to load the ad.
        InterstitialAd.Load(_adUnitId_Intres_, adRequest,
            (InterstitialAd ad, LoadAdError error) =>
            {
              // if error is not null, the load request failed.
              if (error != null || ad == null)
                {
                   // Debug.LogError("interstitial ad failed to load an ad " +
                                  // "with error : " + error);
                    return;
                }

                //Debug.Log("Interstitial ad loaded with response : "
                       //   + ad.GetResponseInfo());
        
                _interstitialAd = ad;
                RegisterEventHandlers(ad);
            });
       
    }

    private void RegisterEventHandlers(InterstitialAd interstitialAd)
    {
        // Raised when the ad is estimated to have earned money.
        interstitialAd.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log(string.Format("Interstitial ad paid {0} {1}.",
                adValue.Value,
                adValue.CurrencyCode));
        };
        // Raised when an impression is recorded for an ad.
        interstitialAd.OnAdImpressionRecorded += () =>
        {
            Debug.Log("Interstitial ad recorded an impression.");
        };
        // Raised when a click is recorded for an ad.
        interstitialAd.OnAdClicked += () =>
        {
            Debug.Log("Interstitial ad was clicked.");
        };
        // Raised when an ad opened full screen content.
        interstitialAd.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Interstitial ad full screen content opened.");
        };
        // Raised when the ad closed full screen content.
        interstitialAd.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Interstitial ad full screen content closed.");
        };
        // Raised when the ad failed to open full screen content.
        interstitialAd.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Interstitial ad failed to open full screen content " +
                           "with error : " + error);
        };
    }

    public void ShowInterstitialAd()
    {
        if (_interstitialAd != null)
        {
           // Debug.Log("Showing interstitial ad.");
            _interstitialAd.Show();
        }
        else
        {
           // Debug.LogError("Interstitial ad is not ready yet.");
            LoadInterstitialAd();
        }

        
    }



    public void LoadRewardedAd()
    {
        // Clean up the old ad before loading a new one.
       if (_rewardedAd != null)
        {
            _rewardedAd.Destroy();
            _rewardedAd = null;
        }

       // Debug.Log("Loading the rewarded ad.");

        // create our request used to load the ad.
        var adRequest = new AdRequest();

        // send the request to load the ad.
        RewardedAd.Load(_adUnitId_video, adRequest,
            (RewardedAd ad, LoadAdError error) =>
            {
              // if error is not null, the load request failed.
              if (error != null || ad == null)
                {
                    //Debug.LogError("Rewarded ad failed to load an ad " +  "with error : " + error);
                    return;
                }

               // Debug.Log("Rewarded ad loaded with response : "
                         // + ad.GetResponseInfo());

                _rewardedAd = ad;
                RegisterEventHandlers(ad);
            });
     
    }


    public void ShowRewardedAd()
    {
        const string rewardMsg =
            "Rewarded ad rewarded the user. Type: {0}, amount: {1}.";

       if (_rewardedAd != null)
        {
            _rewardedAd.Show((Reward reward) =>
            {
                // TODO: Reward the user.
               // Debug.Log(string.Format(rewardMsg, reward.Type, reward.Amount));
                GameManager.Instance.OnRewarded();
            });
        }
        else
        {
            LoadRewardedAd();
        }
       
    }

    private void RegisterEventHandlers(RewardedAd ad)
    {
        // Raised when the ad is estimated to have earned money.
        ad.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log(string.Format("Rewarded ad paid {0} {1}.",
                adValue.Value,
                adValue.CurrencyCode));
        };
        // Raised when an impression is recorded for an ad.
        ad.OnAdImpressionRecorded += () =>
        {
            Debug.Log("Rewarded ad recorded an impression.");
        };
        // Raised when a click is recorded for an ad.
        ad.OnAdClicked += () =>
        {
            Debug.Log("Rewarded ad was clicked.");
        };
        // Raised when an ad opened full screen content.
        ad.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Rewarded ad full screen content opened.");
        };
        // Raised when the ad closed full screen content.
        ad.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Rewarded ad full screen content closed.");
            LoadRewardedAd();
        };
        // Raised when the ad failed to open full screen content.
        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Rewarded ad failed to open full screen content " +
                           "with error : " + error);
        };
    }
}
