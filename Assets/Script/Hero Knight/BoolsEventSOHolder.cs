using EventSystem.SO;
using UnityEngine;

/*
 * Dirty patch to prevent shrodinger
 * It's attached to the player in order to keep an event in the memory and don't destroy him if he isn't used in a new scene
 */
public class BoolsEventSOHolder : MonoBehaviour
{
	[SerializeField] 
	private BoolsEventSO heartPickupEvent;
}