using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DynamicGrid : MonoBehaviour
{
    private Dictionary<Vector2, GameObject> dynamicGrid = new Dictionary<Vector2, GameObject>();

    public GridLayout grid;
    [SerializeField]
    private Tilemap obstacles;

    private Vector3 offset;
    // Start is called before the first frame update
    void Start()
    {
        offset = grid.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
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
        //test
        positionIni = worldToDynamicGridCell(positionIni);

        if (!dynamicGrid.ContainsKey(positionEnd))
        {
            //test
            positionEnd = worldToDynamicGridCell(positionEnd);

            dynamicGrid.Add(positionEnd, dynamicGrid[positionIni]);
            dynamicGrid.Remove(positionIni);
        }
        else
            Debug.Log("This is not suppose to happend");
    }

    public Vector2 placeInGrid(Vector2 position, GameObject gameObject)
    {
        //test
        position = worldToDynamicGridCell(position);

        if (!dynamicGrid.ContainsKey(position))
        {
            dynamicGrid.Add(position, gameObject);
            //Debug.Log("position in dynamic grid: " + position);
        }
        else
            Debug.Log(gameObject.name + " - Another item is already on the grid: " + dynamicGrid[position].name);

        return position;
    }

    public void removeFromGrid(Vector2 position)
    {       
        if (dynamicGrid.ContainsKey(position))
        {
            dynamicGrid.Remove(position);
        }
        else
            Debug.Log("Couldn't find object in position " + position);
    }

    public bool cellIsEmpty(Vector2 cell)
    {
        //test
        Vector2 cell1 = cell;
        cell = worldToDynamicGridCell(cell);

        Vector3Int obstacle = new Vector3Int((int)cell.x, (int)cell.y, 0);

        
        if (obstacles.GetTile(obstacle))
            return false;
        else if (dynamicGrid.ContainsKey(cell))
            return false;
        return true;
    }

    public Vector2 worldToDynamicGridCell(Vector2 cell)
    {
        Vector3 aux = grid.WorldToCell((Vector3)cell);
        cell.x = aux.x;
        cell.y = aux.y;
        return cell;
    }
}
