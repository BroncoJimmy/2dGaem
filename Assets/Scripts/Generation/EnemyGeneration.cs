using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGeneration : MonoBehaviour
{
    public static IDictionary<Vector3, GameObject> unloadedEnemies = new Dictionary<Vector3, GameObject>();
    public static List<GameObject> loadedEnemies = new List<GameObject>();

    public static IDictionary<Vector3, GameObject> unloadedItems = new Dictionary<Vector3, GameObject>();
    public static List<GameObject> loadedItems = new List<GameObject>();

    [SerializeField] float renderDistance = 5;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        removeEnemies(Globals.player.transform.position);
        removeItems(Globals.player.transform.position);
    }

    public static void generateEnemy(GameObject enemy, Vector3 newLocation)
    {
        loadedEnemies.Add(Instantiate(enemy, newLocation, Quaternion.Euler(0, 0, 0)));
    }

    public static void generateItem(GameObject item, Vector3 newLocation)
    {
        loadedItems.Add(Instantiate(item, newLocation, Quaternion.Euler(0, 0, 0)));
    }

    private void removeEnemies(Vector3 loadPoint)
    {
        //List<Vector3> removeKeys = new List<Vector3>();
        for (int instance = 0; instance < loadedEnemies.Count;  instance++)
        {
            if (loadedEnemies[instance] != null) { 
            if ((loadedEnemies[instance].transform.position - loadPoint).magnitude > renderDistance)
            {
                //Debug.Log("Unloaded enemy at " + loadedEnemies[instance].transform.position + ", Load point: " + loadPoint);
                //removeKeys.Add(instance.Key);
                unloadedEnemies.Add(loadedEnemies[instance].transform.position, loadedEnemies[instance]);
                Destroy(loadedEnemies[instance]);
                    //Debug.Log(instance + ", " + loadedEnemies[instance].transform.position);
                loadedEnemies.RemoveAt(instance);
                    //Debug.Log(instance + ", " + loadedEnemies[instance].transform.position);
                instance--;

            } 
            }
        }
        /*foreach (Vector3 key in removeKeys)
        {
            enemies.Add(enemies[key].transform.position, enemies[key]);
            enemies[key].GetComponent<EnemyScript>().unload();
            enemies.Remove(key);
            
        }*/
    }

    private void removeItems(Vector3 loadPoint)
    {
        for (int instance = 0; instance < loadedItems.Count; instance++)
        {
            if (loadedItems[instance] != null)
            {
                if ((loadedItems[instance].transform.position - loadPoint).magnitude > renderDistance)
                {
                    //Debug.Log("Unloaded item at " + loadedItems[instance].transform.position + ", Load point: " + loadPoint);
                    unloadedItems.Add(loadedItems[instance].transform.position, loadedItems[instance]);
                    Destroy(loadedItems[instance]);
                    loadedItems.RemoveAt(instance);
                    instance--;

                }
            }
        }

    }

}

