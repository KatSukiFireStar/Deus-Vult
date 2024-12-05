using System;
using UnityEngine;

public class GetCameraForCanvas : MonoBehaviour
{
	private void Awake()
	{
		//Find the main camera on the player and add it to the canvas in the game object
		
		Canvas canvas = GetComponent<Canvas>();
		
		Camera camera = GameObject.FindGameObjectsWithTag("MainCamera")[0].GetComponent<Camera>();
		
		canvas.worldCamera = camera;
		canvas.sortingLayerName = "Foreground";
		
	}
}