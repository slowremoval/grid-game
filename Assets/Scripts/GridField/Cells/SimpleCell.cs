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


        private Image _cellImage;

        public CellType CellColor { get; private set; }

        public event Action OnColorChanged;
        
        private void Awake()
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
                    CellColor = CellType.empty;
                }
                else
                {
                    _cellImage.color = _firstColor;
                    CellColor = CellType.light;
                }
            }
            else if (Input.GetMouseButton(1))
            {
                if (_cellImage.color == _secondColor)
                {
                    _cellImage.color = _defaultColor;
                    CellColor = CellType.empty;
                }
                else
                {
                    _cellImage.color = _secondColor;
                    CellColor = CellType.dark;
                }
            }
            
            OnColorChanged?.Invoke();
        }
    }
}