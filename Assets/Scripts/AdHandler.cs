using UnityEngine;
using UnityEngine.Advertisements;

public class AdHandler : MonoBehaviour, IUnityAdsListener
{
    [SerializeField] private string _androidGameId = "4232873";
    [SerializeField] private string _iOsGameId = "4232872";
    [SerializeField] private bool _testMode = true;
    [SerializeField] private string _androidRewardID = "Rewarded_Android";
    [SerializeField] private string _iOsRewardID = "Rewarded_iOS";
    [SerializeField] private string _iOsInterstitial = "Interstitial_iOS";
    [SerializeField] private string _androidInterstitial = "Interstitial_Android";
    public AudioManager _audiomanager;
    void Start()
    {
        MetaData metaData = new MetaData("privacy");
        metaData.Set("mode", "app"); // This app is directed at children; no users will receive personalized ads.
        Advertisement.SetMetaData(metaData);
        InitializeAds();
    }

    private void OnDestroy()
    {
        Advertisement.RemoveListener(this);
    }

    public void InitializeAds()
    {
        Advertisement.AddListener(this);
        if (Application.platform == RuntimePlatform.Android)
        {
            Advertisement.Initialize(_androidGameId, _testMode);
        }
        else if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            Advertisement.Initialize(_iOsGameId, _testMode);
        }
    }

    public void DisplayInterstitialAd()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            if (Advertisement.IsReady(_androidInterstitial))
            {
                _audiomanager.MusicVolume(-80.0f);
                Advertisement.Show(_androidInterstitial);
            }
        }
        else if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            if (Advertisement.IsReady(_iOsInterstitial))
            {
                _audiomanager.MusicVolume(-80.0f);
                Advertisement.Show(_iOsInterstitial);
            }
        }
    }

    public void DisplayVideoAd()
    {
        if(Application.platform == RuntimePlatform.Android)
        {
            if (Advertisement.IsReady(_androidRewardID))
            {
                _audiomanager.MusicVolume(-80.0f);
                Advertisement.Show(_androidRewardID);
            }
        }else if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            if (Advertisement.IsReady(_iOsRewardID))
            {
                _audiomanager.MusicVolume(-80.0f);
                Advertisement.Show(_iOsRewardID);
            }
        }
    }

    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        if (placementId == _androidRewardID &&  showResult == ShowResult.Finished || placementId == _iOsRewardID && showResult == ShowResult.Finished)
        {
            GameManager.Instance.SavedGreens += 20;
            GameManager.Instance._mainmenu.UpdateSavedGreens();
            GameManager.Instance.AdCounter(0, true);
            GameManager.Instance.SaveData();
            _audiomanager.MusicVolume(0.0f);
        }
        else if (placementId == _androidInterstitial && showResult == ShowResult.Finished || placementId == _iOsInterstitial && showResult == ShowResult.Finished)
        {
            _audiomanager.MusicVolume(0.0f);
        }
        else if (showResult == ShowResult.Skipped)
        {

            _audiomanager.MusicVolume(0.0f);
        }
        else if (showResult == ShowResult.Failed)
        {
            Debug.LogWarning("The ad did not finish due to an error.");
            _audiomanager.MusicVolume(0.0f);
        }
    }

    public void OnUnityAdsReady(string placementId)
    {
        // must have method because inherits interface
    }

    public void OnUnityAdsDidError(string message)
    {
        Debug.LogError("There is an error with ads.");
    }

    public void OnUnityAdsDidStart(string placementId)
    {
        // must have method because inherits interface
    }
  
}