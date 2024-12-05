using EventSystem.SO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FadeManager : MonoBehaviour
{
	[SerializeField] 
	private RespawnEventSO changeSceneEvent;

	[SerializeField] 
	private BoolEventSO fadeEvent;
	
	
	private Animator animator;

	private void Start()
	{
		animator = GameObject.FindGameObjectWithTag("Player").transform.GetComponentsInChildren<Animator>()[1];
	}

	public void OnFadeComplete()
	{
		//It's launched with the animation
		//It'll load the scene in the change scene event and apply the good information to the player
		GameObject player = GameObject.FindGameObjectWithTag("Player");
				
		SceneManager.LoadScene(changeSceneEvent.Value.sceneName);
		player.transform.position = changeSceneEvent.Value.respawnPosition;
		player.transform.GetChild(0).position = new(0 + changeSceneEvent.Value.respawnPosition.x, 3f + changeSceneEvent.Value.respawnPosition.y, -10);
		CameraMaxBoundary bound = player.transform.GetChild(0).GetComponent<CameraMaxBoundary>();
		bound.MaxX = changeSceneEvent.Value.maxXBoundary;
		bound.MinX = changeSceneEvent.Value.minXBoundary;
		animator.SetTrigger("FadeOut");
		fadeEvent.Value = true;
	}
	
}