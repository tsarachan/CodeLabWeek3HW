using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class WinLoseManager : MonoBehaviour {

	public int scoreToWin = 500;
	public int ScoreToWin{
		get { return scoreToWin; }
		set { scoreToWin = value; }
	}
	private GameManagerScript gameManager;


	private int currentHealth = 0;
	private Transform health;
	private ScoreManager scoreManager;
	private const string SCORE_CANVAS = "Score canvas";
	private const string SCORE = "Score";
	private const string LIFE_ICONS = "Life icons";


	private void Start(){
		health = transform.root.Find(SCORE_CANVAS).Find(LIFE_ICONS);
		scoreManager = transform.root.Find(SCORE_CANVAS).Find(SCORE).GetComponent<ScoreManager>();
		gameManager = GetComponent<GameManagerScript>();
	}


	/// <summary>
	/// Checks whether the player has done something that would cause them to win or lose. Called by HealthManager
	/// and GameManager.
	/// </summary>
	public void TestWinOrLose(){
		Debug.Log("TestWinOrLose() called");
		currentHealth = DetermineCurrentHealth();
		Debug.Log("currentHealth == " + currentHealth);

		if (currentHealth == 0){
			Lose();
		} else if (scoreManager.CurrentScore < ScoreToWin && gameManager.ChordsPlayed >= gameManager.NumChordsInSong){ 
			Lose();
		} else if (scoreManager.CurrentScore > ScoreToWin && gameManager.ChordsPlayed >= gameManager.NumChordsInSong){
			Win();
		}
	}


	/// <summary>
	/// Determines the player's current health.
	/// </summary>
	/// <returns>Player's current health.</returns>
	private int DetermineCurrentHealth(){
		int temp = 0;

		foreach (Transform heart in health){
			if (heart.GetComponent<Image>().enabled) { temp++; }
		}

		return temp;
	}


	/// <summary>
	/// What happens when the player loses?
	/// </summary>
	private void Lose(){
		Debug.Log("You lose!");
	}


	/// <summary>
	/// What happens when the player wins?
	/// </summary>
	private void Win(){
		Debug.Log("You win!");
	}
}
