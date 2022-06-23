using GridField.Cells;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CountingCell : SimpleCell
{
    [SerializeField] private Text _text;

    [SerializeField]private int[] NeighboursOnSides = new int[4];

    public void ArrayToSIdesValues(int[] neighboursOnSides)
    {
        for (int i = 0; i < NeighboursOnSides.Length; i++)
        {
            NeighboursOnSides[i] = neighboursOnSides[i];
        }
    }    
    private void Start()
    {
        base.InitializeCellImage();
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
        UpdateVisualization();
    }

    public void UpdateVisualization()
    {
        _text.text = Capacity.ToString();
    }
}