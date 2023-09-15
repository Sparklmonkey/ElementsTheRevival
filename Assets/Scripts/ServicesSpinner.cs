using System.Collections.Generic;
using UnityEngine;

public class ServicesSpinner : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> elementObjects;
    [SerializeField]
    private GameObject centerPosition;
    private List<Vector3> _elementPositions;
    private List<bool> _elementsIsMovingToCenter;

    private bool _shouldMove = false;
    private void OnEnable()
    {
        _elementPositions = new List<Vector3>();
        _elementsIsMovingToCenter = new List<bool>();
        foreach (GameObject item in elementObjects)
        {
            _elementPositions.Add(item.transform.position);
            _elementsIsMovingToCenter.Add(true);
        }
        _shouldMove = true;
    }

    private void OnDisable()
    {
        for (int i = 0; i < _elementsIsMovingToCenter.Count; i++)
        {
            _elementsIsMovingToCenter[i] = true;
            elementObjects[i].transform.position = _elementPositions[i];
        }
        _shouldMove = false;
    }
    // Update is called once per frame
    void Update()
    {
        if (!_shouldMove) { return; }
        MoveElementIcon(0, _elementsIsMovingToCenter[0]);
        MoveElementIcon(1, _elementsIsMovingToCenter[1]);
        MoveElementIcon(2, _elementsIsMovingToCenter[2]);
        MoveElementIcon(3, _elementsIsMovingToCenter[3]);
        MoveElementIcon(4, _elementsIsMovingToCenter[4]);
        MoveElementIcon(5, _elementsIsMovingToCenter[5]);
        MoveElementIcon(6, _elementsIsMovingToCenter[6]);
        MoveElementIcon(7, _elementsIsMovingToCenter[7]);
        MoveElementIcon(8, _elementsIsMovingToCenter[8]);
        MoveElementIcon(9, _elementsIsMovingToCenter[9]);
        MoveElementIcon(10, _elementsIsMovingToCenter[10]);
        MoveElementIcon(11, _elementsIsMovingToCenter[11]);
    }

    void MoveElementIcon(int imageIndex, bool isMovingToCenter)
    {
        if (isMovingToCenter)
        {
            elementObjects[imageIndex].transform.position = Vector3.MoveTowards(elementObjects[imageIndex].transform.position, centerPosition.transform.position, 100f * Time.deltaTime);
            _elementsIsMovingToCenter[imageIndex] = elementObjects[imageIndex].transform.position != centerPosition.transform.position;
        }
        else
        {
            elementObjects[imageIndex].transform.position = Vector3.MoveTowards(elementObjects[imageIndex].transform.position, _elementPositions[imageIndex], 100f * Time.deltaTime);
            _elementsIsMovingToCenter[imageIndex] = elementObjects[imageIndex].transform.position == _elementPositions[imageIndex];
        }
    }

}
