using System;
using UnityEngine;

public class GetCameraForCanvas : MonoBehaviour
{
	private void Awake()
	{
		Canvas canvas = GetComponent<Canvas>();
		
		Camera camera = GameObject.FindGameObjectsWithTag("MainCamera")[0].GetComponent<Camera>();
		
		canvas.worldCamera = camera;
		canvas.sortingLayerName = "Foreground";
		
	}
}