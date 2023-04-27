using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    Vector3 startPos;
    public float fireRange = 0.2f;
    [SerializeField] GameObject splatter;
    [SerializeField] GameObject hitEffect;

    Quaternion newAngle;
    public int damageAmount { get; set; }

    private void Start()
    {
        startPos = transform.position;
        newAngle.eulerAngles = new Vector3(0, 0, transform.rotation.eulerAngles.z + 180f);

        Invoke("Delete", fireRange);

    }
    private void Update()
    {
        //Debug.DrawRay(transform.position, Vector3.ClampMagnitude((startPos - transform.position), 0.5f), Color.white);
        if (Input.GetKeyDown(KeyCode.K))
        {
            Debug.Log("Damage: " + damageAmount);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        newAngle.eulerAngles = new Vector3(0, 0, transform.rotation.eulerAngles.z);
        if (collision.gameObject.tag.Equals("Enemy"))
        {
            GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraShake>().Shake(.3f);
            GameObject effect = Instantiate(hitEffect, transform.position, newAngle);
            effect.GetComponent<SpriteRenderer>().color = GetComponent<SpriteRenderer>().color;
            Destroy(effect, 0.25f);
            collision.gameObject.SendMessage("wasHit", gameObject);
            //GetComponent<EnemyScript>().wasHit(gameObject);

            newAngle.eulerAngles = new Vector3(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z + Random.Range(160, 200));
            Instantiate(splatter, transform.position, newAngle);
            Debug.Log("Layer hit: " + collision.gameObject.layer);
            Destroy(gameObject);


        }
        else if (!collision.gameObject.layer.Equals(Globals.ITEM_LAYER))
        {
            GameObject effect = Instantiate(hitEffect, transform.position + new Vector3(Globals.lookDirection.x * 0.1f, Globals.lookDirection.y * 0.1f, -5f), newAngle);
            effect.GetComponent<SpriteRenderer>().color = GetComponent<SpriteRenderer>().color;
            Destroy(effect, 0.25f);
            Debug.Log("Layer hit: " + collision.gameObject.layer);
            Destroy(gameObject);
        }
        else if (collision.gameObject.tag.Equals("Crate"))
        {
            Debug.Log(" siugd my peen ");
            collision.gameObject.SendMessage("damageTaken", damageAmount);
            //GetComponent<EnemyScript>().wasHit(gameObject);

            // Debug.Log("Layer hit: " + collision.gameObject.layer);
            Destroy(gameObject);


        }
        Debug.Log(collision.gameObject.tag);


    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.layer.Equals(Globals.PLAYER_LAYER) && !Globals.player.GetComponent<DashAbility>().isDashing)
        {
            collision.gameObject.SendMessage("damageTaken", damageAmount);

            GameObject effect = Instantiate(hitEffect, transform.position + transform.up * 0.1f, Quaternion.Euler(0, 0, Random.Range(0, 180)));
            effect.GetComponent<SpriteRenderer>().color = GetComponent<SpriteRenderer>().color;
            Destroy(effect, effect.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);
            Destroy(gameObject);
        }


    }

    void Delete()
    {
        GameObject effect = Instantiate(hitEffect, transform.position + transform.up * 0.1f, Quaternion.Euler(0, 0, Random.Range(0, 180)));
        effect.GetComponent<SpriteRenderer>().color = GetComponent<SpriteRenderer>().color;
        Destroy(effect, effect.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);
        Destroy(gameObject);
    }

    /*public static void onHit(Collision2D collision, GameObject bullet)
    {
        if (collision.gameObject.tag.Equals("Enemy"))
        {
            collision.gameObject.GetComponent<EnemyScript>().wasHit(bullet);
            // newAngle.eulerAngles = new Vector3;
            Instantiate(splatter, bullet.transform.position, Quaternion.Euler(bullet.transform.rotation.eulerAngles.x, bullet.transform.rotation.eulerAngles.y, bullet.transform.rotation.eulerAngles.z + Random.Range(160, 200)));
        }

        //Debug.Log(collision.gameObject.name);
        //newAngle.eulerAngles = new Vector3;

        GameObject effect = Instantiate(hitEffect, bullet.transform.position + new Vector3(Globals.lookDirection.x * 0.1f, Globals.lookDirection.y * 0.1f), Quaternion.Euler(0, 0, bullet.transform.rotation.eulerAngles.z));
        Destroy(effect, 0.25f);
    }*/

}
/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGeneration : MonoBehaviour
{
    public static IDictionary<Vector3, GameObject> unloadedEnemies = new Dictionary<Vector3, GameObject>();
    public static List<GameObject> loadedEnemies = new List<GameObject>();

    [SerializeField] float renderDistance = 5;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        removeEnemies(Globals.player.transform.position);
    }

    public static void generateEnemy(GameObject enemy, Vector3 newLocation)
    {
        loadedEnemies.Add(Instantiate(enemy, newLocation, Quaternion.Euler(0, 0, 0)));
    }

    private void removeEnemies(Vector3 loadPoint)
    {
        //List<Vector3> removeKeys = new List<Vector3>();
        for (int instance = 0; instance < loadedEnemies.Count; instance++)
        {

            if (loadedEnemies[instance].GetComponent<EnemyScript>().isLoaded && (loadedEnemies[instance].transform.position - loadPoint).magnitude > renderDistance)
            {
                Debug.Log("Unloaded enemy at " + loadedEnemies[instance].transform.position + ", Load point: " + loadPoint);
                //removeKeys.Add(instance.Key);
                unloadedEnemies.Add(loadedEnemies[instance].transform.position, loadedEnemies[instance]);
                Destroy(loadedEnemies[instance]);
                loadedEnemies.RemoveAt(instance);
                instance--;

            }
            else
            {
                //Debug.Log(loadedEnemies[instance].GetComponent<EnemyScript>().disMagnitude);
            }
        }
        /*foreach (Vector3 key in removeKeys)
        {
            enemies.Add(enemies[key].transform.position, enemies[key]);
            enemies[key].GetComponent<EnemyScript>().unload();
            enemies.Remove(key);
            
        }
    }

}
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Room
{
    public bool isLoaded;
    public TileBase[] floor;
    public TileBase[] walls;
    public bool[] roomType;
    public int[] location;
    
    //
    public Room(bool[] parameters, TileBase[] f, TileBase[] w, int[] loc)
    {
        isLoaded = true;
        floor = f;
        walls = w;
        roomType = parameters;
        location = loc;

    }
}

public class TilemapGeneration : MonoBehaviour
{
    public static IEqualityComparer<int[]> Comparer { get; } = new IntArrEqualityComparer();
    [SerializeField] GameObject player;
    [SerializeField] Tilemap floorMap;
    [SerializeField] Tilemap wallMap;
    [SerializeField] Tilemap floorPreset;
    [SerializeField] Tilemap wallPreset;
    [SerializeField] TileBase[] floorTiles;
    [SerializeField] GameObject[] enemies;
    int roomSizeX = 17;
    int roomSizeY = 12;
    List<int[]>checkCoords;



    // Stores the relevant data for each preset room ({North exit, East, South, West}, {tilemapLocation.x, tilemapLocation.y})
    public static IDictionary<bool[], int[]> presetData = new Dictionary<bool[], int[]>(); 
    BoundsInt bounds;


    //List<int[]> roomKeys = new List<int[]>();
    //List<Room> rooms = new List<Room>();
    Room newRoom;

    IDictionary<int[], Room> roomData = new Dictionary<int[], Room>(Comparer);

    /* IDictionary<int[], TileBase[]> storedRooms = new Dictionary<int[], TileBase[]>();
    // Stores a list of coordinates (1 unit = 1 room) where rooms have been generated
    List<int[]> roomsLoc = new List<int[]>();
    // Stores the arrays of tiles for each room that has been generated
    List<TileBase[]> storedRooms = new List<TileBase[]>();
    // Keeps track of the room layouts at each point
    List<int[]> roomTypes = new List<int[]>();
    

// Start is called before the first frame update
void Start()
{


    updatePresets();

    bounds = wallPreset.cellBounds;

    buildRoom(new int[] { 0, 0 });
    //generateEnemies(roomData[new int[] { 0, 0 }].floor, new Vector3Int(roomData[new int[] { 0, 0 }].location[0], roomData[new int[] { 0, 0 }].location[1]));

    // Instantiate(enemies[0], player.transform.position, Quaternion.Euler(0, 0, 0));


}

// Update is called once per frame
void Update()
{
    unloadRooms(player.transform.position);
    foreach (int[] loc in checkRooms(player.transform.position))
    {
        buildRoom(loc);
        Debug.Log("Room loaded: " + loc[0] + ", " + loc[1]);
    }

}

public List<int[]> checkRooms(Vector3 playerLoc)
{
    checkCoords = new List<int[]>();
    int[,] checkBounds = { { 1, 0 }, { -1, 0 }, { 0, 1 }, { 0, -1 }, { 1, 1 }, { 1, -1 }, { -1, -1 }, { -1, 1 } };

    if (Input.GetKeyDown(KeyCode.Space))
    {
        Debug.Log("x: " + playerLoc.x + ", y: " + playerLoc.y);
    }

    for (int i = 0; i < checkBounds.GetLength(0); i++)
    {
        checkCoords.Add(new int[] { (int)((playerLoc.x - 1.36f) / (roomSizeX * .16f)) + checkBounds[i, 0], (int)((playerLoc.y - 0.96f) / (roomSizeY * .16f)) + checkBounds[i, 1] });
    }


    for (int loc = 0; loc < checkCoords.Count; loc++)
    {
        if (roomData.ContainsKey(checkCoords[loc]))
        {
            if (!roomData[checkCoords[loc]].isLoaded)
            {
                reloadRoom(roomData[checkCoords[loc]]);
            }
            checkCoords.RemoveAt(loc);
            loc--;
        }
    }

    return checkCoords;
}

// Takes in a location in units of room size e.g.{1,4} and builds a random room at that location based off of parameters.
public Room buildRoom(int[] location)
{
    int[] presetCoords = new int[2];
    int[] current = new int[2];
    bool[] newBlueprint = { true, true, true, true };
    Room currentRoom;
    List<int> emptyIndexes = new List<int>();
    int falseCount = 0;
    // Check North
    if (roomData.TryGetValue(new int[] { location[0], location[1] + 1 }, out currentRoom))
    {
        Debug.Log("Room to the North");
        if (currentRoom.roomType[2])
        {
            newBlueprint[0] = true;
        }
        else
        {
            newBlueprint[0] = false;
            falseCount++;
        }

    }
    else { emptyIndexes.Add(0); }
    // Check East
    if (roomData.TryGetValue(new int[] { location[0] + 1, location[1] }, out currentRoom))
    {
        Debug.Log("Room to the East");
        if (currentRoom.roomType[3])
        {
            newBlueprint[1] = true;
        }
        else
        {
            newBlueprint[1] = false;
            falseCount++;
        }

    }
    else { emptyIndexes.Add(1); }
    // Check South
    if (roomData.TryGetValue(new int[] { location[0], location[1] - 1 }, out currentRoom))
    {
        Debug.Log("Room to the South");
        if (currentRoom.roomType[0])
        {
            newBlueprint[2] = true;
        }
        else
        {
            newBlueprint[2] = false;
            falseCount++;
        }

    }
    else { emptyIndexes.Add(2); }
    // Check West
    if (roomData.TryGetValue(new int[] { location[0] - 1, location[1] }, out currentRoom))
    {
        Debug.Log("Room to the West");
        if (currentRoom.roomType[1])
        {
            newBlueprint[3] = true;
        }
        else
        {
            newBlueprint[3] = false;
            falseCount++;
        }

    }
    else { emptyIndexes.Add(3); }

    while (falseCount < 2)
    {
        int randInt = (int)(Random.Range(0, 2));
        newBlueprint[emptyIndexes[(int)(Random.Range(0, emptyIndexes.Count) - 0.000001f)]] = (randInt == 1);
        if (randInt != 1)
        {
            falseCount++;
        }
    }

    foreach (var kvp in presetData)
    {

        if (kvp.Key.SequenceEqual<bool>(newBlueprint))
        {
            presetCoords = presetData[kvp.Key];
        }

    }

    /*TileBase[] wallBlueprint = buildWalls(presetCoords, new Vector3Int(location[0], location[1]));
    for (int t = 0; t < tiles.Length; t++)
    {
        tiles[t] = wallBlueprint[t];
    }
    TileBase[] floorBlueprint = buildFloor(presetCoords, new Vector3Int(location[0], location[1]));
    for (int t = 0; t < tiles.Length; t++)
    {
        if (tiles[t] == null)
        {
            tiles[t] = floorBlueprint[t];
        }
    }

    currentRoom = new Room(newBlueprint, buildFloor(presetCoords, new Vector3Int(location[0], location[1])), buildWalls(presetCoords, new Vector3Int(location[0], location[1])), location);
    roomData.Add(location, currentRoom);
    generateEnemies(currentRoom.floor, new Vector3Int(location[0], location[1]));
    Debug.Log("Generated Enemies at " + location[0] + ", " + location[1]);
    return currentRoom;
}



// Builds the walls of the preset at index (presetPoint) at (location) on the Walls tilemap.
public TileBase[] buildWalls(int[] presetPoint, Vector3Int location)
{
    TileBase[] preset = wallPreset.GetTilesBlock(new BoundsInt(bounds.xMin + 1 + (presetPoint[0] * roomSizeX), bounds.yMin + (presetPoint[1] * roomSizeY), 0, roomSizeX, roomSizeY, 1));
    for (int row = 0; row < 12; row++)
    {
        for (int column = 0; column < 17; column++)
        {
            wallMap.SetTile(new Vector3Int(column + (location.x * roomSizeX), row + (location.y * roomSizeY), 0), preset[(roomSizeX * row) + column]);
        }
    }
    return preset;
}

public TileBase[] buildFloor(int[] presetPoint, Vector3Int location)
{
    TileBase[] preset = floorPreset.GetTilesBlock(new BoundsInt(bounds.xMin + 1 + (presetPoint[0] * roomSizeX), bounds.yMin + (presetPoint[1] * roomSizeY), 0, roomSizeX, roomSizeY, 1));

    for (int row = 0; row < 12; row++)
    {
        for (int column = 0; column < 17; column++)
        {
            if (preset[(roomSizeX * row) + column] != null)
            {
                preset[(roomSizeX * row) + column] = randFloorTile();
                floorMap.SetTile(new Vector3Int(column + (location.x * roomSizeX), row + (location.y * roomSizeY), 0), preset[(roomSizeX * row) + column]);
            }


        }
    }
    return preset;
}

private TileBase randFloorTile()
{
    int[] weights = new int[] { 70, 10, 10, 10 };
    int randNum = Random.Range(0, 100);
    int newPercent = 0;
    for (int i = 0; i < weights.Length; i++)
    {
        newPercent += weights[i];
        if (randNum < newPercent)
        {
            //Debug.Log(i);
            return floorTiles[i];
        }
    }
    return floorTiles[0];
}

private void generateEnemies(TileBase[] preset, Vector3Int location)
{
    // Creates a list of enemies to be placed in the room (max 2)
    List<GameObject> enemySet = new List<GameObject>(Random.Range(1, 3));
    for (int index = 0; index < enemySet.Capacity; index++)
    {
        enemySet.Add(enemies[Random.Range(0, enemies.Length)]);
    }
    for (int row = 2; row < roomSizeY - 2; row++)
    {
        for (int column = 3; column < roomSizeX - 3; column++)
        {
            if (preset[(roomSizeX * row) + column] != null && Random.Range(0, 10) == 5)
            {
                Vector3 newLocation = new Vector3((location.x * roomSizeX + column) * .16f, (location.y * roomSizeY + row) * .16f);
                Debug.Log("New Enemy at " + newLocation + ", Coords (" + column + ", " + row);
                EnemyGeneration.generateEnemy(enemySet[0], newLocation);
                enemySet.RemoveAt(0);
            }
            if (enemySet.Count == 0)
            {
                break;
            }
        }
        if (enemySet.Count == 0)
        {
            break;
        }
    }

}

public void reloadRoom(Room room)
{

    for (int row = 0; row < 12; row++)
    {
        for (int column = 0; column < 17; column++)
        {

            if (room.walls[(roomSizeX * row) + column] != null)
            {
                wallMap.SetTile(new Vector3Int(column + (room.location[0] * roomSizeX), row + (room.location[1] * roomSizeY), 0), room.walls[(roomSizeX * row) + column]);
            }
            else
            {
                floorMap.SetTile(new Vector3Int(column + (room.location[0] * roomSizeX), row + (room.location[1] * roomSizeY), 0), room.floor[(roomSizeX * row) + column]);
            }
        }
    }

    room.isLoaded = true;
}

public void unloadRooms(Vector3 playerLoc)
{

    foreach (var kvp in roomData)
    {
        if ((new Vector3((int)((playerLoc.x - 1.36f) / (roomSizeX * .16f)), (int)((playerLoc.y - 0.96f) / (roomSizeY * .16f))) - new Vector3(kvp.Value.location[0], kvp.Value.location[1])).magnitude > 2 && kvp.Value.isLoaded)
        {
            Debug.Log("Room deleted");
            for (int row = 0; row < 12; row++)
            {
                for (int column = 0; column < 17; column++)
                {
                    floorMap.SetTile(new Vector3Int(column + (kvp.Value.location[0] * roomSizeX), row + (kvp.Value.location[1] * roomSizeY), 0), null);
                    wallMap.SetTile(new Vector3Int(column + (kvp.Value.location[0] * roomSizeX), row + (kvp.Value.location[1] * roomSizeY), 0), null);
                    kvp.Value.isLoaded = false;
                }
            }
        }
    }
}

// Initializes the presetData Dictionary with values ({North exit?, East?, South?, West?}, {tilemapLocation.x, tilemapLocation.y})
public void updatePresets()
{
    presetData.Add(new bool[] { true, true, false, false }, new int[] { 0, 0 });
    presetData.Add(new bool[] { false, false, true, true }, new int[] { 0, 1 });
    presetData.Add(new bool[] { true, true, true, true }, new int[] { 1, 0 });
    presetData.Add(new bool[] { true, true, true, false }, new int[] { 1, 1 });
    presetData.Add(new bool[] { false, true, true, false }, new int[] { 1, 2 });
    presetData.Add(new bool[] { false, true, false, true }, new int[] { 2, 0 });
    presetData.Add(new bool[] { true, true, false, true }, new int[] { 2, 1 });
    presetData.Add(new bool[] { false, true, true, true }, new int[] { 2, 2 });
    presetData.Add(new bool[] { true, false, false, true }, new int[] { 3, 0 });
    presetData.Add(new bool[] { true, false, true, true }, new int[] { 3, 1 });
    presetData.Add(new bool[] { true, false, true, false }, new int[] { 3, 2 });
}


    
}

public class IntArrEqualityComparer : EqualityComparer<int[]>
{
    public override bool Equals(int[] x, int[] y)
    {
        bool equals = true;
        for (int index = 0; index < x.Length; index++)
        {
            if (!(x[index] == y[index]))
            {
                equals = false;
                break;
            }
        }
        return (equals);
    }

    public override int GetHashCode(int[] array)
    {
        int hc = array.Length;
        foreach (int val in array)
        {
            hc = unchecked(hc * 17 + val);
        }
        return hc;
    }
}*/
