using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace GridField.Cells
{
    public class CellNode : StableCell
    {
        [SerializeField] private TextMeshProUGUI _textMeshProUGUI;


        [HideInInspector] public int CurrentAmount;

       private void Start()
        {
            if (_textMeshProUGUI == null)
            {
                return;
            }
            UpdateVisualization();
        }

        public void UpdateVisualization()
        {
            ShowNodeRequirements(CurrentAmount);
            base.UpdateTypeVisualization();
        }

        private void ShowNodeRequirements(int currentAmount)
        {
            _textMeshProUGUI.text = $"{currentAmount}/{Capacity}";
        }
    }
}