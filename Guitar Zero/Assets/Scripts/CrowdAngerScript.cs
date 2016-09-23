using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CrowdAngerScript : MonoBehaviour {

	private Image meter;
	public float angerDelay = 2.0f; //total amount of time it takes for the meter to reach zero
	private float angerTimer = 2.0f;

	private void Start(){
		meter = GetComponent<Image>();
	}

	private void Update(){
		angerTimer -= Time.deltaTime;

		meter.fillAmount = angerTimer/angerDelay;

		if (angerTimer <= 0.0f){
			angerTimer = angerDelay;
		}
	}
}
