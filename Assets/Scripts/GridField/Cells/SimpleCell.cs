using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GridField.Cells
{
    public class SimpleCell : GridCell
    {
        [SerializeField] protected GameObject _visualization;

        [SerializeField] protected Color _defaultColor;
        [SerializeField] protected Color _firstColor;
        [SerializeField] protected Color _secondColor;


        protected Image _cellImage;
        
        public event Action<GridCell> OnColorChanged;

        private void Start()
        {
            InitializeCellImage();
        }

        protected void InitializeCellImage()
        {
            _cellImage = _visualization.GetComponent<Image>();
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            SetColorByClick();
        }

        protected void SetColorByClick()
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

            SendVisualizationChanged();
        }

        public void SendVisualizationChanged() => OnColorChanged?.Invoke(this);
    }
}