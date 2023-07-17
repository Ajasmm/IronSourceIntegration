using UnityEngine;

public class AdDemo : MonoBehaviour
{
    private void OnEnable()
    {
        AdManagerRewardedAdEvents.onAdRewardedEvent += OnRewarded;
        AdManagerRewardedAdEvents.onAdShowFailedEvent += OnFailed;

        AdManager.Instance.Initialize();
    }

    public void OnRewarded()
    {
        Debug.LogWarning("Rewarded");
    }
    public void OnFailed()
    {
        Debug.LogWarning("Not Rewarded");
    }



    public void ShowBannerAd()
    {
        AdManager.Instance.ShowBannerAd();
    }
    public void HideBannerAd()
    {
        AdManager.Instance.DestroyBannerAd();
    }
    public void ShowInterstitial()
    {
        AdManager.Instance.ShowInterstitialAd();
    }
    public void showRewarded()
    {
        AdManager.Instance.ShowRewardedAd();
    }
}
