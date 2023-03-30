using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string firstLevel;
    public GameObject options;

    //Start Button
    public void StartGame(){
        SceneManager.LoadScene(firstLevel);
    }

    public void OpenOptions(){
        options.SetActive(true);
    }

    //Quit button
    public void QuitGame(){
        Application.Quit();
        Debug.Log("Quitting");
    }

    void Update()
    {
        if (Input.GetKeyDown("escape"))
        {
            options.SetActive(true);
        }
    }
}
