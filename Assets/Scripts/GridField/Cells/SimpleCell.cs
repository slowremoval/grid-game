using System;
using System.Threading;
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