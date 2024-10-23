using System;
using UnityEngine;

public class CameraMaxBoundary : MonoBehaviour
{
	[SerializeField]
	private float maxX;
	[SerializeField]
	private float minX;

	public float MaxX
	{
		get => maxX;
		set => maxX = value;
	}

	public float MinX
	{
		get => minX;
		set => minX = value;
	}

	private void Update()
	{
		if(transform.parent.position.x < minX)
			transform.position = new Vector3(minX, transform.position.y, transform.position.z);
		else if(transform.parent.position.x > maxX)
			transform.position = new Vector3(maxX, transform.position.y, transform.position.z);
		
		if (transform.position.x < minX)
		{
			transform.position = new Vector3(minX, transform.position.y, transform.position.z);
		}else if (transform.position.x > maxX)
		{
			transform.position = new Vector3(maxX, transform.position.y, transform.position.z);
		}
	}
}