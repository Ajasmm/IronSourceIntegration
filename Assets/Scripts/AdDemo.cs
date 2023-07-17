using UnityEngine;

public class AdDemo : MonoBehaviour
{
    private void OnEnable()
    {
        AdManager.Instance.Initialize();
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
        AdManager.Instance.ShowRewardedAd((RewardedAdResult result) => { Debug.LogWarning(result.ToString()); });
    }
}
