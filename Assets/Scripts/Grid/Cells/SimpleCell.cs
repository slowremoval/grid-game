using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SimpleCell : GridCell, IPointerDownHandler
{
    [SerializeField] private GameObject _visualization;

    [SerializeField] private Color _defaultColor;
    [SerializeField] private Color _firstColor;
    [SerializeField] private Color _secondColor;

    private Image _cellImage;
    
    private void Awake()
    {
        _cellImage = _visualization.GetComponent<Image>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (Input.GetMouseButton(0))
        {
            _cellImage.color =
                _cellImage.color == _firstColor
                    ? _defaultColor
                    : _firstColor;
        }
        else if (Input.GetMouseButton(1))
        {
            _cellImage.color =
                _cellImage.color == _secondColor
                    ? _defaultColor
                    : _secondColor;
        }
    }
}