using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HealthManager : MonoBehaviour {

	private Image[] hearts;
	private MatchManagerScript matchManager;
	private WinLoseManager winLoseManager;
	private const string GAME_MANAGER = "GameManager";

	private void Start(){
		hearts = new Image[transform.childCount];
		hearts = FindHearts();
		matchManager = transform.root.Find(GAME_MANAGER).GetComponent<MatchManagerScript>();
		winLoseManager = transform.root.Find(GAME_MANAGER).GetComponent<WinLoseManager>();
	}

	/// <summary>
	/// Get an array of the hearts used to measure life.
	/// </summary>
	/// <returns>An array containing all the hearts.</returns>
	private Image[] FindHearts(){

		Image[] temp = new Image[transform.childCount];

		for (int i = 0; i < transform.childCount; i++){
			temp[i] = transform.GetChild(i).GetComponent<Image>();
		}

		return temp;
	}

	/// <summary>
	/// Lose 1 health if this is called when the crowd multiplier is > 1. This function is called from
	/// CrowdAngerScript, which knows when the crowd's patience runs out.
	/// </summary>
	/// <param name="multiplier">The crowd multiplier.</param>
	public void DetermineHealthEffect(int multiplier){
		Debug.Log("DetermineHealthEffect() called; multiplier == " + multiplier);
		if (multiplier > matchManager.Min_Crowd_Multiplier){
			foreach (Image heart in hearts){
				if (heart.enabled){
					heart.enabled = false;
					break;
				}
			}
		}

		winLoseManager.TestWinOrLose();
	}
}
