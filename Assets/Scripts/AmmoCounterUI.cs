using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmmoCounterUI : MonoBehaviour
{

    public GameObject playerObject;
    private AmmoSystem ammoSystem;
    public Text ammoCounter;
    // Start is called before the first frame update
    void Start()
    {
        
        ammoSystem = playerObject.GetComponent<AmmoSystem>();

    }

    // Update is called once per frame
    void Update()
    {

        ammoCounter.text = "Pistol Ammo " + ammoSystem.pistolAmmo;
    }



}
