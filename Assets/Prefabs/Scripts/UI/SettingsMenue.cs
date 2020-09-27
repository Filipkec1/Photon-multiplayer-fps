using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenue : MonoBehaviour
{
    //audio
    public AudioMixer audioMixer;

    public void SetVolumen(float _volumen)
    {
        audioMixer.SetFloat("Volume", _volumen);
    }

    //graphics quality
    public void SetQuality(int _qualityIndex)
    {
        QualitySettings.SetQualityLevel(_qualityIndex);
    }

    //fullscreem
    public void SetFullScreen (bool _isFullScreen)
    {
        Screen.fullScreen = _isFullScreen;
    }

    //resolution

    Resolution[] resolutions;

    public Dropdown resolutionDropdown;

    //start
    public GameObject mainMenu;
    public GameObject optionsMenu;

    void Start()
    {
        mainMenu.SetActive(true);
        optionsMenu.SetActive(false);

        resolutions = Screen.resolutions;

        resolutionDropdown.ClearOptions();

        List<string> resolutionOptions = new List<string>();

        int currentResolutionIndex = 0;
        for(int i=0; i< resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            resolutionOptions.Add(option);

            if(resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(resolutionOptions);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    public void setResolution(int _resolutionIndex)
    {
        Resolution resolution = resolutions[_resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }



}
