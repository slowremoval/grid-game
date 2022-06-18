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

        [SerializeField] private GameObject[] _unactiveSidesMarkers;

        public int CellCapacity { get; private set; }

        [HideInInspector] public CellType ThisCellType;

        private Color _color;

        private Image _currentCellVisualization;

        private bool SideSetActive(int sideNumber)
        {
            if (UnactiveSides[sideNumber - 1] == 0)
            {
                UnactiveSides[sideNumber - 1] = (CellSide)sideNumber;
                _unactiveSidesMarkers[sideNumber - 1].SetActive(true);
            }
            else
            {
                UnactiveSides[sideNumber - 1] = (CellSide)0;
                _unactiveSidesMarkers[sideNumber - 1].SetActive(false);
            }

            return UnactiveSides[sideNumber - 1] == 0;
        }

        public bool ChangeLeftSideState() => SideSetActive((int)CellSide.left);
        public bool ChangeRightSideState() => SideSetActive((int)CellSide.right);
        public bool ChangeUpperSideState() => SideSetActive((int)CellSide.top);
        public bool ChangeUnderSideState() => SideSetActive((int)CellSide.down);

        public void SetRequiredAmount(int amount)
        {
            CellCapacity = amount;
            UpdateCellVisualization();
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);
            if (Input.GetMouseButton(1))
            {
                if (ThisCellType == CellType.simple)
                {
                    CellCapacity = 0;
                    ThisCellType = CellType.empty;
                }
                else
                {
                    CellCapacity = 1;
                    ThisCellType = CellType.simple;
                }
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
                    _textMeshProUGUI.text += $"\n_{CellCapacity}";
                    break;

                case CellType.light:
                    _color = Color.yellow;
                    _color.a = 0.81f;
                    _textMeshProUGUI.text += $"\n_{CellCapacity}";
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
            CellCapacity = 1;
            UpdateCellVisualization();
        }

        public void SetDarkNode()
        {
            ThisCellType = CellType.dark;
            CellCapacity = 1;
            UpdateCellVisualization();
        }

        public void SetSimpleCell()
        {
            ThisCellType = CellType.simple;
            CellCapacity = 1;
            UpdateCellVisualization();
        }

        public void SetEmptyCell()
        {
            ThisCellType = CellType.empty;
            CellCapacity = 0;
            UpdateCellVisualization();
        }

        public void SetRotatingCell()
        {
            ThisCellType = CellType.rotating;
            CellCapacity = 1;
            UpdateCellVisualization();
        }

        public void SetUniversalCell()
        {
            ThisCellType = CellType.universal;
            CellCapacity = 1;
            UpdateCellVisualization();
        }

        public void SetStableDarkCell()
        {
            ThisCellType = CellType.stableDark;
            CellCapacity = 1;
            UpdateCellVisualization();
        }

        public void SetStableLightCell()
        {
            ThisCellType = CellType.stableLight;
            CellCapacity = 1;
            UpdateCellVisualization();
        }
    }
}