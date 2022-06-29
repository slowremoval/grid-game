using System.Collections.Generic;
using GridField;
using GridField.Cells;
using UnityEngine;

public class CellConnectionProvider : MonoBehaviour
{
    [Header("Cell item connections")] [Space] [SerializeField]
    private List<CellConnection> _cellItemConnections;
    
    public void ConnectionSetActive(bool setActive, CellSide side, CellType type = CellType.simple)
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

    public void DeactivateHorizontalConnections()
    {
        ConnectionSetActive(false, CellSide.left);
        ConnectionSetActive(false, CellSide.right);
    }

    public void DeactivateVerticalConnections()
    {
        ConnectionSetActive(false, CellSide.top);
        ConnectionSetActive(false, CellSide.down);
    } 
}