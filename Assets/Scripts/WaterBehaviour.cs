using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterBehaviour : MonoBehaviour
{
    public static List<Vector2Int> waterTiles = new List<Vector2Int>();
    [SerializeField] float[] spreadRates = new float[] { 0.5f, 0.5f, 0.5f, 0.5f };
    [SerializeField] float decreaseRate = 0.7f;
    public GameObject parent;
    float splashTime = 0;
    [SerializeField] GameObject splashEffect;
    [SerializeField] Material flashMaterial;

    int[] tileLocation { get { return new int[] { (int)(transform.position.x / 0.16f) + isXPos, (int)(transform.position.y / 0.16f) + isYPos }; } }
    int isXPos;
    int isYPos;

    // Start is called before the first frame update
    void Start()
    {
        isXPos = (transform.position.x < 0 ? -1 : 1);
        isYPos = (transform.position.y < 0 ? -1 : 1);

        if (!waterTiles.Contains(new Vector2Int(tileLocation[0], tileLocation[1]+1)) && Random.Range(1, 101) < spreadRates[0] * 100)
        {
            waterTiles.Add(new Vector2Int(tileLocation[0], tileLocation[1] + 1));
            GameObject newTile = Instantiate(gameObject, new Vector2(tileLocation[0] * 0.16f - 0.08f * isXPos, (tileLocation[1] + 1) * 0.16f - 0.08f * isYPos), Quaternion.identity);
            float[] newRates = new float[4];
            for (int i = 0; i < 4; i++)
            {
                newRates[i] = spreadRates[i] * decreaseRate;
            }
            newTile.SendMessage("UpdateRates", newRates);
            newTile.SendMessage("AddParent", gameObject);
            // Debug.Log("Added water up at " + newTile.transform.position + " from " + transform.position + " - " + tileLocation[0] + ", " + tileLocation[1]);
        }
        if (!waterTiles.Contains(new Vector2Int(tileLocation[0] + 1, tileLocation[1])) && Random.Range(1, 101) < spreadRates[1] * 100)
        {
            waterTiles.Add(new Vector2Int(tileLocation[0] + 1, tileLocation[1]));
            GameObject newTile = Instantiate(gameObject, new Vector2((tileLocation[0] + 1) * 0.16f - 0.08f * isXPos, (tileLocation[1]) * 0.16f - 0.08f * isYPos), Quaternion.identity);
            float[] newRates = new float[4];
            for (int i = 0; i < 4; i++)
            {
                newRates[i] = spreadRates[i] * decreaseRate;
            }
            newTile.SendMessage("UpdateRates", newRates);
            newTile.SendMessage("AddParent", gameObject);
            // Debug.Log("Added water right at " + newTile.transform.position + " from " + transform.position + " - " + tileLocation[0] + ", " + tileLocation[1]);
        }
        if (!waterTiles.Contains(new Vector2Int(tileLocation[0], tileLocation[1] - 1)) && Random.Range(1, 101) < spreadRates[2] * 100)
        {
            waterTiles.Add(new Vector2Int(tileLocation[0], tileLocation[1] - 1));
            GameObject newTile = Instantiate(gameObject, new Vector2(tileLocation[0] * 0.16f - 0.08f * isXPos, (tileLocation[1] - 1) * 0.16f - 0.08f * isYPos), Quaternion.identity);
            float[] newRates = new float[4];
            for (int i = 0; i < 4; i++)
            {
                newRates[i] = spreadRates[i] * decreaseRate;
            }
            newTile.SendMessage("UpdateRates", newRates);
            newTile.SendMessage("AddParent", gameObject);
            // Debug.Log("Added water down at " + newTile.transform.position + " from " + transform.position + " - " + tileLocation[0] + ", " + tileLocation[1]);
        }
        if (!waterTiles.Contains(new Vector2Int(tileLocation[0] - 1, tileLocation[1])) && Random.Range(1, 101) < spreadRates[3] * 100)
        {
            waterTiles.Add(new Vector2Int(tileLocation[0] - 1, tileLocation[1]));
            GameObject newTile = Instantiate(gameObject, new Vector2((tileLocation[0] - 1 ) * 0.16f - 0.08f * isXPos, (tileLocation[1]) * 0.16f - 0.08f * isYPos), Quaternion.identity);
            float[] newRates = new float[4];
            for (int i = 0; i < 4; i++)
            {
                newRates[i] = spreadRates[i] * decreaseRate;
            }
            newTile.SendMessage("UpdateRates", newRates);
            newTile.SendMessage("AddParent", gameObject);
            // Debug.Log("Added water left at " + newTile.transform.position + " from " + transform.position + " - " + tileLocation[0] + ", " + tileLocation[1]);
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (splashTime > 0)
        {
            splashTime -= Time.deltaTime;
        }
    }

    public void UpdateRates(float[] newRates)
    {
        spreadRates = newRates;
    }

    public void AddParent(GameObject p)
    {
        parent = p;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && splashTime <= 0)
        {
            Debug.Log("Splash");
            splashTime = 0.3f;
            GameObject effect = Instantiate(splashEffect, collision.gameObject.transform.position, Quaternion.identity);
            effect.GetComponent<SpriteRenderer>().material = flashMaterial;
            effect.GetComponent<SpriteRenderer>().color = new Color(.9f, .9f, 1f);
            Destroy(effect, 0.25f);
        }
        //Debug.Log("Splash");
    }

    
}
