using System;
using UnityEngine;

public class AdManager : MonoBehaviour
{
    #region Singlton
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
    #endregion

    private bool showBannerAd = false; // To show the banner ad immediatly after loaded
    private event Action<RewardedAdResult> rewardedAdCallbacks; // To keep track of delta callbacks in show rewarded Ad

    #region Callback events
    // Rewarded Ad callbacks
    public static Action onRewardedAdAvailableEvent;
    public static Action onRewardedAdUnavailableEvent;
    public static Action onRewardedAdOpenedEvent;
    public static Action onRewardedAdClosedEvent;
    public static Action onRewardedAdRewardedEvent; // Called in case of rewarded Ad and successfull interstitial Ad for the use of rewarded interstitial
    public static Action onRewardedAdShowFailedEvent;
    public static Action onRewardedAdClickedEvent;

    // Interstitial Ad callbacks
    public static Action onInterstitialAdReadyEvent;
    public static Action onInterstitialAdLoadFailedEvent;
    public static Action onInterstitialAdOpenedEvent;
    public static Action onInterstitialAdClickedEvent;
    public static Action onInterstitialAdShowSucceededEvent;
    public static Action onInterstitialAdShowFailedEvent;
    public static Action onInterstitialAdClosedEvent;

    // Banner Ad callbacks
    public static Action onBannerAdLoadedEvent;
    public static Action onBannerAdLeftApplicationEvent;
    public static Action onBannerAdScreenDismissedEvent;
    public static Action onBannerAdScreenPresentedEvent;
    public static Action onBannerAdClickedEvent;
    public static Action onBannerAdLoadFailedEvent;
    #endregion

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
    private void OnDisable()
    {
        RemoveListeners();
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
    private void RemoveListeners()
    {
        //Add AdInfo Rewarded Video Events
        IronSourceRewardedVideoEvents.onAdOpenedEvent -= RewardedVideoOnAdOpenedEvent;
        IronSourceRewardedVideoEvents.onAdClosedEvent -= RewardedVideoOnAdClosedEvent;
        IronSourceRewardedVideoEvents.onAdAvailableEvent -= RewardedVideoOnAdAvailable;
        IronSourceRewardedVideoEvents.onAdUnavailableEvent -= RewardedVideoOnAdUnavailable;
        IronSourceRewardedVideoEvents.onAdShowFailedEvent -= RewardedVideoOnAdShowFailedEvent;
        IronSourceRewardedVideoEvents.onAdRewardedEvent -= RewardedVideoOnAdRewardedEvent;
        IronSourceRewardedVideoEvents.onAdClickedEvent -= RewardedVideoOnAdClickedEvent;

        //Add AdInfo Interstitial Events
        IronSourceInterstitialEvents.onAdReadyEvent -= InterstitialOnAdReadyEvent;
        IronSourceInterstitialEvents.onAdLoadFailedEvent -= InterstitialOnAdLoadFailed;
        IronSourceInterstitialEvents.onAdOpenedEvent -= InterstitialOnAdOpenedEvent;
        IronSourceInterstitialEvents.onAdClickedEvent -= InterstitialOnAdClickedEvent;
        IronSourceInterstitialEvents.onAdShowSucceededEvent -= InterstitialOnAdShowSucceededEvent;
        IronSourceInterstitialEvents.onAdShowFailedEvent -= InterstitialOnAdShowFailedEvent;
        IronSourceInterstitialEvents.onAdClosedEvent -= InterstitialOnAdClosedEvent;

        //Add AdInfo Banner Events
        IronSourceBannerEvents.onAdLoadedEvent -= BannerOnAdLoadedEvent;
        IronSourceBannerEvents.onAdLoadFailedEvent -= BannerOnAdLoadFailedEvent;
        IronSourceBannerEvents.onAdClickedEvent -= BannerOnAdClickedEvent;
        IronSourceBannerEvents.onAdScreenPresentedEvent -= BannerOnAdScreenPresentedEvent;
        IronSourceBannerEvents.onAdScreenDismissedEvent -= BannerOnAdScreenDismissedEvent;
        IronSourceBannerEvents.onAdLeftApplicationEvent -= BannerOnAdLeftApplicationEvent;
    }


    #region Public Methodes
    public void Initialize()
    {
        // everything elses is automaticaly initialized by ironsource sdk
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
    public void ShowBannerAd(BannerAdPosition bannerAdPosition)
    {
        IronSourceBannerPosition bannerPos;
        switch (bannerAdPosition)
        {
            case BannerAdPosition.TOP:
                bannerPos = IronSourceBannerPosition.TOP;
                break;
            default:
                bannerPos = IronSourceBannerPosition.BOTTOM;
                break;
        }
        IronSource.Agent.loadBanner(IronSourceBannerSize.SMART, bannerPos);
        showBannerAd = true;
    }
    public void DestroyBannerAd()
    {
        IronSource.Agent.destroyBanner();
        showBannerAd = false;
    }
    public void ShowRewardedAd(Action<RewardedAdResult> callback)
    {
        rewardedAdCallbacks += callback;

        if (IronSource.Agent.isRewardedVideoAvailable())
            IronSource.Agent.showRewardedVideo();
        else if (IronSource.Agent.isInterstitialReady())
            IronSource.Agent.showInterstitial();
        else
        {
            IronSource.Agent.loadRewardedVideo();
            IronSource.Agent.loadInterstitial();
            rewardedAdCallbacks?.Invoke(RewardedAdResult.Failed);
            rewardedAdCallbacks = null;
        }
    }
    public bool IsRewardedAdAvailable()
    {
        if (IronSource.Agent.isRewardedVideoAvailable())
        {
            return true;
        }else if (IronSource.Agent.isInterstitialReady())
        {
            IronSource.Agent.loadRewardedVideo();
            return true;
        }
        else
        {
            IronSource.Agent.loadRewardedVideo();
            IronSource.Agent.loadInterstitial();
            return false;
        }
    }
    #endregion

    #region Rewarded callbacks
    /************* RewardedVideo AdInfo Delegates *************/
    // Indicates that there’s an available ad.
    // The adInfo object includes information about the ad that was loaded successfully
    // This replaces the RewardedVideoAvailabilityChangedEvent(true) event
    void RewardedVideoOnAdAvailable(IronSourceAdInfo adInfo)
    {
        onRewardedAdAvailableEvent?.Invoke();
    }
    // Indicates that no ads are available to be displayed
    // This replaces the RewardedVideoAvailabilityChangedEvent(false) event
    void RewardedVideoOnAdUnavailable()
    {
        onRewardedAdUnavailableEvent?.Invoke();
    }
    // The Rewarded Video ad view has opened. Your activity will loose focus.
    void RewardedVideoOnAdOpenedEvent(IronSourceAdInfo adInfo)
    {
        onRewardedAdOpenedEvent?.Invoke();
    }
    // The Rewarded Video ad view is about to be closed. Your activity will regain its focus.
    void RewardedVideoOnAdClosedEvent(IronSourceAdInfo adInfo)
    {
        IronSource.Agent.loadRewardedVideo();
        onRewardedAdClosedEvent?.Invoke();
    }
    // The user completed to watch the video, and should be rewarded.
    // The placement parameter will include the reward data.
    // When using server-to-server callbacks, you may ignore this event and wait for the ironSource server callback.
    void RewardedVideoOnAdRewardedEvent(IronSourcePlacement placement, IronSourceAdInfo adInfo)
    {
        onRewardedAdRewardedEvent?.Invoke();
        rewardedAdCallbacks?.Invoke(RewardedAdResult.Successful);
        rewardedAdCallbacks = null;
    }
    // The rewarded video ad was failed to show.
    void RewardedVideoOnAdShowFailedEvent(IronSourceError error, IronSourceAdInfo adInfo)
    {
        onRewardedAdShowFailedEvent?.Invoke();
        rewardedAdCallbacks?.Invoke(RewardedAdResult.Failed);
        rewardedAdCallbacks = null;
    }
    // Invoked when the video ad was clicked.
    // This callback is not supported by all networks, and we recommend using it only if
    // it’s supported by all networks you included in your build.
    void RewardedVideoOnAdClickedEvent(IronSourcePlacement placement, IronSourceAdInfo adInfo)
    {
        onRewardedAdClickedEvent?.Invoke();
    }
    #endregion

    #region Interstitial callbacks
    /************* Interstitial AdInfo Delegates *************/
    // Invoked when the interstitial ad was loaded succesfully.
    void InterstitialOnAdReadyEvent(IronSourceAdInfo adInfo)
    {
        onInterstitialAdReadyEvent?.Invoke();
    }
    // Invoked when the initialization process has failed.
    void InterstitialOnAdLoadFailed(IronSourceError ironSourceError)
    {
        IronSource.Agent.loadInterstitial();
        onInterstitialAdLoadFailedEvent?.Invoke();
    }
    // Invoked when the Interstitial Ad Unit has opened. This is the impression indication. 
    void InterstitialOnAdOpenedEvent(IronSourceAdInfo adInfo)
    {
        onInterstitialAdOpenedEvent?.Invoke();
    }
    // Invoked when end user clicked on the interstitial ad
    void InterstitialOnAdClickedEvent(IronSourceAdInfo adInfo)
    {
        onInterstitialAdClickedEvent?.Invoke();
    }
    // Invoked when the ad failed to show.
    void InterstitialOnAdShowFailedEvent(IronSourceError ironSourceError, IronSourceAdInfo adInfo)
    {
        onInterstitialAdShowFailedEvent?.Invoke();
        rewardedAdCallbacks?.Invoke(RewardedAdResult.Failed);
        rewardedAdCallbacks = null;
    }
    // Invoked when the interstitial ad closed and the user went back to the application screen.
    void InterstitialOnAdClosedEvent(IronSourceAdInfo adInfo)
    {
        IronSource.Agent.loadInterstitial();
        onInterstitialAdClosedEvent?.Invoke();
        rewardedAdCallbacks?.Invoke(RewardedAdResult.Successful);
        rewardedAdCallbacks = null;
    }
    // Invoked before the interstitial ad was opened, and before the InterstitialOnAdOpenedEvent is reported.
    // This callback is not supported by all networks, and we recommend using it only if  
    // it's supported by all networks you included in your build. 
    void InterstitialOnAdShowSucceededEvent(IronSourceAdInfo adInfo)
    {
        Debug.LogWarning("Interstitial showd success fully");
        onInterstitialAdShowSucceededEvent?.Invoke();
        onRewardedAdRewardedEvent?.Invoke(); // Incase of using interstitial ad as rewarded ad
        rewardedAdCallbacks?.Invoke(RewardedAdResult.Successful);
        rewardedAdCallbacks = null;
    }
    #endregion

    #region Banner callbacks
    /************* Banner AdInfo Delegates *************/
    //Invoked once the banner has loaded
    void BannerOnAdLoadedEvent(IronSourceAdInfo adInfo)
    {
        if (showBannerAd)
            IronSource.Agent.displayBanner();
        onBannerAdLoadedEvent?.Invoke();
    }
    //Invoked when the banner loading process has failed.
    void BannerOnAdLoadFailedEvent(IronSourceError ironSourceError)
    {
        IronSource.Agent.loadBanner(IronSourceBannerSize.SMART, IronSourceBannerPosition.BOTTOM);
        onBannerAdLoadFailedEvent?.Invoke();
    }
    // Invoked when end user clicks on the banner ad
    void BannerOnAdClickedEvent(IronSourceAdInfo adInfo)
    {
        onBannerAdClickedEvent?.Invoke();
    }
    //Notifies the presentation of a full screen content following user click
    void BannerOnAdScreenPresentedEvent(IronSourceAdInfo adInfo)
    {
        onBannerAdScreenPresentedEvent?.Invoke();
    }
    //Notifies the presented screen has been dismissed
    void BannerOnAdScreenDismissedEvent(IronSourceAdInfo adInfo)
    {
        onBannerAdScreenDismissedEvent?.Invoke();
    }
    //Invoked when the user leaves the app
    void BannerOnAdLeftApplicationEvent(IronSourceAdInfo adInfo)
    {
        IronSource.Agent.destroyBanner();
        onBannerAdLeftApplicationEvent?.Invoke();
    }
    #endregion


    public enum RewardedAdResult
    {
        Successful,
        Failed
    }
    public enum BannerAdPosition
    {
        TOP,
        BOTTOM
    }

}
