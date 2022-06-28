using System.Linq;
using GridField.Cells;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CountingCell : SimpleCell
{
    [SerializeField] private Text _text;

    [SerializeField] private int[] NeighboursOnSides = new int[4];
    [SerializeField] private int[] NeighboursOnSidesReserve = new int[4];

    public int GetFullCapacity() => NeighboursOnSidesReserve.Sum();

    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
        UpdateVisualization();
    }

    public void UpdateVisualization() => _text.text = NeighboursOnSides.Sum().ToString();

    public void ArrayToSIdesValues(int[] neighboursOnSides,
        bool isComparing = false)
    {
        for (int i = 0; i < NeighboursOnSides.Length; i++)
        {
            if (isComparing)
            {
                if (neighboursOnSides[i] != 0)
                {
                    NeighboursOnSides[i] = neighboursOnSides[i];
                }
            }
            else
            {
                NeighboursOnSides[i] = neighboursOnSides[i];
                NeighboursOnSidesReserve[i] = neighboursOnSides[i];
            }
        }
    }

    private void Start() => base.InitializeCellImage();
}