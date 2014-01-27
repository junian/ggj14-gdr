using UnityEngine;
using System.Collections;

public class Game : MonoBehaviour {

	[HideInInspector]
	public int score = 0;

	[HideInInspector]
	public int totalLove = 3;

	public PlayerController playerMale;
	public PlayerController playerFemale;

	public Texture2D loveTexture;
	public Texture2D emptyLoveTexture;

	[HideInInspector]
	public GameState gameState;
	private int maxScore;

	private static Game instance;
	public static Game Instance
	{
		get
		{
			return Game.instance;
		}
	}

	void Awake()
	{
		//if(instance == null)
		//{
			instance = this;
		//}
		score = 0;
		gameState = GameState.Playing;
		playerMale.PlayerMeet += HandlePlayerMeet;
		var objs = GameObject.FindGameObjectsWithTag(Tags.Coin);
		maxScore = objs.Length + 1;
	}

	void HandlePlayerMeet (object sender, System.EventArgs e)
	{
		if(gameState == GameState.Playing)
		{
			gameState = GameState.Win;
			GameManager.SaveGame(score);
		}
	}

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI()
	{
		var width = loveTexture.width;
		var height = loveTexture.height;

		var startX = (Screen.width - width * totalLove) / 2.0f;
		var startY = (Screen.height-height)/2.0f;

		if(playerMale.playerState == PlayerState.Win && playerFemale.playerState == PlayerState.Win)
		{
			var theScore = ((score - 1) * totalLove) / maxScore;
			for(int i=1; i <= totalLove; i++)
			{
				var rect = new Rect(startX + (i-1)*width, 0.5f*startY, width, height);
				if(i <= theScore)
				{
					GUI.DrawTexture(rect, loveTexture);
				}
				else
				{
					GUI.DrawTexture(rect, emptyLoveTexture);
				}
			}

			var buttonWidth = 1.5f * width;
			var buttonHeight = 48;
			var btnRect = new Rect(startX, startY, buttonWidth, buttonHeight);

			if(GUI.Button(btnRect, "Main Menu"))
			{
				Application.LoadLevel(Scenes.LevelSelect);
			}

			btnRect = new Rect(startX + buttonWidth, startY, buttonWidth, buttonHeight);

			if(GUI.Button(btnRect, "Next Level"))
			{
				//print ("next level");
				GameManager.NextLevel();
			}
		}
	}
}
