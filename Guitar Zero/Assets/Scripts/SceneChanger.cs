using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneChanger : MonoBehaviour {

	const string TITLE_SCENE = "Title scene";
	const string GAME_SCENE = "GameScene";

	void Update(){
		if (Input.GetMouseButtonDown(0) && SceneManager.GetActiveScene().name == TITLE_SCENE){
			SceneManager.LoadScene(GAME_SCENE);
		}
	}
}
