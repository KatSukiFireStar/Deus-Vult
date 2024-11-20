using EventSystem.SO;
using UnityEngine;

public class FadeManager : MonoBehaviour
{
	[SerializeField] 
	private BoolEventSO fadeEvent;

	public void OnFadeComplete()
	{
		fadeEvent.Value = true;
	}
	
}