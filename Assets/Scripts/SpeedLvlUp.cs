using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedLvlUp : MonoBehaviour
{

    [SerializeField] public GameObject player;
    public float speedXP = 0f;
    public PlayerScript PlayerScript;
    
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame 
    void Update()
    {
        //Gaining XP from moving around
        if (PlayerScript.movementInput != Vector2.zero)
        {
            speedXP += 1f * Time.deltaTime;
        }

        //Levelling Up
        if (speedXP >= 5 && PlayerScript.moveSpeed < 2)
        {
            PlayerScript.moveSpeed += 0.01f;
            speedXP = 0;
        }

    }
}
