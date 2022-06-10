using System.Collections.Generic;
using UnityEngine;

public class CellConnectionProvider : MonoBehaviour
{
    [Header("Cell item connections")] [Space] [SerializeField]
    private List<GameObject> _cellItemConnections;


    public void ConnectionSetActive(bool setActive, CellSide side) =>
        _cellItemConnections[(int)side - 1].SetActive(setActive);
}