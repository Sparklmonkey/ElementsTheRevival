using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServicesSpinner : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> elementObjects;
    [SerializeField]
    private GameObject centerPosition;
    private List<Vector3> elementPositions;
    private List<bool> elementsIsMovingToCenter;

    private bool shouldMove = false;
    private void OnEnable()
    {
        elementPositions = new List<Vector3>();
        elementsIsMovingToCenter = new List<bool>();
        foreach (GameObject item in elementObjects)
        {
            elementPositions.Add(item.transform.position);
            elementsIsMovingToCenter.Add(true);
        }
        shouldMove = true;
    }

    private void OnDisable()
    {
        for (int i = 0; i < elementsIsMovingToCenter.Count; i++)
        {
            elementsIsMovingToCenter[i] = true;
            elementObjects[i].transform.position = elementPositions[i];
        }
        shouldMove = false;
    }
    // Update is called once per frame
    void Update()
    {
        if(!shouldMove) { return; }
        MoveElementIcon(0, elementsIsMovingToCenter[0]);
        MoveElementIcon(1, elementsIsMovingToCenter[1]);
        MoveElementIcon(2, elementsIsMovingToCenter[2]);
        MoveElementIcon(3, elementsIsMovingToCenter[3]);
        MoveElementIcon(4, elementsIsMovingToCenter[4]);
        MoveElementIcon(5, elementsIsMovingToCenter[5]);
        MoveElementIcon(6, elementsIsMovingToCenter[6]);
        MoveElementIcon(7, elementsIsMovingToCenter[7]);
        MoveElementIcon(8, elementsIsMovingToCenter[8]);
        MoveElementIcon(9, elementsIsMovingToCenter[9]);
        MoveElementIcon(10, elementsIsMovingToCenter[10]);
        MoveElementIcon(11, elementsIsMovingToCenter[11]);
    }

    void MoveElementIcon(int imageIndex, bool isMovingToCenter)
    {
        if (isMovingToCenter)
        {
            elementObjects[imageIndex].transform.position = Vector3.MoveTowards(elementObjects[imageIndex].transform.position, centerPosition.transform.position, 100f * Time.deltaTime);
            elementsIsMovingToCenter[imageIndex] = elementObjects[imageIndex].transform.position != centerPosition.transform.position;
        }
        else
        {
            elementObjects[imageIndex].transform.position = Vector3.MoveTowards(elementObjects[imageIndex].transform.position, elementPositions[imageIndex], 100f * Time.deltaTime);
            elementsIsMovingToCenter[imageIndex] = elementObjects[imageIndex].transform.position == elementPositions[imageIndex];
        }
    }

}
