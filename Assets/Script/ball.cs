using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ball : MonoBehaviour {
	
	public GameObject ballPrefab;
	public Sprite[] ballSprites;
	private GameObject firstBall;
	private GameObject lastBall;
	private string currentName;

	private bool isStart = false;
	private bool isEnd = false;
	public static bool isFinish = false;
	public static bool isPose = false;

	private int maxTime = 60;
	private float timeCounter = 60f;

	private int remove_cnt = 0;

	[SerializeField] private Text score = null;
	[SerializeField] private Text timer = null;
	[SerializeField] private Text countDown = null;

	[Header("ゲーム終了時用アニメーション")]
	[SerializeField] private GameObject finishAnimator = null;

	[Header("スキル発動用パーティクルシステム")]
	[SerializeField] private ParticleSystem exp = null;

	[Header("スキル発動ボタン用Image")]
	[SerializeField] private Image skillImg = null;

	[Header("ボール消滅時SE")]
	[SerializeField] private AudioClip[] se = null;

	[Header("ゲームポーズ用モーダル")]
	[SerializeField] private GameObject poseModal = null;

	public static int scoreCount = 0;

	private int skillCounter = 0;

	List<GameObject> removableBallList = new List<GameObject>();
	
	void Start () {
		timer.text = maxTime.ToString ();
		score.text = "0";
		StartCoroutine (CountDownTimer ());
	}
	
	void Update () {

		if (isStart && !isPose) {

			timeCounter -= Time.deltaTime;

			isEnd = (timer.text == "0") ? true : false;

			timeCounter = Mathf.Max (timeCounter, 0.0f);
			timer.text = ((int)timeCounter).ToString ();

			if (Input.GetMouseButtonDown (0) && firstBall == null) {
				OnDragStart ();
			} else if (Input.GetMouseButtonUp (0) && firstBall) {
				OnDragEnd ();
			} else if (Input.GetMouseButton (0) && firstBall) {
				OnDragging ();
			}

		}

		if (Input.GetKey (KeyCode.Space)) {
			isEnd = true;
		}

		if (isEnd) {

			isFinish = true;
			isStart = false;
			isEnd = false;

			GameObject endObj = Instantiate (finishAnimator) as GameObject;

		}
	}

	void FixedUpdate()
	{

	}
	
	private void OnDragStart() {
		removableBallList = new List<GameObject> ();

		RaycastHit2D hit = Physics2D.Raycast (Camera.main.ScreenToWorldPoint (Input.mousePosition), Vector2.zero);
		
		if (hit.collider != null) {
			if (hit.collider.gameObject.name.IndexOf ("BALL") != -1) {
				firstBall = hit.collider.gameObject;
				lastBall = firstBall;
				currentName = hit.collider.gameObject.name;
				removableBallList = new List<GameObject>();
				PushToList(lastBall);
			}
		}
	}
	
	private void OnDragging ()
	{
		RaycastHit2D hit = Physics2D.Raycast (Camera.main.ScreenToWorldPoint (Input.mousePosition), Vector2.zero);

		if (hit.collider != null) {
			GameObject hitObj = hit.collider.gameObject;
			if (hit.collider.gameObject.name == lastBall.name) {
				if(hit.collider.gameObject != lastBall){
				float distance = Vector2.Distance (lastBall.transform.position, hit.collider.gameObject.transform.position);
				if (distance <= 1.6f) {

					int idExistNo = -1;

					for (int i = 0; i < removableBallList.Count; i++) {
						if (removableBallList [i] == hitObj) {
							idExistNo = i;
						}
					}

					if (idExistNo == -1) {
						lastBall = hitObj;
						PushToList (hitObj);
					} else {

						for (int i = idExistNo + 1; i < removableBallList.Count; i++) {
							ChangeColor (removableBallList [i], 1f);
						}
						removableBallList.Remove (removableBallList [idExistNo + 1]);
						lastBall = removableBallList [removableBallList.Count - 1];
					}


				}
			}
			}
		}
	}
	
	private void OnDragEnd () {
		remove_cnt = removableBallList.Count;
		if (remove_cnt >= 3) {
			for (int i = 0; i < remove_cnt; i++) {
				Destroy (removableBallList [i]);
			}

			this.gameObject.GetComponent<AudioSource> ().PlayOneShot (se [0]);

			skillCounter += remove_cnt;
			skillImg.fillAmount = ((float)skillCounter) / 20f;

			scoreCount += remove_cnt;
			score.text = scoreCount.ToString ();

			removableBallList = new List<GameObject> ();
			StartCoroutine (DropBall (remove_cnt));
		} else {
			for (int i = 0; i < remove_cnt; i++) {
				ChangeColor (removableBallList [i], 1.0f);
			}
		}
			
		firstBall = null;
		lastBall = null;
	}
	
	IEnumerator DropBall(int count) {
		for (int i = 0; i < count; i++) {
			Vector2 pos = new Vector2(Random.Range(-2.0f, 2.0f), 7f);
			GameObject ball = Instantiate(ballPrefab, pos,
			                              Quaternion.AngleAxis(Random.Range(-40, 40), Vector3.forward)) as GameObject;
			int spriteId = Random.Range(0, 5);
			ball.name = "BALL" + spriteId;
			SpriteRenderer spriteObj = ball.GetComponent<SpriteRenderer>();
			spriteObj.sprite = ballSprites[spriteId];

//			if (remove_cnt > 6) {
//
//				int big = Random.Range (0, 101);
//
//				if (big <= 20) {
//					ball.transform.localScale = new Vector3 (1.5f, 1.5f, 1f);
//				}
//			}

			yield return new WaitForSeconds(0.05f);
		}
	}
	
	void PushToList (GameObject obj) {

		this.gameObject.GetComponent<AudioSource> ().PlayOneShot (se [2]);

		removableBallList.Add (obj);
		ChangeColor(obj, 0.5f);
	}
	void ChangeColor (GameObject obj, float transparency) {
		SpriteRenderer ballTexture = obj.GetComponent<SpriteRenderer>();
		ballTexture.color = new Color(ballTexture.color.r, ballTexture.color.g, ballTexture.color.b, transparency);
	}

	private IEnumerator CountDownTimer()
	{
		countDown.gameObject.SetActive (true);

		countDown.text = "3";
		yield return new WaitForSeconds (1f);

		countDown.text = "2";
		yield return new WaitForSeconds (1f);

		countDown.text = "1";
		yield return new WaitForSeconds (1f);

		countDown.text = "GO!!";
		StartCoroutine (DropBall (30));
		yield return new WaitForSeconds (1f);



		countDown.text = "";
		countDown.gameObject.SetActive (false);

		isStart = true;

		yield break;
	}

	public void OnOption()
	{

		isPose = true;
		GameObject pObj = Instantiate (this.poseModal) as GameObject;
		Time.timeScale = 0f;

	}

	public void OnSkill()
	{
		if (skillCounter >= 20) {

			skillCounter = 0;

			exp.Play ();
			this.gameObject.GetComponent<AudioSource> ().PlayOneShot (se [1]);

			skillImg.fillAmount = 0f;

			Collider2D[] targets = Physics2D.OverlapCircleAll (new Vector2 (0f, 0f), 1.5f);

			for (int i = 0; i < targets.Length; i++) {
				Destroy (targets [i].gameObject);
			}

			scoreCount += targets.Length;
			score.text = scoreCount.ToString ();

			StartCoroutine (DropBall (targets.Length));

		}
	}

	public void OnSpin()
	{
			Collider2D[] targets = Physics2D.OverlapCircleAll (new Vector2 (0f, 0f), 3f);

			foreach (Collider2D obj in targets) {
				if ((obj) && (obj.tag == "ball")) {
					int range = Random.Range (0, 2);
					float xrange = (range == 0) ? Random.Range (-300f, -500f) : Random.Range (300f, 500f);
					obj.gameObject.GetComponent<Rigidbody2D> ().AddForce (new Vector2 (xrange, Random.Range (300f, 500f)));
				}
			}
	}
}