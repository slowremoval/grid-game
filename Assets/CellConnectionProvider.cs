using System.Collections.Generic;
using UnityEngine;

public class CellConnectionProvider : MonoBehaviour
{
    
    [Header("Cell item connections")] [Space] [SerializeField]
    private List<GameObject> _cellItemConnections;


    public void TopConnectionSetActive(bool setActive) => _cellItemConnections[0].SetActive(setActive);

    public void DownConnectionSetActive(bool setActive) => _cellItemConnections[1].SetActive(setActive);

    public void LeftConnectionSetActive(bool setActive) => _cellItemConnections[2].SetActive(setActive);

    public void RightConnectionSetActive(bool setActive) => _cellItemConnections[3].SetActive(setActive);

}