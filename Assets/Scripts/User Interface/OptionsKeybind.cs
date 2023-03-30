using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsKeybind : MonoBehaviour
{
    public GameObject options;
    public static OptionsKeybind optionsKeybind;

    private void Awake()
    {
        if (optionsKeybind != null)
        {
            Destroy(gameObject);
            return;
        }

        optionsKeybind = this;
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        if (Input.GetKeyDown("escape"))
        {
            options.SetActive(true);
        }
    }
}
