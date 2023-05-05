using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameSummary : MonoBehaviour
{
    public static GameSummary instance;
    public GameObject summary;
    public static Canvas canvas;
    public int kills;
    public float xp;
    [SerializeField]
    private Text text;

    private void Awake()
    {
        canvas = GetComponent<Canvas>();
        text = GameObject.Find("SummaryText").GetComponent<Text>();

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
        text.text = "LEVEL: "+"\nKILLS: "+kills+"\nXP: "+xp;
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
