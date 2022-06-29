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

        
        public void ShowUnactiveSides()
        {
            for (int i = 0; i < UnactiveSides.Length; i++)
            {
                if (UnactiveSides[i] > 0)
                {
                    SideSetActive((int)UnactiveSides[i], true);
                }
            }
        }

        public void ChangeLeftSideState() => SideSetActive((int)CellSide.left);

        public void ChangeRightSideState() => SideSetActive((int)CellSide.right);

        public void ChangeUpperSideState() => SideSetActive((int)CellSide.top);

        public void ChangeUnderSideState() => SideSetActive((int)CellSide.down);

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
        
        private void SideSetActive(int sideNumber)
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
        }

        private void SideSetActive(int sideNumber, bool setActive) =>
            _unactiveSidesMarkers[sideNumber - 1].SetActive(setActive);

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
                case CellType.universalRotating:
                    _color = Color.cyan;
                    _color.a = 0.7f;
                    break;
                case CellType.darkNodeRotating:
                    _color = Color.gray;
                    _color.a = 0.7f;
                    _textMeshProUGUI.text = $"Rotating\n_{CellCapacity}";
                    break;
                case CellType.darkStableRotating:
                    _color = Color.gray;
                    _color.a = 0.42f;
                    break;
                case CellType.lightNodeRotating:
                    _color = Color.yellow;
                    _color.a = 0.81f;
                    _textMeshProUGUI.text = $"Rotating\n_{CellCapacity}";
                    break;
                case CellType.lightStableRotating:
                    _color = Color.yellow;
                    _color.a = 0.42f;
                    break;
                default: 
                    _color = Color.magenta;
                    _color.a = 1;
                    break;
            }
        }

        #region CellType Set Buttons
        
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

        public void SetUniversalRotatingCell()
        {
            ThisCellType = CellType.universalRotating;
            CellCapacity = 1;
            UpdateCellVisualization();
        }

        public void SetStableDarkRotatingCell()
        {
            ThisCellType = CellType.darkStableRotating;
            CellCapacity = 1;
            UpdateCellVisualization();
        }

        public void SetStableLightRotatingCell()
        {
            ThisCellType = CellType.lightStableRotating;
            CellCapacity = 1;
            UpdateCellVisualization();
        }

        public void SetDarkNodeRotatingCell()
        {
            ThisCellType = CellType.darkNodeRotating;
            CellCapacity = 0;
            UpdateCellVisualization();
        }

        public void SetLightNodeRotatingCell()
        {
            ThisCellType = CellType.lightNodeRotating;
            CellCapacity = 0;
            UpdateCellVisualization();
        }
    }
    
    #endregion 
}