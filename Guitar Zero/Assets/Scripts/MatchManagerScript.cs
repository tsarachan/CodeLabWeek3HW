using UnityEngine;
using System.Collections;

public class MatchManagerScript : MonoBehaviour {

	protected GameManagerScript gameManager;    //"protected" means this field is public to child scripts
	//but not to unrelated scripts
	protected ScoreManager scoreManager;

	GameObject colorBurst;
	const int CHORD_SIZE = 3;

	public virtual void Awake (){
		gameManager = GetComponent<GameManagerScript>();
		scoreManager = transform.root.Find("Score canvas").Find("Score").GetComponent<ScoreManager>();
		colorBurst = Resources.Load("Color burst") as GameObject;
	}

	/// <summary>
	/// Checks the entire grid for matches.
	/// </summary>
	/// 
	/// <returns><c>true</c>, if there are any matches, <c>false</c> otherwise.</returns>
	public virtual bool GridHasMatch(){
		bool match = false; //assume there is no match

		//check each square in the grid
		for(int x = 0; x < gameManager.gridWidth; x++){
			for(int y = 0; y < gameManager.gridHeight ; y++){
				if (x < gameManager.gridWidth - 2){	//GridHasHorizontalMatch checks 2 to the right
					//gameManager.gridWidth - 2 ensures you're never extending into
					//a space that doesn't exist
					match = match || GridHasAChord(x, y); //if match was ever set to true, it stays true forever
				}

				if (x < gameManager.gridWidth - 3 && y < gameManager.gridHeight - 2){
					match = match || GridHasCChord(x, y);
				}

				if (x < gameManager.gridWidth - 5 && y < gameManager.gridHeight - 1){
					match = match || GridHasGChord(x, y);
				}
			}
		}

		return match;
	}

	public bool GridHasAChord(int x, int y){
		GameObject token1 = gameManager.gridArray[x + 0, y + 0];
		GameObject token2 = gameManager.gridArray[x + 1, y + 0];
		GameObject token3 = gameManager.gridArray[x + 2, y + 0];

		if(token1 != null && token2 != null && token3 != null){ //ensure all of the token exists
			SpriteRenderer sr1 = token1.GetComponent<SpriteRenderer>();
			SpriteRenderer sr2 = token2.GetComponent<SpriteRenderer>();
			SpriteRenderer sr3 = token3.GetComponent<SpriteRenderer>();

			return (sr1.sprite == sr2.sprite && sr2.sprite == sr3.sprite);  //compare their sprites
			//to see if they're the same
		} else {
			return false;
		}
	}

	public bool GridHasCChord(int x, int y){
		GameObject token1 = gameManager.gridArray[x + 0, y + 0];
		GameObject token2 = gameManager.gridArray[x + 1, y + 1];
		GameObject token3 = gameManager.gridArray[x + 3, y + 2];

		if(token1 != null && token2 != null && token3 != null){ //ensure all of the token exists
			SpriteRenderer sr1 = token1.GetComponent<SpriteRenderer>();
			SpriteRenderer sr2 = token2.GetComponent<SpriteRenderer>();
			SpriteRenderer sr3 = token3.GetComponent<SpriteRenderer>();

			return (sr1.sprite == sr2.sprite && sr2.sprite == sr3.sprite);  //compare their sprites
			//to see if they're the same
		} else {
			return false;
		}
	}

	public bool GridHasGChord(int x, int y){
		GameObject token1 = gameManager.gridArray[x + 0, y + 0];
		GameObject token2 = gameManager.gridArray[x + 1, y + 1];
		GameObject token3 = gameManager.gridArray[x + 5, y + 0];

		if(token1 != null && token2 != null && token3 != null){ //ensure all of the token exists
			SpriteRenderer sr1 = token1.GetComponent<SpriteRenderer>();
			SpriteRenderer sr2 = token2.GetComponent<SpriteRenderer>();
			SpriteRenderer sr3 = token3.GetComponent<SpriteRenderer>();

			return (sr1.sprite == sr2.sprite && sr2.sprite == sr3.sprite);  //compare their sprites
			//to see if they're the same
		} else {
			return false;
		}
	}

	/// <summary>
	/// Destroys all tokens in a match of three or more
	/// </summary>
	/// <returns>The number of tokens destroyed.</returns>
	public virtual int RemoveMatches(){
		int numRemoved = 0;

		//iterate across entire grid, looking for matches
		//wherever a chord is found, respond
		for(int x = 0; x < gameManager.gridWidth; x++){
			for(int y = 0; y < gameManager.gridHeight ; y++){

				if (x < gameManager.gridWidth - 2){
					if (GridHasAChord(x, y)){
						RemoveChord(x, y, 'A');
						numRemoved += CHORD_SIZE;
					}
				}

				if (x < gameManager.gridWidth - 3 && y < gameManager.gridHeight - 2){
					if (GridHasCChord(x, y)){
						RemoveChord(x, y, 'C');
						numRemoved += CHORD_SIZE;
					}
				}

				if (x < gameManager.gridWidth - 5 && y < gameManager.gridHeight - 1){
					if (GridHasGChord(x, y)){
						RemoveChord(x, y, 'G');
						numRemoved += CHORD_SIZE;
					}
				}
			}
		}

		if (numRemoved > 3){
			
			scoreManager.BonusFeedback(scoreManager.BonusIncrement * numRemoved);
			scoreManager.UpdateScore(scoreManager.BonusIncrement * numRemoved);
		}

		return numRemoved;
	}

	protected void RemoveChord(int x, int y, char chord){
		GameObject token1 = gameManager.gridArray[x, y];
		gameManager.gridArray[x, y] = null;


		GameObject token2 = null;
		GameObject token3 = null;

		switch(chord){
			case 'A':
				token2 = gameManager.gridArray[x + 1, y + 0];
				gameManager.gridArray[x + 1, y + 0] = null;
				token3 = gameManager.gridArray[x + 2, y + 0];
				gameManager.gridArray[x + 2, y + 0] = null;
				break;
			case 'C':
				token2 = gameManager.gridArray[x + 1, y + 1];
				gameManager.gridArray[x + 1, y + 1] = null;
				token3 = gameManager.gridArray[x + 3, y + 2];
				gameManager.gridArray[x + 3, y + 2] = null;
				break;
			case 'G':
				token2 = gameManager.gridArray[x + 1, y + 1];
				gameManager.gridArray[x + 1, y + 1] = null;
				token3 = gameManager.gridArray[x + 5, y + 0];
				gameManager.gridArray[x + 5, y + 0] = null;
				break;
			default:
				Debug.Log("Illegal chord: " + chord);
				break;
		}
			
		ChordFeedback(new GameObject[] { token1, token2, token3 });

		Destroy(token1);
		Destroy(token2);
		Destroy(token3);

	}

	protected void ChordFeedback(GameObject[] tokens){
		foreach (GameObject token in tokens){
			ProvideFeedback(token.transform.position, CHORD_SIZE);
		}
	}

	protected void ProvideFeedback(Vector3 loc, int multiplier){
		scoreManager.UpdateScore(scoreManager.BasicIncrement * multiplier);
		scoreManager.LocalizedFeedback(scoreManager.BasicIncrement * multiplier, loc);
		Instantiate(colorBurst, loc, Quaternion.identity);
	}
}
