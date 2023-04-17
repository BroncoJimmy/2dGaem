using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FontDisplay : MonoBehaviour
{
    [SerializeField] Font[] fonts;
    static GameObject popUp;
    [SerializeField] GameObject indicatorText;
    [SerializeField] List<Color> colors = new List<Color>();
    [HideInInspector] public static int damagedColor = 0;
    [HideInInspector] public static int healedColor = 1;
    [HideInInspector] public static int bulletColor = 2;
    [HideInInspector] public static int bombColor = 3;
    static int textSortingOrder = 6;
    static GameObject popUpDisplayer;

    
    // Start is called before the first frame update
    void Start()
    {
        popUpDisplayer = GameObject.FindGameObjectWithTag("FontDisplayer");
        popUp = indicatorText;
        foreach (Font f in fonts)
        {
            f.material.mainTexture.filterMode = FilterMode.Point;
            //Debug.Log(f.material.mainTexture.filterMode.Equals(FilterMode.Point));
        }
    }

    public static void instantiate(string number, Vector2 position, int col, float scale)
    {
        
        GameObject indicator = Instantiate(popUp, new Vector3(position.x, position.y, 0), Quaternion.identity);
        indicator.GetComponent<Renderer>().sortingOrder = textSortingOrder;
        indicator.GetComponent<TextMesh>().text = number;
        Color newColor = FontDisplay.popUpDisplayer.GetComponent<FontDisplay>().colors[col];
        newColor.a = 1;
        indicator.GetComponent<TextMesh>().color = newColor;
        indicator.transform.localScale = new Vector3(0.03f, 0.03f) * scale;

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            Instantiate(popUp, new Vector2(Random.Range(0, 10) * 0.1f, Random.Range(0, 10) * 0.1f), Quaternion.identity);
        }
    }
}
