using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextIndicator : MonoBehaviour
{
    [SerializeField] float floatSpeed;
    [SerializeField] float xOffset;
    float timeTracker;
    float randomBiasDirection;

    // Start is called before the first frame update
    void Start()
    {
        xOffset = 0;
        timeTracker = 0;
        randomBiasDirection = Random.Range(-10, 10) * .01f;
    }

    // Update is called once per frame
    void Update()
    {
        timeTracker += Time.deltaTime;
        //xOffset = Mathf.Sin(timeTracker * 5);
        //transform.position += new Vector3((xOffset/3 + randomBiasDirection) * Time.deltaTime, floatSpeed * Time.deltaTime);
        transform.position += new Vector3(0, floatSpeed * Time.deltaTime);
        if (timeTracker > 0.5)
        {
            Destroy(gameObject);
        }
    }
}
