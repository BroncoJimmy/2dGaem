using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSummary : MonoBehaviour
{
    public static GameSummary instance;
    public GameObject summary;
    
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
        summary.SetActive(false);
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LeaveGame()
    {
        //Save xp I guess?
        SceneManager.LoadScene("MainMenu");
        summary.SetActive(false);

    }

    void Update()
    {
        
    }
}
