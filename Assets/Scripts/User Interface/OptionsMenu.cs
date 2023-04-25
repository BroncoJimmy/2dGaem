using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class OptionsMenu : MonoBehaviour
{

    public AudioMixer mainMixer;
    public GameObject instance;
    public static Canvas canvas;
    public static OptionsMenu optionsMenu;
    public bool isGameplayScene;
    public GameSummary summary;

    //Gets reference to GameSummary at startup, sets this GameObject to DontDestroyOnLoad.
    private void Awake()
    {
        if (optionsMenu != null)
        {
            Destroy(gameObject);
            return;
        }

        optionsMenu = this;
        DontDestroyOnLoad(gameObject);
        Invoke("Deactivate", .01f);
    }

    //The two methods below disable/enable the Canvas component of this GameObject which stops the menu from rendering.
    public void Deactivate()
    {
        canvas = GetComponent<Canvas>();
        canvas.enabled = false;
    }

    public void Activate()
    {
        canvas = GetComponent<Canvas>();
        canvas.enabled = true;
    }

    //Remaining methods are for the buttons
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
        summary.Activate();
        CloseOptions();
    }

}
