using System.Collections;
using System.Collections.Generic;
using GridField.Cells;
using UnityEngine;

public class GridConnectionProvider : MonoBehaviour
{
    //top - 0
    //down - 1
    //left - 2
    //right - 3

    [SerializeField] SimpleCell _simpleCell;


    [Header("Placeholder connections")] [Space] [SerializeField]
    private List<GameObject> _placeholderConnections;

    [Header("Cell item connections")] [Space] [SerializeField]
    private List<GameObject> _cellItemrConnections;

    private float _connectionScaleSpeed;

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
            connectionSide.transform.localScale = Vector3.zero;
            if (neighbours[index])
            {
                connectionSide.SetActive(true);
                StartCoroutine(ActivateConnectionRoutine(connectionSide));
            }
        }
    }

    private IEnumerator ActivateConnectionRoutine(GameObject connectionSide)
    {
        while (connectionSide.transform.localScale.x < 1)
        {
            yield return null;

            _connectionScaleSpeed = 5f;
            connectionSide.transform.localScale =
                Vector3.Lerp(connectionSide.transform.localScale, new Vector3(1.05f, 1, 1),
                    Time.deltaTime * _connectionScaleSpeed);
        }
    }
}