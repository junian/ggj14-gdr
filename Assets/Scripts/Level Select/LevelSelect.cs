using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LevelSelect : MonoBehaviour {

	public int buttonWidth = 64;
	public int buttonHeight = 64;
	public int row = 4;
	public int column = 4;
	public int space = 16;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI()
	{
		int totalWidth = buttonWidth * column + (column - 1) * space;
		int totalHeight = buttonHeight * row + (row - 1) * space;

		int startX = (Screen.width - totalWidth)/2;
		int startY = (Screen.height - totalHeight)/2;

		int levelCounter = 1;

		for(int i=0;i<row;i++)
		{
			for(int j=0;j<column && GameManager.Levels[levelCounter] > -1;j++)
			{
				if(GUI.Button(new Rect(startX + j*buttonWidth + j*space,
				                       startY + i*buttonHeight + i*space,
				                       buttonWidth,
				                       buttonHeight), (levelCounter++).ToString()))
				{
					//Application.LoadLevel(PrefKeys.GetLevel(levelCounter++));
					if(GameManager.Levels[levelCounter-1] > -1)
					{
						GameManager.SelectLevel(levelCounter-1);
					}
					else
					{
						print ("masih locked");
					}
				}
			}
		}

		#if DEBUG || UNITY_EDITOR

		if(GUI.Button(new Rect(1,1,buttonWidth,buttonHeight), "Master"))
		{
			SceneManager.LoadScene(Scenes.Level00);
		}

		#endif
	}
}
