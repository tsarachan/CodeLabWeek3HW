using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class CreditSceneLoader : MonoBehaviour {

	private const string TITLE_SCENE_NAME = "Title scene";

	public void LoadTitleScene(){
		SceneManager.LoadScene(TITLE_SCENE_NAME);
	}
}
