using System;
using UnityEngine;

[Serializable]
public class Respawn
{
	public string sceneName;
	public Vector2 respawnPosition;
	public int minXBoundary;
	public int maxXBoundary;
}