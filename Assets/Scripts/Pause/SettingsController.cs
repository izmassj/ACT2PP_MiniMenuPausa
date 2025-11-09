using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Collections.Generic;
using TMPro;

public class SettingsController : MonoBehaviour
{
    [Header("UI Resolution")]
    public TMP_Dropdown resolutionDropdown;
    public Toggle fullscreenToggle;

    const string RES_KEY = "resolutionIndex";
    const string FULL_KEY = "fullscreen";

    Resolution[] available;
    List<Resolution> uniqueRes = new List<Resolution>();

    void Start()
    {
        available = Screen.resolutions;
        var pairs = new HashSet<string>();
        foreach (var r in available)
        {
            float ratio = (float)r.width / r.height;
            bool is16_9 = Mathf.Abs(ratio - 16f / 9f) < 0.01f;
            bool is16_10 = Mathf.Abs(ratio - 16f / 10f) < 0.01f;

            if ((is16_9 || is16_10) && !pairs.Contains(r.width + "x" + r.height))
            {
                pairs.Add(r.width + "x" + r.height);
                uniqueRes.Add(r);
            }
        } 

        resolutionDropdown.ClearOptions();
        var options = uniqueRes.Select(r => r.width + " x " + r.height + " (" + (Mathf.Abs((float)r.width / r.height - 16f / 9f) < 0.01f ? "16:9" : "16:10") + ")").ToList();

        resolutionDropdown.AddOptions(options);

        int saved = PlayerPrefs.HasKey(RES_KEY) ? PlayerPrefs.GetInt(RES_KEY) : FindCurrentResolutionIndex();
        resolutionDropdown.value = Mathf.Clamp(saved, 0, options.Count - 1);
        fullscreenToggle.isOn = PlayerPrefs.GetInt(FULL_KEY, Screen.fullScreen ? 1 : 0) == 1;

        resolutionDropdown.onValueChanged.AddListener(OnResolutionChanged);
        fullscreenToggle.onValueChanged.AddListener(OnFullscreenChanged);
    }


    int FindCurrentResolutionIndex()
    {
        for (int i = 0; i < uniqueRes.Count; i++)
        {
            if (uniqueRes[i].width == Screen.currentResolution.width && uniqueRes[i].height == Screen.currentResolution.height)
                return i;
        }
        return 0;
    }

    public void OnResolutionChanged(int idx)
    {
        var r = uniqueRes[idx];
        Screen.SetResolution(r.width, r.height, Screen.fullScreenMode); 
        PlayerPrefs.SetInt(RES_KEY, idx);
    }

    public void OnFullscreenChanged(bool fs)
    {
        Screen.fullScreenMode = fs ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
        PlayerPrefs.SetInt(FULL_KEY, fs ? 1 : 0);
    }

    public void RestoreDefaults()
    {
        PlayerPrefs.DeleteKey(RES_KEY);
        PlayerPrefs.DeleteKey(FULL_KEY);
    }
}
