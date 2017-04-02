using UnityEngine;
using System.Collections;

public class TestBall : MonoBehaviour {

	[SerializeField] private float y = 1f;

	void Update () {
	
		RaycastHit2D hit = Physics2DExtentsion.RaycastAndDraw (this.gameObject.transform.position, new Vector2 (100f, y), 1f, 100);

	}
}
