﻿using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using System.Collections;

public class GameManagerScript : MonoBehaviour {

	public int gridWidth = 8;
	public int gridHeight = 8;
	public float tokenSize = 1;

	private const string SCORE_CANVAS = "Score canvas";
	private const string CHORDS_REMAINING = "Chords remaining";
	private const string SCORE_REQUIRED = "Score to win";
	protected MatchManagerScript matchManager;
	protected InputManagerScript inputManager;
	protected RepopulateScript repopulateManager;
	protected MoveTokensScript moveTokenManager;
	protected ScoreManager scoreManager;

	public GameObject grid;

	//we're declaring a MULTI-DIMENSIONAL ARRAY; this array has two dimensions, so it's a GRID, not a line
	//that will make it easy to track tokens in a grid (which is a two-dimensional shape)
	public GameObject[,] gridArray;

	protected UnityEngine.Object[] tokenTypes;
	protected Sprite[] sprites;

	protected GameObject stringGraphic;

	protected const string INSTRUCTION_SCENE_NAME = "Instruction scene";

	public int numChordsInSong = 50;
	public int NumChordsInSong{
		get { return numChordsInSong; }
	}
	private WinLoseManager winLoseManager;
	private int chordsPlayed = 0;
	public int ChordsPlayed{
		get { return chordsPlayed; }
		set{
			chordsPlayed = value;
			chordsRemaining.text = NumChordsInSong - chordsPlayed + " chords remaining";
		}
	}
	private Text chordsRemaining;
	private Text scoreRequired;


	public virtual void Start () {
		//load the tokens, make the grid, and create references to the other scripts
		tokenTypes = (UnityEngine.Object[])Resources.LoadAll("Tokens/");
		sprites = Resources.LoadAll<Sprite>("Sprites/");
		gridArray = new GameObject[gridWidth, gridHeight];
		matchManager = GetComponent<MatchManagerScript>();
		inputManager = GetComponent<InputManagerScript>();
		repopulateManager = GetComponent<RepopulateScript>();
		moveTokenManager = GetComponent<MoveTokensScript>();
		scoreManager = transform.root.Find("Score canvas").Find("Score").GetComponent<ScoreManager>();
		stringGraphic = Resources.Load("String") as GameObject;
		MakeGrid();
		ChangeGridDuplicates();
		SceneManager.LoadScene(INSTRUCTION_SCENE_NAME, LoadSceneMode.Additive);
		winLoseManager = GetComponent<WinLoseManager>();
		chordsRemaining = transform.root.Find(SCORE_CANVAS).Find(CHORDS_REMAINING).GetComponent<Text>();
		ChordsPlayed = 0; //set the text correctly at the start of the game
		scoreRequired = transform.root.Find(SCORE_CANVAS).Find(SCORE_REQUIRED).GetComponent<Text>();
		scoreRequired.text = winLoseManager.ScoreToWin + " to win";
	}

	public virtual void Update(){
		//every frame, check whether the grid is full of tokens.

		if(!GridHasEmpty()){
			//if the grid is full of tokens and has matches, remove them.
			if(matchManager.GridHasMatch()){
				ChordsPlayed += matchManager.RemoveMatches();
				if (ChordsPlayed >= numChordsInSong) { winLoseManager.TestWinOrLose(); }
			} else {
				//if the grid is full and there are no matches, wait for the player to make a move (and look for it in InputManager)
				inputManager.SelectToken();
			}

		} else {
			if(!moveTokenManager.move){
				//if the icons are currently moving, set them up to move and leave it be
				moveTokenManager.SetupTokenMove();
			}
			if(!moveTokenManager.MoveTokensToFillEmptySpaces()){
				//if the MoveTokenManager hasn't added any tokens to the grid
				//tell Repopulate Script to add new tokens
				repopulateManager.AddNewTokensToRepopulateGrid();
			}
		}
	}
		
	/// <summary>
	/// This creates the grid, fills it with tokens, and makes the GameObjects children of a new GameObject, TokenGrid.
	/// It looks like this should only be called once a game.
	/// </summary>
	void MakeGrid(){
		//creates a GameObject called TokenGrid, then makes all of the tokens as children of it
		//this keeps our hierarchy neat
		grid = new GameObject("TokenGrid");
		//then, fill the grid with tokens
		for(int x = 0; x < gridWidth; x++){

			GameObject newStringGraphic = Instantiate(stringGraphic,
													  new Vector3(x - gridWidth/2 * tokenSize,
																  0.0f,
																  0.0f),
													  Quaternion.identity) as GameObject;
			newStringGraphic.transform.parent = transform.root.Find("Strings");

			for(int y = 0; y < gridHeight; y++){
				AddTokenToPosInGrid(x, y, grid);
			}
		}
	}

