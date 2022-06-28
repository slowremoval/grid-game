using System.Linq;
using TMPro;
using UnityEngine;

namespace GridField.Cells
{
    public class CellNode : StableCell
    {
        [SerializeField] private TextMeshProUGUI _textMeshProUGUI;

        private int _currentAmount;

        private int[] _neighboursOnSidesAround  = new int[4];
        
        public void UpdateVisualization()
        {
            _currentAmount = _neighboursOnSidesAround.Sum();
            
            ShowNodeRequirements();
            
            base.UpdateTypeVisualization();
        }

        public void SetNeighboursAround(int[] neighboursOnSides)
        {
            for (int i = 0; i < _neighboursOnSidesAround.Length; i++)
            {
                _neighboursOnSidesAround[i] = neighboursOnSides[i];
            }
            UpdateVisualization();
        }
        
        private void ShowNodeRequirements() => _textMeshProUGUI.text = $"{_currentAmount}\n{Capacity}";

        private void Start()
        {
            if (_textMeshProUGUI == null)
            {
                return;
            }
            UpdateVisualization();
        }
    }
}