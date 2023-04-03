using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSummary : MonoBehaviour
{
    public static GameSummary instance;
    public GameObject summary;
    public static Canvas canvas;
    public int kills;
    public int damageDealt;
    public float speedxp;

    private void Awake()
    {
        canvas = GetComponent<Canvas>();

        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
        Invoke("Deactivate", .01f);
    }

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

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Deactivate();
    }

    public void LeaveGame()
    {
        //Save xp I guess?
        SceneManager.LoadScene("MainMenu");
        Deactivate();

    }

    void Update()
    {
        
    }
}
