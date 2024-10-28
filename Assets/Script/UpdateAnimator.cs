using EventSystem.SO;
using UnityEngine;

public class UpdateAnimator : MonoBehaviour
{
	[SerializeField] 
	private BoolEventSO blockingEvent;
	
	[SerializeField] 
	private BoolEventSO rollingEvent;

	public void EndBlocking()
	{
		blockingEvent.Value = false;
	}

	public void EndRolling()
	{
		rollingEvent.Value = false;
	}
}