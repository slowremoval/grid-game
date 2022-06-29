using UnityEngine;
using UnityEngine.UI;

namespace GridField.Cells
{
    public class StableCell : GridCell
    {
        [SerializeField] private GameObject _visualization;
        private Image _nodeVisualization;

        [SerializeField] protected Color _stableLightColor;
        [SerializeField] protected Color _stableDarkColor;
        [SerializeField] protected Color _universalColor;
        
        private void Start()
        {
            UpdateTypeVisualization();
        }

        protected void UpdateTypeVisualization() => SelectCellType();

        private void SelectCellType()
        {
            InitializeVisualizationImageIfNeeds();
            Color color = Color.magenta;
            
            switch (CellType)
            {
                case CellType.dark:
                    color = _stableDarkColor;
                    break;
                case CellType.light:
                    color = _stableLightColor;
                    break;
                case CellType.universal:
                    color = _universalColor;
                    break;
                case CellType.stableDark:
                    CellType = CellType.dark;
                    color = _stableDarkColor;
                    color.a = 0.7f;
                    break;
                case CellType.stableLight:
                    CellType = CellType.light;
                    color = _stableLightColor;
                    color.a = 0.42f;
                    break;
                default:
                    _nodeVisualization.color = Color.magenta;
                    break;
            }

            _nodeVisualization.color = color;
        }

        private void InitializeVisualizationImageIfNeeds() =>
            _nodeVisualization ??= _visualization.GetComponent<Image>();
    }
}