using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreFeedbackBehavior : MonoBehaviour {

	RectTransform rectTransform;
	public float moveDuration = 5.0f;
	float timer = 0.0f;
	Vector3 start;
	Vector3 end;
	public Vector3 moveDist = new Vector3(0.0f, 10.0f, 0.0f);
	public AnimationCurve animCurve;

	void Start(){
		rectTransform = GetComponent<RectTransform>();
		start = rectTransform.anchoredPosition;
		end = start + moveDist;
	}

	void Update(){
		timer += Time.deltaTime;

		Vector3 loc = Vector3.Lerp(start, end, animCurve.Evaluate(timer/moveDuration));
		rectTransform.anchoredPosition = loc;

		if (timer >= moveDuration){
			Destroy(gameObject);
		}
	}
}
