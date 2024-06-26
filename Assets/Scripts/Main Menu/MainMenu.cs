﻿using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

	private int buttonWidth = 128;
	private int buttonHeight = 64;

	void Awake()
	{
		GameManager.InitGame();
	}

	void OnGUI()
	{
		if(GUI.Button(new Rect((Screen.width - buttonWidth)/2,
		                       (Screen.height - buttonHeight)/2,
		                       buttonWidth,
		                       buttonHeight), "Start Game"))
		{
			SceneManager.LoadScene(Scenes.LevelSelect);
		}
	}
}
