using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CrowdAngerScript : MonoBehaviour {

	private Image meter;
	public float angerDelay = 2.0f; //total amount of time it takes for the meter to reach zero
	private float maxDelay;
	private const float MIN_DELAY = 1.0f;
	private float angerTimer;
	public float delayReduction = 0.9f;
	private MatchManagerScript matchManagerScript;
	private const string GAMEMANAGER = "GameManager";
	private ScoreManager scoreManager;
	private const string SCORE_NAME = "Score";

	private void Start(){
		meter = GetComponent<Image>();
		maxDelay = angerDelay;
		angerTimer = angerDelay;
		matchManagerScript = transform.root.Find(GAMEMANAGER).GetComponent<MatchManagerScript>();
		scoreManager = transform.parent.Find(SCORE_NAME).GetComponent<ScoreManager>();
	}

	private void Update(){
		angerTimer -= Time.deltaTime;

		meter.fillAmount = angerTimer/angerDelay;

		if (angerTimer <= 0.0f){
			ResetForFailure();
		}
	}

	public void ResetForSuccess(){
		angerDelay *= delayReduction;
		if (angerDelay < MIN_DELAY) { angerDelay = MIN_DELAY; }
		angerTimer = angerDelay;
	}

	public void ResetForFailure(){
		angerDelay = maxDelay;
		angerTimer = angerDelay;
		matchManagerScript.CrowdMultiplier = matchManagerScript.Min_Crowd_Multiplier;
		scoreManager.BonusFeedback(matchManagerScript.Min_Crowd_Multiplier);
	}
}
