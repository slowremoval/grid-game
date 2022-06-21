using GridField.Cells;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CountingCell : SimpleCell
{
    [SerializeField] private Text _text;

    public bool isChecked = false;
    
    private void Start()
    {
        base.InitializeCellImage();
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
        UnpateVisualization();
    }

    public void UnpateVisualization()
    {
        _text.text = Capacity.ToString();
    }
}