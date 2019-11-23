using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DynamicGrid : MonoBehaviour
{
    
    public GridLayout grid;

    private Tilemap obstacles;
    private Dictionary<Vector2, GameObject> dynamicGrid = new Dictionary<Vector2, GameObject>();

    [SerializeField]
    private string colliderMap = "Obstacles";
    // Start is called before the first frame update

    public void loadGrid()
    {
        Tilemap[] maps = FindObjectsOfType<Tilemap>();
        Debug.Log("Maps: " + maps.Length);
        for (int i = 0; i < maps.Length; i++)
        {
            Debug.Log("Map: " + maps[i].name);
            if (maps[i].name.Contains(colliderMap))
            {
                obstacles = maps[i];
                grid = obstacles.GetComponentInParent<GridLayout>();
            }
        }
        /*
        if (obstacles == null)
        {
            Debug.Log("Need to create a grid");
            GameObject newGrid = new GameObject("DynamicGrid");
            newGrid.AddComponent<GridLayout>();
            grid = newGrid.GetComponent<GridLayout>();
            Instantiate(newGrid);

            GameObject newMap = new GameObject(colliderMap);
            newMap.AddComponent<Tilemap>();
            obstacles = newMap.GetComponent<Tilemap>();
            Instantiate(newMap, grid.transform);
        }
        */
    }

    // Only use getInCell when previously used cellIsEmpty
    public GameObject getInCell(Vector2 cell)
    {
        if (dynamicGrid.ContainsKey(cell))
        {
            return dynamicGrid[cell].gameObject;
        }
        else
            return null;
    }
    public void moveInGrid (Vector2 positionIni, Vector2 positionEnd)
    {

        if (!dynamicGrid.ContainsKey(positionEnd))
        {
            dynamicGrid.Add(positionEnd, dynamicGrid[positionIni]);
            dynamicGrid.Remove(positionIni);
        }
        else
            Debug.LogError("This is not suppose to happend");
    }

    public Vector2 placeInGrid(Vector2 position, GameObject gameObject)
    {
        if (!dynamicGrid.ContainsKey(position))
        {
            dynamicGrid.Add(position, gameObject);
            //Debug.Log("position in dynamic grid: " + position);
        }
        else
            Debug.LogError(gameObject.name + " - Another item is already on the grid: " + dynamicGrid[position].name);

        return position;
    }

    public void removeFromGrid(Vector2 position)
    {
        if (dynamicGrid.ContainsKey(position))
        {
            dynamicGrid.Remove(position);
        }
        else
            Debug.LogError("Couldn't find object in position " + position);
    }

    public bool cellIsEmpty(Vector2 cell)
    {
        Vector3Int obstacle = new Vector3Int((int)cell.x, (int)cell.y, 0);

        
        if (obstacles.GetTile(obstacle))
            return false;
        else if (dynamicGrid.ContainsKey(cell))
            return false;
        return true;
    }
}
