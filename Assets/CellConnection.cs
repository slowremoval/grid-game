using System;
using System.Collections;
using GridField.Cells;
using UnityEngine;
using UnityEngine.UI;

public class CellConnection : MonoBehaviour
{
    [SerializeField] private GameObject _visualisation;

    private Image _visualizationImage;
    private Color _color;

    private void Start()
    {
        TryInitializeImage();
    }

    public void ActivateConnection(CellType type)
    {
        StopAllCoroutines();
        SetColor(type);
        TryInitializeImage();
        if (_visualizationImage == null)
        {
            return;
        }
        _visualizationImage.color = _color;
        StartCoroutine(UpscaleConnectionRoutine(gameObject, Vector3.one * 1.1f, 8f));
    }

    
    
    public void DeactivateConnectionUrgently()
    {
        StopAllCoroutines();
        StartCoroutine(UpscaleConnectionRoutine(gameObject, Vector3.zero, 11));
    }

    private void SetColor(CellType type)
    {
        switch (type)
        {
            case CellType.dark:
                _color = Color.gray;
                _color.a = 1;
                break;
            case CellType.light:
                _color = Color.yellow;
                _color.a = 1;
                break;
            case CellType.simple:
                _color = Color.white;
                _color.a = 1;
                break;
            default:
                _color = Color.magenta;
                break;
        }
    }

    private void TryInitializeImage()
    {
        if (_visualizationImage is null)
        {
            _visualisation.TryGetComponent<Image>(out _visualizationImage);
        }
    }

    private IEnumerator UpscaleConnectionRoutine(GameObject connectionSide, Vector3 toScale, float connectionScaleSpeed = 5f)
    {
        while (Math.Abs(connectionSide.transform.localScale.x - toScale.x) > 0.001f)
        {
            yield return null;
            
            connectionSide.transform.localScale =
                Vector3.Lerp(connectionSide.transform.localScale, toScale,
                    Time.deltaTime * connectionScaleSpeed);
        }
    }
}