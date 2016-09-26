using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class WinLoseManager : MonoBehaviour {

	public int scoreToWin = 500;
	public int ScoreToWin{
		get { return scoreToWin; }
		set { scoreToWin = value; }
	}


	private int currentHealth = 0;
	private Transform health;
	private ScoreManager scoreManager;
	private const string SCORE_CANVAS = "Score canvas";
	private const string SCORE = "Score";
	private const string LIFE_ICONS = "Life icons";


	private void Start(){
		health = transform.root.Find(SCORE_CANVAS).Find(LIFE_ICONS);
		scoreManager = transform.root.Find(SCORE_CANVAS).Find(SCORE).GetComponent<ScoreManager>();
	}

	public void TestWinOrLose(){
		currentHealth = DetermineCurrentHealth();

		if (currentHealth == 0){
			Lose();
		} else if (scoreManager.CurrentScore < ScoreToWin){ 
			Lose();
		} else if (scoreManager.CurrentScore > ScoreToWin){
			Win();
		}
	}

	private int DetermineCurrentHealth(){
		int temp = 0;

		foreach (Transform heart in health){
			if (heart.GetComponent<Image>().enabled) { temp++; }
		}

		return temp;
	}

	private void Lose(){
		Debug.Log("You lose!");
	}

	private void Win(){
		Debug.Log("You win!");
	}
}
