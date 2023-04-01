using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class OptionsMenu : MonoBehaviour
{

    public AudioMixer mainMixer;
    public GameObject canvas;
    public static OptionsMenu optionsMenu;
    public bool isGameplayScene;
    public GameObject summary;

    //Gets reference to GameSummary at startup and sets this GameObject to DontDestroyOnLoad
    private void Awake()
    {
        
        summary = GameObject.Find("Game Summary");
        
        if (optionsMenu != null)
        {
            Destroy(gameObject);
            return;
        }

        optionsMenu = this;
        DontDestroyOnLoad(gameObject);
        Invoke("Deactivate", .01f);
    }

    public void Deactivate()
    {
        canvas.SetActive(false);
    }

    public void SetVolume(float volume)
    {
        mainMixer.SetFloat("volume", volume);
    }

    public void SetFullscreen(bool isFullscreen){
        Screen.fullScreen = isFullscreen;
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void CloseOptions()
    {
        Deactivate();
    }

    public void LeaveGame()
    {
        summary.SetActive(true);
        CloseOptions();
    }

    void Update()
    {
        if (Input.GetKeyDown("escape"))
        {
            Deactivate();
        }
    }
}
