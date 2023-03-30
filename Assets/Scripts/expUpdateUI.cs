using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class expUpdateUI : MonoBehaviour
{
    // Start is called before the first frame update

    //UI Gameobjects and Variables
    public int exp;
    public Text xpText1;

    //Text components
    

    void Start()
    {
        

    }

    // Update is called once per frame
    void Update()
    {
        xpText1.text = "XP: " + exp.ToString();
    }

    
}
