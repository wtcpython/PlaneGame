using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ResolutionManager : MonoBehaviour
{
    public TMP_Dropdown resolutionDropdown;
    public TMP_Dropdown fullscreenDropdown;
    
    Resolution[] resolutions;
    private int currentResolutionIndex = 0;
    private int currentFullscreenIndex = 0;
    
    void Start()
    {
        EventSystem.current.SetSelectedGameObject(this.resolutionDropdown.gameObject);
        resolutions = Screen.resolutions
            .GroupBy(r => new { r.width, r.height })
            .Select(g => g.First())
            .OrderByDescending(r => r.width)
            .ThenByDescending(r => r.height)
            .ToArray();
        
        resolutionDropdown.ClearOptions();
        
        List<string> options = new List<string>();
        
        for (int i = 0; i < resolutions.Length; i++)
        {
            var r = resolutions[i];
            string option = $"{r.width} x {r.height}";
            options.Add(option);
        
            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }
        resolutionDropdown.AddOptions(options);
        if (PlayerPrefs.HasKey("ResolutionIndex"))
        {
            currentResolutionIndex = PlayerPrefs.GetInt("ResolutionIndex");
        }
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
        
        SetResolution(currentResolutionIndex);
        
        fullscreenDropdown.ClearOptions();
        List<string> fullscreenOptions = new List<string> { "窗口模式", "全屏模式" };
        fullscreenDropdown.AddOptions(fullscreenOptions);
        
        currentFullscreenIndex = PlayerPrefs.HasKey("FullscreenIndex") ? PlayerPrefs.GetInt("FullscreenIndex") : (Screen.fullScreen ? 1 : 0);
        fullscreenDropdown.value = currentFullscreenIndex;
        fullscreenDropdown.RefreshShownValue();
        
        SetFullScreenMode(currentFullscreenIndex);
        
        resolutionDropdown.onValueChanged.AddListener(SetResolution);
        fullscreenDropdown.onValueChanged.AddListener(SetFullScreenMode);
    }

    private void SetResolution(int index)
    {
        Resolution resolution = resolutions[index];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        PlayerPrefs.SetInt("ResolutionIndex", index);
        PlayerPrefs.Save();
    }
    
    private void SetFullScreenMode(int index)
    {
        Screen.fullScreen = (index == 1);
        PlayerPrefs.SetInt("FullScreenIndex", index);
        PlayerPrefs.Save();
    }
}
