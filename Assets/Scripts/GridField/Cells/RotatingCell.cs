using System;
using GridField.Cells;
using UnityEngine;

public class RotatingCell : SimpleCell
{
    [SerializeField] private RotationTrigger _rotationTrigger;
    [SerializeField] private GameObject _placeholderVisualization;

    private void Start()
    {
        _rotationTrigger.OnRotationTriggerActivated += RotateUnactiveSides;
        _rotationTrigger.OnRotationTriggerActivated += RotateVisualization;
        base.InitializeCellImage();
    }

    private void RotateUnactiveSides()
    {
        int CellSideMemberCount = Enum.GetNames(typeof(CellSide)).Length;
        
        for (int i = 0; i < UnactiveSides.Length; i++)
        {
            if ((int)UnactiveSides[i] == 0)
            {
                continue;
            }
            
            if ((int)UnactiveSides[i] < CellSideMemberCount - 1)
            {
                UnactiveSides[i] = (CellSide)UnactiveSides[i] + 1;
            }
            else
            {
                UnactiveSides[i] = (CellSide)1;
            }
        }
    }


    private void RotateVisualization()
    {
        var currentRotation = _placeholderVisualization.transform.rotation.eulerAngles;

        var newRotation = new Vector3(currentRotation.x, currentRotation.y, currentRotation.z - 90);
        
        _placeholderVisualization.transform.rotation = Quaternion.Euler(newRotation);
        
        SendVisualizationChanged();
    }
}