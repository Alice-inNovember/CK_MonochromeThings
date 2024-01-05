using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class UILookMe : MonoBehaviour
{
	[SerializeField] private List<GameObject> uiList;

	private void Start()
	{
		var target = this.transform.position;
		foreach (var obj in uiList)
		{
			var position = obj.transform.position;
			obj.transform.rotation =  Quaternion.Euler(0,0,  Mathf.Atan2(position.y - target.y, position.x - target.x) * Mathf.Rad2Deg);
		}
	}
}
