using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Splatter : MonoBehaviour
{
    [SerializeField] float lastTime = 2f;
    public SpriteRenderer renderer;
    [SerializeField] Sprite[] sprites;
    bool isAHit { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
        
        renderer.sprite = sprites[Random.Range(0, 3)];
        Destroy(gameObject, lastTime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
