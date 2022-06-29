using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GridField.Cells
{
    public class SimpleCell : GridCell
    {
        [SerializeField] protected GameObject _visualization;

        
        private Image _cellImage;
        
        public void InitializeCellImage() => _cellImage = _visualization.GetComponent<Image>();

        public override void OnPointerDown(PointerEventData eventData) => SetColorByClick();

        private void Start() => InitializeCellImage();
        
        [SerializeField] protected Color _defaultColor;
        [SerializeField] protected Color _lightColor;
        [SerializeField] protected Color _darkColor;

        private void SetColorByClick()
        {
            if (Input.GetMouseButton(0))
            {
                if (_cellImage.color == _lightColor)
                {
                    _cellImage.color = _defaultColor;
                    CellType = CellType.empty;
                }
                else
                {
                    _cellImage.color = _lightColor;
                    CellType = CellType.light;
                }
            }
            else if (Input.GetMouseButton(1))
            {
                if (_cellImage.color == _darkColor)
                {
                    _cellImage.color = _defaultColor;
                    CellType = CellType.empty;
                }
                else
                {
                    _cellImage.color = _darkColor;
                    CellType = CellType.dark;
                }
            }

            SendVisualizationChanged();
        }

    }
}