using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreManager : MonoBehaviour {

	private Text score;

	private int currentScore = 0;
	public int CurrentScore{
		get { return currentScore; }
	}
	public int maxScore = 9999;

	private int basicIncrement = 1;
	public int BasicIncrement {
		get { return basicIncrement; }
		set { basicIncrement = value; }
	}

	private int bonusIncrement = 100;
	public int BonusIncrement {
		get { return bonusIncrement; }
		set { bonusIncrement = value; }
	}

	private GameObject scoreFeedback;
	GameObject bonusFeedback;

	private float canvasX;
	private float canvasY;

	private const float BONUS_X_LOC = -66.0f;
	private const float BONUS_Y_LOC = 183.0f;

	private void Start() {
		score = GetComponent<Text>();
		score.text = currentScore.ToString();
		scoreFeedback = Resources.Load("Score feedback") as GameObject;
		bonusFeedback = Resources.Load("Bonus feedback") as GameObject;

		RectTransform overlayCanvas = transform.root.Find("Score canvas").GetComponent<RectTransform>();
		canvasX = overlayCanvas.anchoredPosition.x;
		canvasY = overlayCanvas.anchoredPosition.y;
	}

	public void UpdateScore(int amount){
		currentScore += amount;
		if (currentScore <= maxScore){
			score.text = currentScore.ToString();
		} else {
			currentScore = currentScore - maxScore;
			score.text = currentScore.ToString();
		}
	}

	public void LocalizedFeedback(int amount, Vector3 loc){
		GameObject newScoreFeedback = Instantiate(scoreFeedback,
												  new Vector3(0.0f, 0.0f, 0.0f),
												  Quaternion.identity) as GameObject;
		
		Vector3 pos = Camera.main.WorldToScreenPoint(loc);
		Vector3 correctedPos = new Vector3(pos.x - canvasX,
										   pos.y - canvasY,
										   pos.z);
		newScoreFeedback.GetComponent<RectTransform>().anchoredPosition = correctedPos;

		newScoreFeedback.transform.SetParent(transform.root.Find("Score canvas"), false);
		newScoreFeedback.GetComponent<Text>().text = amount.ToString();
	}

	public void BonusFeedback(int amount){
		GameObject newBonusFeedback = Instantiate(bonusFeedback, transform.root.Find("Score canvas")) as GameObject;

		newBonusFeedback.GetComponent<RectTransform>().anchoredPosition = new Vector3(BONUS_X_LOC,
																					  BONUS_Y_LOC,
																					  0.0f);

		newBonusFeedback.GetComponent<Text>().text = "x" + amount.ToString() + " multiplier";
	}
}
