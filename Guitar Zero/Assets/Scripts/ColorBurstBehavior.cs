using UnityEngine;
using System.Collections;

public class ColorBurstBehavior : MonoBehaviour {

	float timer = 0.0f;
	float existDuration = 0.417f;

	void Start(){
		Animation anim = GetComponent<Animation>();
		existDuration = anim.clip.length;
	}

	void Update(){
		timer += Time.deltaTime;

		if (timer >= existDuration) { Destroy(gameObject); }
	}
}
