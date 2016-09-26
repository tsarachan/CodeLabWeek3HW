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
	private HealthManager healthManager;
	private const string LIFE_ICONS = "Life icons";

	private void Start(){
		meter = GetComponent<Image>();
		maxDelay = angerDelay;
		angerTimer = angerDelay;
		matchManagerScript = transform.root.Find(GAMEMANAGER).GetComponent<MatchManagerScript>();
		scoreManager = transform.parent.Find(SCORE_NAME).GetComponent<ScoreManager>();
		healthManager = transform.parent.Find(LIFE_ICONS).GetComponent<HealthManager>();
	}

	private void Update(){
		angerTimer -= Time.deltaTime;

		meter.fillAmount = angerTimer/angerDelay;

		if (angerTimer <= 0.0f){
			ResetForFailure();
		}
	}

	/// <summary>
	/// Refill the crowd's patience when the player successfully makes a match before time runs out.
	/// </summary>
	public void ResetForSuccess(){
		angerDelay *= delayReduction;
		if (angerDelay < MIN_DELAY) { angerDelay = MIN_DELAY; }
		angerTimer = angerDelay;
	}

	/// <summary>
	/// If the crowd's patience runs out, reset their patience and cause ancillary effects.
	/// </summary>
	public void ResetForFailure(){
		angerDelay = maxDelay;
		angerTimer = angerDelay;
		Debug.Log("ResetForFailure() called");
		healthManager.DetermineHealthEffect(matchManagerScript.CrowdMultiplier); //possibly lose health
		matchManagerScript.CrowdMultiplier = matchManagerScript.Min_Crowd_Multiplier; //reset the multiplier to 1
		scoreManager.BonusFeedback(matchManagerScript.Min_Crowd_Multiplier);
	}
}
