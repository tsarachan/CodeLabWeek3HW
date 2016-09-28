using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneChanger : MonoBehaviour {

	private const string TITLE_SCENE = "Title scene";
	private const string GAME_SCENE = "GameScene";
	private const string CREDITS_SCENE = "Credits scene";
	private void Update(){
		if (Input.GetMouseButtonDown(0)){
			SceneManager.LoadScene(TITLE_SCENE);
		} else if (Input.GetMouseButtonDown(1)){
			SceneManager.LoadScene(CREDITS_SCENE);
		}
	}
}
