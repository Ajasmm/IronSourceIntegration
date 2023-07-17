using System;
using UnityEngine;

public class AdManager : MonoBehaviour
{
    bool showBannerAd = false;


    public static AdManager Instance { get { return GetInstance(); } }
    private static AdManager instance;
    private static AdManager GetInstance()
    {
        if (instance == null)
        {
            GameObject newObj = new GameObject("AdManager");
            newObj.AddComponent<AdManager>();
        }
        return instance;
    }


    private void OnEnable()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        IronSource.Agent.shouldTrackNetworkState(true);
        AddListeners();
    }
    private void AddListeners()
    {
        //Add AdInfo Rewarded Video Events
        IronSourceRewardedVideoEvents.onAdOpenedEvent += RewardedVideoOnAdOpenedEvent;
        IronSourceRewardedVideoEvents.onAdClosedEvent += RewardedVideoOnAdClosedEvent;
        IronSourceRewardedVideoEvents.onAdAvailableEvent += RewardedVideoOnAdAvailable;
        IronSourceRewardedVideoEvents.onAdUnavailableEvent += RewardedVideoOnAdUnavailable;
        IronSourceRewardedVideoEvents.onAdShowFailedEvent += RewardedVideoOnAdShowFailedEvent;
        IronSourceRewardedVideoEvents.onAdRewardedEvent += RewardedVideoOnAdRewardedEvent;
        IronSourceRewardedVideoEvents.onAdClickedEvent += RewardedVideoOnAdClickedEvent;

        //Add AdInfo Interstitial Events
        IronSourceInterstitialEvents.onAdReadyEvent += InterstitialOnAdReadyEvent;
        IronSourceInterstitialEvents.onAdLoadFailedEvent += InterstitialOnAdLoadFailed;
        IronSourceInterstitialEvents.onAdOpenedEvent += InterstitialOnAdOpenedEvent;
        IronSourceInterstitialEvents.onAdClickedEvent += InterstitialOnAdClickedEvent;
        IronSourceInterstitialEvents.onAdShowSucceededEvent += InterstitialOnAdShowSucceededEvent;
        IronSourceInterstitialEvents.onAdShowFailedEvent += InterstitialOnAdShowFailedEvent;
        IronSourceInterstitialEvents.onAdClosedEvent += InterstitialOnAdClosedEvent;

        //Add AdInfo Banner Events
        IronSourceBannerEvents.onAdLoadedEvent += BannerOnAdLoadedEvent;
        IronSourceBannerEvents.onAdLoadFailedEvent += BannerOnAdLoadFailedEvent;
        IronSourceBannerEvents.onAdClickedEvent += BannerOnAdClickedEvent;
        IronSourceBannerEvents.onAdScreenPresentedEvent += BannerOnAdScreenPresentedEvent;
        IronSourceBannerEvents.onAdScreenDismissedEvent += BannerOnAdScreenDismissedEvent;
        IronSourceBannerEvents.onAdLeftApplicationEvent += BannerOnAdLeftApplicationEvent;
    }


    #region Rewarded callbacks
    /************* RewardedVideo AdInfo Delegates *************/
    // Indicates that there’s an available ad.
    // The adInfo object includes information about the ad that was loaded successfully
    // This replaces the RewardedVideoAvailabilityChangedEvent(true) event
    void RewardedVideoOnAdAvailable(IronSourceAdInfo adInfo)
    {
        AdManagerRewardedAdEvents.onAdAvailableEvent?.Invoke();
    }
    // Indicates that no ads are available to be displayed
    // This replaces the RewardedVideoAvailabilityChangedEvent(false) event
    void RewardedVideoOnAdUnavailable()
    {
        AdManagerRewardedAdEvents.onAdUnavailableEvent?.Invoke();
    }
    // The Rewarded Video ad view has opened. Your activity will loose focus.
    void RewardedVideoOnAdOpenedEvent(IronSourceAdInfo adInfo)
    {
        AdManagerRewardedAdEvents.onAdOpenedEvent?.Invoke();
    }
    // The Rewarded Video ad view is about to be closed. Your activity will regain its focus.
    void RewardedVideoOnAdClosedEvent(IronSourceAdInfo adInfo)
    {
        IronSource.Agent.loadRewardedVideo();
        AdManagerRewardedAdEvents.onAdClosedEvent?.Invoke();
    }
    // The user completed to watch the video, and should be rewarded.
    // The placement parameter will include the reward data.
    // When using server-to-server callbacks, you may ignore this event and wait for the ironSource server callback.
    void RewardedVideoOnAdRewardedEvent(IronSourcePlacement placement, IronSourceAdInfo adInfo)
    {
        AdManagerRewardedAdEvents.onAdRewardedEvent?.Invoke();
    }
    // The rewarded video ad was failed to show.
    void RewardedVideoOnAdShowFailedEvent(IronSourceError error, IronSourceAdInfo adInfo)
    {
        AdManagerRewardedAdEvents.onAdShowFailedEvent?.Invoke();
    }
    // Invoked when the video ad was clicked.
    // This callback is not supported by all networks, and we recommend using it only if
    // it’s supported by all networks you included in your build.
    void RewardedVideoOnAdClickedEvent(IronSourcePlacement placement, IronSourceAdInfo adInfo)
    {

    }
    #endregion

    #region Interstitial callbacks
    /************* Interstitial AdInfo Delegates *************/
    // Invoked when the interstitial ad was loaded succesfully.
    void InterstitialOnAdReadyEvent(IronSourceAdInfo adInfo)
    {
    }
    // Invoked when the initialization process has failed.
    void InterstitialOnAdLoadFailed(IronSourceError ironSourceError)
    {
        IronSource.Agent.loadInterstitial();
    }
    // Invoked when the Interstitial Ad Unit has opened. This is the impression indication. 
    void InterstitialOnAdOpenedEvent(IronSourceAdInfo adInfo)
    {
    }
    // Invoked when end user clicked on the interstitial ad
    void InterstitialOnAdClickedEvent(IronSourceAdInfo adInfo)
    {
    }
    // Invoked when the ad failed to show.
    void InterstitialOnAdShowFailedEvent(IronSourceError ironSourceError, IronSourceAdInfo adInfo)
    {
    }
    // Invoked when the interstitial ad closed and the user went back to the application screen.
    void InterstitialOnAdClosedEvent(IronSourceAdInfo adInfo)
    {
        IronSource.Agent.loadInterstitial();
    }
    // Invoked before the interstitial ad was opened, and before the InterstitialOnAdOpenedEvent is reported.
    // This callback is not supported by all networks, and we recommend using it only if  
    // it's supported by all networks you included in your build. 
    void InterstitialOnAdShowSucceededEvent(IronSourceAdInfo adInfo)
    {
        Debug.LogWarning("Interstitial showd success fully");
        AdManagerRewardedAdEvents.onAdRewardedEvent?.Invoke();
    }
    #endregion

    #region Banner callbacks
    /************* Banner AdInfo Delegates *************/
    //Invoked once the banner has loaded
    void BannerOnAdLoadedEvent(IronSourceAdInfo adInfo)
    {
        if (showBannerAd)
            IronSource.Agent.displayBanner();
    }
    //Invoked when the banner loading process has failed.
    void BannerOnAdLoadFailedEvent(IronSourceError ironSourceError)
    {
        IronSource.Agent.loadBanner(IronSourceBannerSize.SMART, IronSourceBannerPosition.BOTTOM);
    }
    // Invoked when end user clicks on the banner ad
    void BannerOnAdClickedEvent(IronSourceAdInfo adInfo)
    {
    }
    //Notifies the presentation of a full screen content following user click
    void BannerOnAdScreenPresentedEvent(IronSourceAdInfo adInfo)
    {
    }
    //Notifies the presented screen has been dismissed
    void BannerOnAdScreenDismissedEvent(IronSourceAdInfo adInfo)
    {
    }
    //Invoked when the user leaves the app
    void BannerOnAdLeftApplicationEvent(IronSourceAdInfo adInfo)
    {
        IronSource.Agent.destroyBanner();
    }
    #endregion

    public void Initialize()
    {
        IronSource.Agent.loadRewardedVideo();
        IronSource.Agent.loadInterstitial();
    }
    public void OnApplicationPause(bool pause)
    {
        IronSource.Agent.onApplicationPause(pause);
    }


    public void ShowInterstitialAd()
    {
        if (IronSource.Agent.isInterstitialReady())
        {
            IronSource.Agent.showInterstitial();
        }
        else
        {
            IronSource.Agent.loadInterstitial();
        }
    }

    public void ShowBannerAd()
    {
        IronSource.Agent.loadBanner(IronSourceBannerSize.SMART, IronSourceBannerPosition.BOTTOM);
        showBannerAd = true;
    }
    public void DestroyBannerAd()
    {
        IronSource.Agent.destroyBanner();
        showBannerAd = false;
    }

    public void ShowRewardedAd()
    {
        if (IronSource.Agent.isRewardedVideoAvailable())
            IronSource.Agent.showRewardedVideo();
        else if (IronSource.Agent.isInterstitialReady())
            IronSource.Agent.showInterstitial();
        else
        {
            IronSource.Agent.loadRewardedVideo();
            IronSource.Agent.loadInterstitial();
        }
    }
    public bool IsRewardedAdAvailable()
    {
        return IronSource.Agent.isRewardedVideoAvailable() || IronSource.Agent.isInterstitialReady();
    }

}

public static class AdManagerRewardedAdEvents
{
    // Rewarded Video
    public static Action onAdShowFailedEvent;
    public static Action onAdOpenedEvent;
    public static Action onAdClosedEvent;
    public static Action onAdRewardedEvent;
    public static Action onAdAvailableEvent;
    public static Action onAdUnavailableEvent;
    public static Action onAdLoadFailedEvent;
    public static Action onAdReadyEvent;
} 
public static class AdManagerInterstitialAdEvents
{
    // Interstitial
    public static event Action onAdReadyEvent;
    public static event Action onAdLoadFailedEvent;
    public static event Action onAdOpenedEvent;
    public static event Action onAdClosedEvent;
    public static event Action onAdShowSucceededEvent;
    public static event Action onAdShowFailedEvent;
    public static event Action onAdClickedEvent;
}
public static class AdManagerBannerAdEvents
{
    public static event Action onAdLoadedEvent;
    public static event Action onAdLeftApplicationEvent;
    public static event Action onAdScreenDismissedEvent;
    public static event Action onAdScreenPresentedEvent;
    public static event Action onAdClickedEvent;
    public static event Action onAdLoadFailedEvent;
}