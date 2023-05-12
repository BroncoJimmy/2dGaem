using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameplayDisplay : MonoBehaviour
{
    [SerializeField] GameObject healthDisplay;
    [SerializeField] GameObject xpDisplay;
    [SerializeField] GameObject ammoDisplay;
    [SerializeField] GameObject grenadeDisplay;

    PlayerHealth health;
    AmmoSystem ammo;
    PlayerScript grenades;

    // Start is called before the first frame update
    void Start()
    {
        health = Globals.player.GetComponent<PlayerHealth>();
        ammo = Globals.player.GetComponent<AmmoSystem>();
        grenades = Globals.player.GetComponent<PlayerScript>();
    }

    // Update is called once per frame
    void Update()
    {
        healthDisplay.GetComponent<TextMeshProUGUI>().text = ("Health: " + health.playerHealth);
        ammoDisplay.GetComponent<TextMeshProUGUI>().text = ("Ammo: " + ammo.pistolAmmo);
        grenadeDisplay.GetComponent<TextMeshProUGUI>().text = ("Bombs: " + grenades.numGrenades);
    }
}
