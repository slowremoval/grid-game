using System;
using UnityEngine;

namespace GridField.Cells.Cell_Modifiers
{
    public class RotatingCellModifier : MonoBehaviour, ICellModifier
    {
        [SerializeField] private RotationTrigger _rotationTrigger;
        [SerializeField] private GameObject _placeholderVisualization;
        [SerializeField] private GridCell _cellType;

        private void Start()
        {
            if (_cellType is SimpleCell simpleCell)
            {
                simpleCell.InitializeCellImage();
            }
        }

        private void OnEnable()
        {
            _rotationTrigger.OnRotationTriggerActivated += RotateUnactiveSides;
            _rotationTrigger.OnRotationTriggerActivated += RotateVisualization;
        }

        private void OnDisable()
        {
            _rotationTrigger.OnRotationTriggerActivated -= RotateUnactiveSides;
            _rotationTrigger.OnRotationTriggerActivated -= RotateVisualization;
        }

        private void RotateUnactiveSides()
        {
            int CellSideMemberCount = Enum.GetNames(typeof(CellSide)).Length;

            for (int i = 0; i < _cellType.UnactiveSides.Length; i++)
            {
                if ((int)_cellType.UnactiveSides[i] == 0)
                {
                    continue;
                }

                if ((int)_cellType.UnactiveSides[i] < CellSideMemberCount - 1)
                {
                    _cellType.UnactiveSides[i] = (CellSide)_cellType.UnactiveSides[i] + 1;
                }
                else
                {
                    _cellType.UnactiveSides[i] = (CellSide)1;
                }
            }
        }

        private void RotateVisualization()
        {
            var currentRotation = _placeholderVisualization.transform.rotation.eulerAngles;

            var newRotation = new Vector3(currentRotation.x, currentRotation.y, currentRotation.z - 90);

            _placeholderVisualization.transform.rotation = Quaternion.Euler(newRotation);

            _cellType.SendVisualizationChanged();
        }
    }
}