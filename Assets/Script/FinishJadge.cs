using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FinishJadge : MonoBehaviour {

	[SerializeField] private Text resultCount = null;

	void Update () {

		if (ball.isFinish) {

			resultCount.text = ball.scoreCount.ToString ();

		}
	
	}
		
	public void OnTitle()
	{
		SceneManager.LoadScene ("Title");
	}

}
