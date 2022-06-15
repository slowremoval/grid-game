using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GridField.Cells
{
    public class GridCellConstruct : GridCell
    {
        [SerializeField] private TextMeshProUGUI _textMeshProUGUI;

        [SerializeField] private GameObject _visualization;

        public int requiredAmount { get; private set; }

        [HideInInspector] public CellType ThisCellType;

        private Color _color;

        private Image _currentCellVisualization;

        private bool SideSetActive(int sideNumber)
        {
            UnactiveSides[sideNumber - 1] = UnactiveSides[sideNumber - 1] == 0 ? (CellSide)sideNumber : (CellSide)0;
            return UnactiveSides[sideNumber - 1] == 0;
        }

        public bool ChangeLeftSideState() => SideSetActive((int)CellSide.left);
        public bool ChangeRightSideState() => SideSetActive((int)CellSide.right);
        public bool ChangeUpperSideState() => SideSetActive((int)CellSide.top);
        public bool ChangeUnderSideState() => SideSetActive((int)CellSide.down);

        public void SetRequiredAmount(int amount)
        {
            requiredAmount = amount;
            UpdateCellVisualization();
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);
            if (Input.GetMouseButton(1))
            {
                ThisCellType =
                    ThisCellType == CellType.simple
                        ? CellType.empty
                        : CellType.simple;
            }

            UpdateCellVisualization();
        }

        private void OnValidate()
        {
            UpdateCellVisualization();
        }

        private void UpdateCellVisualization()
        {
            _currentCellVisualization ??= _visualization.GetComponent<Image>();

            _textMeshProUGUI.text = $"{ThisCellType.ToString()}";

            SwitchCellTypeAndProperties();

            _currentCellVisualization.color = _color;
        }

        private void SwitchCellTypeAndProperties()
        {
            switch (ThisCellType)
            {
                case CellType.empty:
                    _color = Color.white;
                    _color.a = 0.1f;
                    break;

                case CellType.dark:
                    _color = Color.grey;
                    _color.a = 0.81f;
                    _textMeshProUGUI.text += $"\n_{requiredAmount}";
                    break;

                case CellType.light:
                    _color = Color.yellow;
                    _color.a = 0.81f;
                    _textMeshProUGUI.text += $"\n_{requiredAmount}";
                    break;

                case CellType.simple:
                    _color = Color.white;
                    _color.a = 0.9f;
                    break;

                case CellType.rotating:
                    _color = Color.green;
                    _color.a = 0.9f;
                    break;
                case CellType.stableDark:
                    _color = Color.gray;
                    _color.a = 0.7f;
                    break;
                case CellType.stableLight:
                    _color = Color.yellow;
                    _color.a = 0.42f;
                    break;
                case CellType.universal:
                    _color = Color.cyan;
                    _color.a = 0.8f;
                    break;
            }
        }

        public void SetCellType(CellType cellType)
        {
            ThisCellType = cellType;
            UpdateCellVisualization();
        }

        public void SetLightNode()
        {
            ThisCellType = CellType.light;
            UpdateCellVisualization();
        }

        public void SetDarkNode()
        {
            ThisCellType = CellType.dark;
            UpdateCellVisualization();
        }

        public void SetSimpleCell()
        {
            ThisCellType = CellType.simple;
            requiredAmount = 0;
            UpdateCellVisualization();
        }

        public void SetEmptyCell()
        {
            ThisCellType = CellType.empty;
            requiredAmount = 0;
            UpdateCellVisualization();
        }

        public void SetRotatingCell()
        {
            ThisCellType = CellType.rotating;
            requiredAmount = 0;
            UpdateCellVisualization();
        }

        public void SetUniversalCell()
        {
            ThisCellType = CellType.universal;
            requiredAmount = 0;
            UpdateCellVisualization();
        }

        public void SetStableDarkCell()
        {
            ThisCellType = CellType.stableDark;
            requiredAmount = 0;
            UpdateCellVisualization();
        }

        public void SetStableLightCell()
        {
            ThisCellType = CellType.stableLight;
            requiredAmount = 0;
            UpdateCellVisualization();
        }
    }
}