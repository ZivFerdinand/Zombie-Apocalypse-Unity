using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;

public class BannerAds : MonoBehaviour
{
    [SerializeField] Button _loadBannerButton;
    [SerializeField] Button _showBannerButton;
    [SerializeField] Button _hideBannerButton;

    [SerializeField] BannerPosition _bannerPosition = BannerPosition.BOTTOM_CENTER;

    [SerializeField] string _androidAdUnitId = "Banner_Android";
    [SerializeField] string _iOSAdUnitId = "Banner_iOS";
    string _adUnitId = null; // This will remain null for unsupported platforms.

    void Start()
    {

#if UNITY_IOS
        _adUnitId = _iOSAdUnitId;
#elif UNITY_ANDROID
        _adUnitId = _androidAdUnitId;
#endif

        // Set the banner position:
        Advertisement.Banner.SetPosition(_bannerPosition);

        LoadBanner();
    }

    /// <summary>
    /// Implement a method to call when the Load Banner button is clicked.
    /// </summary>
    public void LoadBanner()
    {
        // Set up options to notify the SDK of load events:
        BannerLoadOptions options = new BannerLoadOptions
        {
            loadCallback = OnBannerLoaded,
            errorCallback = OnBannerError
        };

        // Load the Ad Unit with banner content:
        Advertisement.Banner.Load(_adUnitId, options);
    }

    /// <summary>
    /// Implement code to execute when the loadCallback event triggers.
    /// </summary>
    void OnBannerLoaded()
    {
        Debug.Log("Banner loaded");
    }

    /// <summary>
    /// Implement code to execute when the loadCallback event triggers.
    /// </summary>
    void OnBannerError(string message)
    {
        Debug.Log($"Banner Error: {message}");
        // Optionally execute additional code, such as attempting to load another ad.
    }

    /// <summary>
    /// This function will show the Banner when the button is clicked.
    /// </summary>
    public void ShowBannerAd()
    {
        // Set up options to notify the SDK of show events:
        BannerOptions options = new BannerOptions
        {
            clickCallback = OnBannerClicked,
            hideCallback = OnBannerHidden,
            showCallback = OnBannerShown
        };
        Advertisement.Banner.SetPosition(_bannerPosition);

        // Show the loaded Banner Ad Unit:
        Advertisement.Banner.Show(_adUnitId, options);
        Advertisement.Banner.SetPosition(_bannerPosition);
    }

    /// <summary>
    /// Implement a method to call when the Hide Banner button is clicked.
    /// </summary>
    public void HideBannerAd()
    {
        // Hide the banner:
        Advertisement.Banner.Hide();
    }

    void OnBannerClicked() { }
    void OnBannerShown() { }
    void OnBannerHidden() { }
}