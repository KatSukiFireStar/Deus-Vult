using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

	public void Play()
	{
		//Load the main scene
		SceneManager.LoadScene("VillageScene");
	}

	public void Controls()
	{
		//Show the controls panel and hiode the main menu panel
		transform.GetChild(1).gameObject.SetActive(false);
		transform.GetChild(2).gameObject.SetActive(true);
	}

	public void Return()
	{
		//Show the main menu panel and hiode the controls panel
		transform.GetChild(1).gameObject.SetActive(true);
		transform.GetChild(2).gameObject.SetActive(false);
	}
	
	public void Quit()
	{
		//Quit the application
#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
#else
		Application.Quit();
#endif
	}
}