	//checks whether there is an empty space in the grid
	public virtual bool GridHasEmpty(){
		//this checks every x and y in the grid
		for(int x = 0; x < gridWidth; x++){
			for(int y = 0; y < gridHeight ; y++){
				if(gridArray[x, y] == null){
					//if this is an empty space, return true (so that we spawn more tokens)
					return true;
				}
			}
		}
		//if all spots in the grid are filled, we returned false (and will spawn no tokens)
		return false;
	}

	/// <summary>
	/// Gets the position of a specific token
	/// </summary>
	/// 
	/// <param name="token">A token GameObject, 'cause Matt likes sports</param>
	/// <returns>The Vector2 coordinate of a token in the grid</returns>
	public Vector2 GetPositionOfTokenInGrid(GameObject token){
		for(int x = 0; x < gridWidth; x++){
			for(int y = 0; y < gridHeight ; y++){
				if(gridArray[x, y] == token){
					return(new Vector2(x, y));
				}
			}
		}
		return new Vector2();
	}
		

	/// <summary>
	/// Converts a grid position into a Worldspace Position
	/// </summary>
	/// 
	/// <param name="x">An int x that is the x coordinate in the grid</param>
	/// <param name="y">An int y that is the y coordinate in the grid</param>
	/// <returns>A Vector2 coordinate in Worldspace</returns>
	public Vector2 GetWorldPositionFromGridPosition(int x, int y){
		return new Vector2(
			(x - gridWidth/2) * tokenSize,
			(y - gridHeight/2) * tokenSize);
	}


	/// <summary>
	/// This creates a random token and puts it into the grid at the given coordinate
	/// (It also makes the token a child of the grid, to keep the hierarchy tidy.)
	/// </summary>
	/// 
	/// <param name="x">An int x that is the x coordinate in the grid</param>
	/// <param name="y">An int y that is the y coordinate in the grid</param>
	/// <param name="parent">The parent of this token. It should be TokenGrid</param> 
	public void AddTokenToPosInGrid(int x, int y, GameObject parent){
		Vector3 position = GetWorldPositionFromGridPosition(x, y);
		GameObject token = 
			//we create a random kind of token, at that exact position in the grid, with the same rotation as its parent (TokenGrid)
			Instantiate(tokenTypes[UnityEngine.Random.Range(0, tokenTypes.Length)], 
			            position, 
			            Quaternion.identity) as GameObject;
		token.transform.parent = parent.transform;
		//then, we put this token into the array of tokens
		gridArray[x, y] = token;
	}

	protected void ChangeGridDuplicates(){
		bool foundDuplicates = false;

		for (int x = 0; x < gridWidth; x++){
			for (int y = 0; y < gridHeight; y++){
				if (x < gridWidth - 2){	//GridHasHorizontalMatch checks 2 to the right
					if (matchManager.GridHasAChord(x, y)){
						gridArray[x, y].GetComponent<SpriteRenderer>().sprite = ChangeSprite(x, y);
						foundDuplicates = true;
					}
				}

				if (x < gridWidth - 3 && y < gridHeight - 2){
					if (matchManager.GridHasCChord(x, y)){
						gridArray[x, y].GetComponent<SpriteRenderer>().sprite = ChangeSprite(x, y);
						foundDuplicates = true;
					}
				}

				if (x < gridWidth - 5 && y < gridHeight - 1){
					if (matchManager.GridHasGChord(x, y)){
						gridArray[x, y].GetComponent<SpriteRenderer>().sprite = ChangeSprite(x, y);
						foundDuplicates = true;
					}
				}
			}
		}

		if (foundDuplicates) { ChangeGridDuplicates(); }
	}

	protected Sprite ChangeSprite(int x, int y){
		int index = Array.IndexOf(sprites, gridArray[x, y].GetComponent<SpriteRenderer>().sprite);
		index++;
		if (index > sprites.Length - 1) { index = 0; }
		return sprites[index];
	}
}
