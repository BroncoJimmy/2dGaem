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
    [SerializeField] GameObject[] items;
    [SerializeField] GameObject[] obstacles;
    [SerializeField] GameObject dungeonEntrance;
    public static int roomSizeX = 17;
    public static int roomSizeY = 12;
    List<int[]> checkCoords;



    // Stores the relevant data for each preset room ({North exit, East, South, West}, {tilemapLocation.x, tilemapLocation.y})
    public static IDictionary<bool[], int[,]> presetData = new Dictionary<bool[], int[,]>();
    BoundsInt bounds;

    IDictionary<int[], Room> roomData = new Dictionary<int[], Room>(Comparer);

    

    [SerializeField] float distanceFromSpawn;

    // Uses this distribution -((x/10)-2)^2+5
    //[SerializeField] float SPAWN_MAX { get { return 5 - Mathf.Pow(distanceFromSpawn/20f - 2, 0.5f); } }
    [SerializeField] float spawnRateAvg;
    float spawnRateTotal;
    int totalSpawns;


    int numRoomsLoaded;

    

    // Start is called before the first frame update
    void Start()
    {
        player = Globals.player;
        distanceFromSpawn = 0;
        /*for (int i = 0; i < 100; i++)
        {
            spawnRateTotal += (float) Globals.GenerateSkewedRandomNumber(0, 10, 2);

        }
        Debug.Log(spawnRateTotal / 100);
        spawnRateTotal = 0;
        for (int i = 0; i < 100; i++)
        {
            spawnRateTotal += (float)Globals.GenerateSkewedRandomNumber(0, 10, 5);

        }
        Debug.Log(spawnRateTotal / 100);
        */
        updatePresets();

        bounds = wallPreset.cellBounds;

        buildRoom(new int[] { 0, 0 }, false);
        //generateEnemies(roomData[new int[] { 0, 0 }].floor, new Vector3Int(roomData[new int[] { 0, 0 }].location[0], roomData[new int[] { 0, 0 }].location[1]));

        // Instantiate(enemies[0], player.transform.position, Quaternion.Euler(0, 0, 0));


    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            distanceFromSpawn = player.transform.position.magnitude;
            unloadRooms(player.transform.position);
            foreach (int[] loc in checkRooms(player.transform.position))
            {
                buildRoom(loc, true);
                //Debug.Log("Room loaded: " + loc[0] + ", " + loc[1]);
            }
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
    public Room buildRoom(int[] location, bool hasEnemies)
    {
        numRoomsLoaded++;
        int[] presetCoords = new int[2];
        int[] current = new int[2];
        bool[] newBlueprint = { true, true, true, true };
        Room currentRoom;
        List<int> emptyIndexes = new List<int>();
        int falseCount = 0;
        // Check North
        if (roomData.TryGetValue(new int[] { location[0], location[1] + 1 }, out currentRoom))
        {
            //Debug.Log("Room to the North");
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
            //Debug.Log("Room to the East");
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
            //Debug.Log("Room to the South");
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
            //Debug.Log("Room to the West");
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
            int randInt = (int)(Random.Range(0, 3));
            int randomSpot = Random.Range(0, emptyIndexes.Count);
            // Debug.Log("Random index = " + randomSpot + "; Max: " + emptyIndexes.Count);
            newBlueprint[emptyIndexes[randomSpot]] = (randInt != 1);
            if (randInt != 1)
            {
                falseCount++;
            }
        }

        string roomType = "(";
        foreach (bool i in newBlueprint)
        {
            roomType += " ," + i;
        }
        roomType += " )";
        //Debug.Log(roomType + " generated at " + location);
        // Grabs the set of rooms that fit the parameters and adds a random one of them to presetCoords variable
        foreach (var kvp in presetData)
        {
            if (kvp.Key.SequenceEqual<bool>(newBlueprint))
            {
                // Chooses a random preset that mathces the parameters to be entered into the scene
                int roomChoice = Random.Range(0, presetData[kvp.Key].GetLength(0));
                if (distanceFromSpawn < 1)
                {
                    roomChoice = 0;
                } else
                {
                    roomChoice = Random.Range(0, presetData[kvp.Key].GetLength(0));
                }
                //Debug.Log(roomChoice);
                for (int i = 0; i < 2; i++)
                {
                    presetCoords[i] = presetData[kvp.Key][roomChoice, i];
                }
                
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
        }*/

        currentRoom = new Room(newBlueprint, buildFloor(presetCoords, new Vector3Int(location[0], location[1])), buildWalls(presetCoords, new Vector3Int(location[0], location[1])), location);
        roomData.Add(location, currentRoom);
        generateObstacles(currentRoom.floor, new Vector3Int(location[0], location[1]));
        if (hasEnemies)
        {
            Debug.Log("Location Length = " + location.Length);
            generateEnemies(currentRoom.floor, new Vector3Int(location[0], location[1]));
        } 
        if (presetCoords.SequenceEqual(new int[] {0,2 }) && distanceFromSpawn > 15f)
        {
            Instantiate(dungeonEntrance, new Vector2((location[0] * roomSizeX + 8.5f) * 0.16f, (location[1] * roomSizeY + 5f) * 0.16f), Quaternion.identity);
        }
        generateItems(currentRoom.floor, new Vector3Int(location[0], location[1]));
        //Debug.Log("Generated Enemies at " + location[0] + ", " + location[1]);
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

        //int randomNumEnemies = Random.Range(0, (int)SPAWN_RATE/2) + Random.Range(0, (int)SPAWN_RATE/2);
        /*spawnRateTotal += randomNumEnemies;
        totalNum += 1;
        spawnRateAvg = (spawnRateTotal / totalNum);
        
        for (int index = 0; index < enemySet.Capacity; index++)
        {
            enemySet.Add(enemies[Random.Range(0, enemies.Length)]);
        }*/
        // Pulls a preset of enemies to be placed in the room
        //int[] enemySet = enemyPresets[(int) Globals.GenerateSkewedRandomNumber(0, distanceFromSpawn, 5)];
        List<int> enemySet = new List<int>();
        int SPAWN_VALUE = 1 + (int) ((distanceFromSpawn + 2) / 5);
        for (int i = 0; i < 3; i++)
        {
            int randEnemy = Random.Range(0, 4);
            if (randEnemy > 0 && randEnemy <= SPAWN_VALUE)
            {
                SPAWN_VALUE -= randEnemy;
                enemySet.Add(randEnemy);
            }
        }
        string set = "(";
        for (int i = 0; i < enemySet.Count; i++)
        {
            set += enemySet[i] + " ";
        }
        //Debug.Log("Generated " + set + " at " + location);

        for (int row = 3; row < roomSizeY - 3; row++)
        {
            for (int column = 3; column < roomSizeX - 3; column++)
            {
                if (preset[(roomSizeX * row) + column] != null && !WaterBehaviour.waterTiles.Contains(new Vector2Int(location.x * roomSizeX + column, (location.y * roomSizeY + row))) && Random.Range(0, 10) == 5)
                {
                    Vector3 newLocation = new Vector3((location.x * roomSizeX + column) * .16f + 0.08f, (location.y * roomSizeY + row) * .16f + 0.08f);
                    //Debug.Log("Enemy generated at " + newLocation + ", Coords (" + column + ", " + row);
                    EnemyGeneration.generateEnemy(enemies[enemySet[0]-1], newLocation);
                    enemySet.RemoveAt(0);
                }
                if (enemySet.Count <= 0)
                {
                    break;
                }
            }
            if (enemySet.Count <= 0)
            {
                break;
            }
        }

    }

    private void generateItems(TileBase[] preset, Vector3Int location)
    {
        // Creates a list of enemies to be placed in the room (max 2)
        List<GameObject> itemSet = new List<GameObject>(Random.Range(0, 2));
        for (int index = 0; index < itemSet.Capacity; index++)
        {
            itemSet.Add(items[Random.Range(0, items.Length)]);
        }
        for (int row = Random.Range(2, roomSizeY - 4); row < roomSizeY - 2; row++)
        {
            if (itemSet.Count == 0)
            {
                break;
            }
            for (int column = Random.Range(3, roomSizeY - 5); column < roomSizeX - 3; column++)
            {
                if (preset[(roomSizeX * row) + column] != null && Random.Range(0, 10) == 5)
                {
                    Vector3 newLocation = new Vector3((location.x * roomSizeX + column) * .16f - 0.08f, (location.y * roomSizeY + row) * .16f - 0.08f);
                    //Debug.Log("New Item at " + newLocation + ", Coords (" + column + ", " + row);
                    EnemyGeneration.generateItem(itemSet[0], newLocation);
                    itemSet.RemoveAt(0);
                }
                if (itemSet.Count == 0)
                {
                    break;
                }
            }

        }

    }

    private void generateObstacles(TileBase[] preset, Vector3Int location)
    {
        // Creates a list of enemies to be placed in the room (max 2)
        List<Vector2Int> waterTiles = new List<Vector2Int>();
        List<GameObject> obstacleSet = new List<GameObject>(Random.Range(0, 10));
        for (int index = 0; index < obstacleSet.Capacity; index++)
        {
            obstacleSet.Add(obstacles[Random.Range(0, obstacles.Length)]);
        }
        for (int item = 0; item < obstacleSet.Count; item++)
        {
            int randomRow = Random.Range(2, roomSizeY - 4);
            int randomColumn = Random.Range(3, roomSizeY - 5);
            // Checks to see if it can be placed on this random tile
            if (preset[(roomSizeX * randomRow) + randomColumn] != null)
            {
                Vector3 newLocation = new Vector3((location.x * roomSizeX + randomColumn) * .16f - 0.08f, (location.y * roomSizeY + randomRow) * .16f - 0.08f);
                EnemyGeneration.generateObstacle(obstacleSet[0], newLocation);
                obstacleSet.RemoveAt(0);
            }
            else
            {
                // Reload the same item
                item--;
            }
        }
        /*
        for (int row = Random.Range(2, roomSizeY - 4); row < roomSizeY - 2; row++)
        {
            if (itemSet.Count == 0)
            {
                break;
            }
            for (int column = Random.Range(3, roomSizeY - 5); column < roomSizeX - 3; column++)
            {
                if (preset[(roomSizeX * row) + column] != null && Random.Range(0, 10) == 5)
                {
                    Vector3 newLocation = new Vector3((location.x * roomSizeX + column) * .16f - 0.08f, (location.y * roomSizeY + row) * .16f - 0.08f);
                    //Debug.Log("New Item at " + newLocation + ", Coords (" + column + ", " + row);
                    EnemyGeneration.generateItem(itemSet[0], newLocation);
                    itemSet.RemoveAt(0);
                }
                if (itemSet.Count == 0)
                {
                    break;
                }
            }

        }
        */
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
                //Debug.Log("Room deleted");
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
        presetData.Add(new bool[] { true, true, false, false }, new int[,] { { 0, 0 } });
        presetData.Add(new bool[] { false, false, true, true }, new int[,] { { 0, 1 } });
        presetData.Add(new bool[] { true, true, true, true }, new int[,] { { 1, 0 } , { 0, 2 } });
        presetData.Add(new bool[] { true, true, true, false }, new int[,] { { 1, 1 } });
        presetData.Add(new bool[] { false, true, true, false }, new int[,] { { 1, 2 } });
        presetData.Add(new bool[] { false, true, false, true }, new int[,] { { 2, 0 } });
        presetData.Add(new bool[] { true, true, false, true }, new int[,] { { 2, 1 } });
        presetData.Add(new bool[] { false, true, true, true }, new int[,] { { 2, 2 } });
        presetData.Add(new bool[] { true, false, false, true }, new int[,] { { 3, 0 } });
        presetData.Add(new bool[] { true, false, true, true }, new int[,] { { 3, 1 } });
        presetData.Add(new bool[] { true, false, true, false }, new int[,] { { 3, 2 } });

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
}



//To get a specific row or column from the multidimensional array you can use some LINQ:

public class CustomArray<T>
{
    public T[] GetColumn(T[,] matrix, int columnNumber)
    {
        return Enumerable.Range(0, matrix.GetLength(0))
                .Select(x => matrix[x, columnNumber])
                .ToArray();
    }

    public T[] GetRow(T[,] matrix, int rowNumber)
    {
        return Enumerable.Range(0, matrix.GetLength(1))
                .Select(x => matrix[rowNumber, x])
                .ToArray();
    }
}