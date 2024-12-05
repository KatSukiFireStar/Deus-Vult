using System;
using UnityEngine;

public class DontDestroy : MonoBehaviour
{
	private void Start()
	{
		//Add the game object to the unity don't destroy on load
		DontDestroyOnLoad(gameObject);
	}
}