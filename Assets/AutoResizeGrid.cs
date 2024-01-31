using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AutoResizeGrid : MonoBehaviour
{
    private GridLayoutGroup _gridLayoutGroup;
    private RectTransform _rect;
    private float _height = 180;
   	public int cellCount = 2;
    
   	private void Start ()
   	{
   		_gridLayoutGroup = GetComponent<GridLayoutGroup>();
	    _rect = GetComponent<RectTransform>();
	    var rect = _rect.rect;
	    _gridLayoutGroup.cellSize = new Vector2(rect.height, rect.height); 
        cellCount = GetComponentsInChildren<RectTransform>().Length;
	}
    
    private void OnRectTransformDimensionsChange()
    {
	    if (_gridLayoutGroup == null || _rect == null) return;
	    if ((_rect.rect.height + (_gridLayoutGroup.padding.horizontal * 2)) * cellCount < _rect.rect.width)
	    {
		    var rect = _rect.rect;
		    _gridLayoutGroup.cellSize = new Vector2 (rect.height, rect.height);
	    }
    }
}
