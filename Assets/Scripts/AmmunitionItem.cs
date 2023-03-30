using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmunitionItem : MonoBehaviour
{
    [SerializeField] public int ammoCount;
    [SerializeField] SpriteRenderer renderer;
    // Start is called before the first frame update
    void Start()
    {
        ammoCount = 2 + Random.Range(1, 9);

        renderer.color = new Color(1, 1f - (float)((ammoCount - 3) * 0.05), 1f - (float)((ammoCount-3) * 0.1));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
