using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour {

	public void OnStart()
	{
		SceneManager.LoadScene ("main");
	}

	public void OnReturn()
	{
		ball.isPose = false;
		Time.timeScale = 1f;
		Destroy (this.gameObject);
	}

	public void OnTitle()
	{
		Time.timeScale = 1f;
		SceneManager.LoadScene ("Title");
	}

}
