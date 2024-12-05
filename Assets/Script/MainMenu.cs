using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

	public void Play()
	{
		SceneManager.LoadScene("VillageScene");
	}

	public void Controls()
	{
		transform.GetChild(1).gameObject.SetActive(false);
		transform.GetChild(2).gameObject.SetActive(true);
	}

	public void Return()
	{
		transform.GetChild(1).gameObject.SetActive(true);
		transform.GetChild(2).gameObject.SetActive(false);
	}
	
	public void Quit()
	{
#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
#else
		Application.Quit();
#endif
	}
}