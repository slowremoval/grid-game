using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GridField.Cells
{
    public class SimpleCell : GridCell
    {
        [SerializeField] private GameObject _visualization;

        [SerializeField] private Color _defaultColor;
        [SerializeField] private Color _firstColor;
        [SerializeField] private Color _secondColor;

        public CellConnectionProvider CellConnectionProvider;

        private Image _cellImage;
        
        public event Action OnColorChanged;

        private void Start()
        {
            _cellImage = _visualization.GetComponent<Image>();
        }
        
        public override void OnPointerDown(PointerEventData eventData)
        {
            SetColorByClick();
        }

        private void SetColorByClick()
        {
            if (Input.GetMouseButton(0))
            {
                if (_cellImage.color == _firstColor)
                {
                    _cellImage.color = _defaultColor;
                    CellType = CellType.empty;
                }
                else
                {
                    _cellImage.color = _firstColor;
                    CellType = CellType.light;
                }
            }
            else if (Input.GetMouseButton(1))
            {
                if (_cellImage.color == _secondColor)
                {
                    _cellImage.color = _defaultColor;
                    CellType = CellType.empty;
                }
                else
                {
                    _cellImage.color = _secondColor;
                    CellType = CellType.dark;
                }
            }

            OnColorChanged?.Invoke();
        }
    }
}

public class RotatingCell : MonoBehaviour
{
    public CellConnectionProvider CellConnectionProvider;

    //[SerializeField] private RotationTrigger _rotationTrigger;
    [SerializeField] private GameObject _placeholderVisualization;

    private void Start()
    {
    //    _rotationTrigger.OnRotationTriggerActivated += RotateVisualization;
    }

    private void RotateVisualization()
    {
        var currentRotation = _placeholderVisualization.transform.rotation.eulerAngles;

        var newRotation = new Vector3(currentRotation.x, currentRotation.y, currentRotation.z - 90);
        
        _placeholderVisualization.transform.rotation = Quaternion.Euler(newRotation);
    }
}