using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string firstLevel;
    public GameObject options;
    public OptionsMenu optionsMenu;

    private void Awake()
    {
        options = GameObject.Find("Options Menu");
        optionsMenu = options.GetComponent<OptionsMenu>();
    }

    //Start Button
    public void StartGame(){
        SceneManager.LoadScene(firstLevel);
    }

    public void OpenOptions(){
        optionsMenu.Activate();
    }

    //Quit button
    public void QuitGame(){
        Application.Quit();
        Debug.Log("Quitting");
    }

}
