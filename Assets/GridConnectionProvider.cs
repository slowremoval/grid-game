using System.Collections.Generic;
using GridField.Cells;
using UnityEngine;

public class GridConnectionProvider : MonoBehaviour
{
    
    //top - 0
    //down - 1
    //left - 2
    //right - 3

    [SerializeField]SimpleCell _simpleCell;
    
    
    [Header("Placeholder connections")] [Space] [SerializeField]
    private List<GameObject> _placeholderConnections;

    [Header("Cell item connections")] [Space] [SerializeField]
    private List<GameObject> _cellItemrConnections;

    private void Start()
    {
        ShowGridPlaceholderConnections();
    }

    private void ShowGridPlaceholderConnections()
    {
        bool[] neighbours = _simpleCell.HasNeighbours();

        for (var index = 0; index < _placeholderConnections.Count; index++)
        {
            GameObject connectionSide = _placeholderConnections[index];
            
            connectionSide.SetActive(neighbours[index]);
        }
    }
}