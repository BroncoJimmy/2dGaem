using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainCamera : MonoBehaviour
{
    [SerializeField] GameObject player;

    private void Awake()
    {
        player = Globals.player;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //if (SceneManager.GetActiveScene().name.Equals("BossRoom") && player.transform.position.magnitude )
        
        transform.position = player.transform.position + new Vector3(0, 0, -10);
    }

    public void PlayerChange(GameObject newPlayer)
    {
        player = newPlayer;
    }
}
