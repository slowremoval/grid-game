using UnityEngine;
using UnityEngine.UI;

namespace GridField.Cells
{
    public class StableCell : GridCell
    {
        [SerializeField] private GameObject _visualization;
        private Image _nodeVisualization;

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
                    color = Color.gray;
                    break;
                case CellType.light:
                    color = Color.yellow;
                    break;
                case CellType.universal:
                    color = Color.cyan;
                    break;
                case CellType.stableDark:
                    CellType = CellType.dark;
                    color = Color.gray;
                    color.a = 0.7f;
                    break;
                case CellType.stableLight:
                    CellType = CellType.light;
                    color = Color.yellow;
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