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
        
        private Image _cellImage;
        
        public void InitializeCellImage() => _cellImage = _visualization.GetComponent<Image>();

        public override void OnPointerDown(PointerEventData eventData) => SetColorByClick();

        private void Start() => InitializeCellImage();
        

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

            SendVisualizationChanged();
        }

    }
}