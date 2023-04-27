using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageXP : MonoBehaviour
{

    [SerializeField] public int shootLevel = 1;
    [SerializeField] public int shootXP = 0;
    public GameObject gunObject;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (shootXP >= 100) {
            shootLevel = 2;
        }

        if (shootXP >= 200) {
            shootLevel = 3;
        }

        if (shootXP >= 300) {
            shootLevel = 3;
        }

        if (shootLevel == 2) {
            GunScript Gundmg = gunObject.GetComponent<GunScript>();
            Gundmg.gunDamage = 25;
        }
    }
}
