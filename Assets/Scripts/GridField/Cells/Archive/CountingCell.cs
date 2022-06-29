// using System.Linq;
// using GridField.Cells;
// using UnityEngine;
// using UnityEngine.EventSystems;
// using UnityEngine.UI;
//
// public class CountingCell : SimpleCell
// {
//     [SerializeField] private Text _text;
//
//     private int[] _neighboursOnSides = new int[4];
//     private int[] _neighboursOnSidesAround = new int[4];
//
//     public int GetFullCapacity() => _neighboursOnSidesAround.Sum();
//
//     public override void OnPointerDown(PointerEventData eventData)
//     {
//         base.OnPointerDown(eventData);
//         SendVisualizationChanged();
//         UpdateVisualization();
//     }
//
//     public void UpdateVisualization() => _text.text = _neighboursOnSides.Sum().ToString();
//
//     public void ArrayToSIdesValues(int[] neighboursOnSides,
//         bool isComparing = false)
//     {
//         for (int i = 0; i < _neighboursOnSides.Length; i++)
//         {
//             if (isComparing)
//             {
//                 if (neighboursOnSides[i] != 0)
//                 {
//                     _neighboursOnSides[i] = neighboursOnSides[i];
//                 }
//             }
//             else
//             {
//                 _neighboursOnSidesAround[i] = neighboursOnSides[i];
//             }
//         }
//     }
//
//     private void Start() => base.InitializeCellImage();
// }