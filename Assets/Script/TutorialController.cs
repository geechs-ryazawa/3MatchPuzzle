using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class TutorialController : MonoBehaviour {

	[SerializeField] private List<GameObject> img = new List<GameObject>();

	public void Step0()
	{
		GameObject obj = Instantiate (img [0]) as GameObject;
	}

}