using EventSystem.SO;
using UnityEngine;

public class UpdateAnimator : MonoBehaviour
{
	[SerializeField] 
	private BoolEventSO blockingEvent;
	

	public void EndBlocking()
	{
		blockingEvent.Value = false;
	}
	
}