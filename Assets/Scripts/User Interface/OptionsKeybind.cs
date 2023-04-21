using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsKeybind : MonoBehaviour
{
    public GameObject options;
    public static OptionsKeybind optionsKeybind;
    public OptionsMenu optionsMenu;

    private void Awake()
    {
        if (optionsKeybind != null)
        {
            Destroy(gameObject);
            return;
        }

        optionsKeybind = this;
        DontDestroyOnLoad(gameObject);
        optionsMenu = options.GetComponent<OptionsMenu>();
    }

    //Press Escape to open the options.
    void Update()
    {
        if (Input.GetKeyDown("escape"))
        {
            if (optionsMenu.GetComponent<Canvas>().enabled == false)
            {
                Debug.Log(optionsMenu.GetComponent<Canvas>().enabled);
                optionsMenu.Activate();
            }
            else
            {
                optionsMenu.Deactivate();
            }
        }
    }
}
