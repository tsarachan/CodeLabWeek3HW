using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class TitleButtonsScript : MonoBehaviour {

	private const string GAME_SCENE = "GameScene";
	private const string CREDITS_SCENE = "Credits scene";
	private const string HOW_TO_PLAY_SCENE = "How to play scene";

	public void LoadGame(){
		SceneManager.LoadScene(GAME_SCENE);
	}

	public void LoadCredits(){
		SceneManager.LoadScene(CREDITS_SCENE);
	}

	public void LoadHowToPlay(){
		SceneManager.LoadScene(HOW_TO_PLAY_SCENE);
	}
}
