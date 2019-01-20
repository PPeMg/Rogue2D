using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour {

    public int columns = 8;
    public int rows = 8;

    public GameObject[] floorTiles, outerWallTiles, wallTiles, foodTiles, enemys;
    public GameObject exit;

    private Transform boardHolder, objectHolder;
    private List<Vector2> gridPositions = new List<Vector2>();

    void InitializeList()
    {
        gridPositions.Clear();
        for (int x = 1; x < columns - 1; x++)
        {
            for (int y = 1; y < columns - 1; y++)
            {
                gridPositions.Add(new Vector2(x, y));
            }
        }
    }

    Vector2 RandomPosition()
    {
        int randomIndex = Random.Range(0, gridPositions.Count);
        Vector2 randomPosition = gridPositions[randomIndex];
        gridPositions.RemoveAt(randomIndex);

        return randomPosition;
    }

    void LayoutObjectAtRandom(GameObject[] objectArray, int min, int max, Transform holder = null)
    {
        int objectCount = Random.Range(min, max + 1);

        for(int i = 0; i < objectCount; i++)
        {
            Vector2 position = RandomPosition();
            GameObject objectSelected = GetRandomInArray(objectArray);

            GameObject instance = Instantiate(objectSelected, position, Quaternion.identity);

            if(holder != null)
            {
                instance.transform.SetParent(holder);
            }
        }
    }

    public void SetupScene(int level)
    {
        objectHolder = new GameObject("Objects").transform;
        Debug.Log("Seting Setup in Scene...");
        BoardSetup();
        InitializeList();
        LayoutObjectAtRandom(wallTiles, 5, 9, objectHolder);
        LayoutObjectAtRandom(foodTiles, 1, 5, objectHolder);

        int enemyCount = (int) Mathf.Log((float) level, 2f);
        LayoutObjectAtRandom(enemys, enemyCount, enemyCount);
        Debug.Log("Finished");
    }

    void BoardSetup()
    {
        boardHolder = new GameObject("Board").transform;
        for (int x = -1; x < columns + 1; x++)
        {
            for (int y = -1; y < columns + 1; y++)
            {
                GameObject toInstantiate = ((x == -1) || (y == -1) || (x == rows) || (y == columns)) ? GetRandomInArray(outerWallTiles) : GetRandomInArray(floorTiles);

                GameObject instance = Instantiate(toInstantiate, new Vector2(x, y), Quaternion.identity);

                instance.transform.SetParent(boardHolder);
            }
        }
    }

    GameObject GetRandomInArray(GameObject[] array)
    {
        return array[Random.Range(0, array.Length)];
    }
}
