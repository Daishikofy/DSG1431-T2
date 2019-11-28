using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DynamicGrid : MonoBehaviour
{
    private List<Tilemap> obstacles;
    private Dictionary<Vector2, GameObject> dynamicGrid = new Dictionary<Vector2, GameObject>();

    // Start is called before the first frame update

    public void clear()
    {
        obstacles = null;
        dynamicGrid = new Dictionary<Vector2, GameObject>();
    }
    public void loadObstacles()
    {
        obstacles = new List<Tilemap>();
        Tilemap[] maps = FindObjectsOfType<Tilemap>();
        for (int i = 0; i < maps.Length; i++)
        {
            if (maps[i].CompareTag("Obstacles"))
            {
                obstacles.Add(maps[i]);
            }
        }
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
        position.x = (int)position.x;
        position.y = (int)position.y;
        if (!dynamicGrid.ContainsKey(position))
        {
            dynamicGrid.Add(position, gameObject);
            Debug.Log("position in dynamic grid: " + position + " - " + gameObject.name);
        }
        else
            Debug.LogError(gameObject.name + " - Another item is already on the grid: " + dynamicGrid[position].name);

        return position;
    }

    public void removeFromGrid(Vector2 position)
    {
        position.x = (int)position.x;
        position.y = (int)position.y;
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

        if (dynamicGrid.ContainsKey(cell))
            return false;
        if (obstacles != null)
            foreach (var map in obstacles)
            {
                if (map.GetTile(obstacle))
                    return false;           
            }    
        return true;
    }
}
