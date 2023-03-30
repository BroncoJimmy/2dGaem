using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugFollow : MonoBehaviour
{
    [SerializeField] GameObject target;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(target.transform.position.x + 0.01361722f, target.transform.position.y - 0.007781148f);
    }
}
