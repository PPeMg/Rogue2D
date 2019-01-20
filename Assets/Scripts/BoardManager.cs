using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour {

    public int columns = 8;
    public int rows = 8;

    public GameObject[] floorTiles, outerWallTiles;

    private Transform boardHolder;

    public void SetupScene()
    {
        Debug.Log("Soy SetupScene()");
        BoardSetup();
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
