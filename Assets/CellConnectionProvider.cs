using System.Collections.Generic;
using GridField.Cells;
using UnityEngine;

public class CellConnectionProvider : MonoBehaviour
{
    [Header("Cell item connections")] [Space] [SerializeField]
    private List<CellConnection> _cellItemConnections;
    
    public void ConnectionSetActive(bool setActive, CellSide side, CellType type)
    {
        switch (setActive)
        {
            case true:
                _cellItemConnections[(int)side - 1].ActivateConnection(type);
                break;
            case false:
                _cellItemConnections[(int)side - 1].DeactivateConnectionUrgently();
                break;
        }
    }
}