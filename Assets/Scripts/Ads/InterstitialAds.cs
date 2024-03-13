using UnityEngine;
using UnityEngine.Advertisements;

public class InterstitialAds : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener
{
    [SerializeField] string _androidAdUnitId = "Interstitial_Android";
    [SerializeField] string _iOsAdUnitId = "Interstitial_iOS";
    string _adUnitId;

    void Awake()
    {
        // Get the Ad Unit ID for the current platform:
        _adUnitId = (Application.platform == RuntimePlatform.IPhonePlayer)
            ? _iOsAdUnitId
            : _androidAdUnitId;
    }

    /// <summary>
    /// This function will load ads.
    /// </summary>
    public void LoadAd()
    {
        // IMPORTANT! Only load content AFTER initialization (in this example, initialization is handled in a different script).
        Debug.Log("Loading Ad: " + _adUnitId);
        Advertisement.Load(_adUnitId, this);
    }

    /// <summary>
    /// This function will show ads.
    /// </summary>
    public void ShowAd()
    {
        // Note that if the ad content wasn't previously loaded, this method will fail
        Debug.Log("Showing Ad: " + _adUnitId);
        Advertisement.Show(_adUnitId, this);
    }

    /// <summary>
    /// A collection of events that will be triggered when the ad is loaded
    /// </summary>
    /// <param name="adUnitId">String code of ads</param>
    public void OnUnityAdsAdLoaded(string adUnitId)
    {
        Debug.Log("Uploaded");
        // Optionally execute code if the Ad Unit successfully loads content.
    }

    /// <summary>
    /// A collection of events that will be triggered when the ad fails to load
    /// </summary>
    /// <param name="_adUnitId">String code of ads</param>
    /// <param name="error">Event error</param>
    /// <param name="message">String message</param>
    public void OnUnityAdsFailedToLoad(string _adUnitId, UnityAdsLoadError error, string message)
    {
        Debug.Log($"Error loading Ad Unit: {_adUnitId} - {error.ToString()} - {message}");
        // Optionally execute code if the Ad Unit fails to load, such as attempting to try again.
    }

    /// <summary>
    /// A collection of events that will be triggered when an ad fails
    /// </summary>
    /// <param name="_adUnitId">String code of ads</param>
    /// <param name="error">Event error</param>
    /// <param name="message">String message</param>
    public void OnUnityAdsShowFailure(string _adUnitId, UnityAdsShowError error, string message)
    {
        Debug.Log($"Error showing Ad Unit {_adUnitId}: {error.ToString()} - {message}");
        // Optionally execute code if the Ad Unit fails to show, such as loading another ad.
    }

    public void OnUnityAdsShowStart(string _adUnitId) { }
    public void OnUnityAdsShowClick(string _adUnitId) { }
    public void OnUnityAdsShowComplete(string _adUnitId, UnityAdsShowCompletionState showCompletionState) { }
}