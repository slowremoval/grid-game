using System.Collections;
using System.Collections.Generic;
using GridField.Cells;
using UnityEngine;

public class PlaceholderConnectionProvider : MonoBehaviour
{
    [SerializeField] GridCell _cell;

    private float _connectionScaleSpeed;

    [Header("Placeholder connections")] [Space] [SerializeField]
    private List<GameObject> _placeholderConnections;

    private void Start()
    {
        ShowGridPlaceholderConnections();
    }

    private void ShowGridPlaceholderConnections()
    {
        bool[] neighbours = _cell.HasNeighbours();

        for (var index = 0; index < _placeholderConnections.Count; index++)
        {
            GameObject connectionSide = _placeholderConnections[index];
            connectionSide.transform.localScale = Vector3.zero;

            if (neighbours[index])
            {
                connectionSide.SetActive(true);
                StartCoroutine(ActivateConnectionRoutine(connectionSide));
            }
            else
            {
                _cell.UnactiveSides[index] = (CellSide)(index + 1);
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
                Vector3.Lerp(connectionSide.transform.localScale, new Vector3(1.025f, 1.025f, 1),
                    Time.deltaTime * _connectionScaleSpeed);
        }
    }

}