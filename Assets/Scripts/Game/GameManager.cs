using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public static int MaxLevel = 6;
	public static int SelectedLevel = 0;
	public static int CurrentLevel = 0;

	public static int[] Levels;

	public static void SelectLevel(int level)
	{
		GameManager.SelectedLevel = level;
		Application.LoadLevel(PrefKeys.GetLevel(level));
	}

	public static void SaveGame(int score)
	{
		var level = GameManager.SelectedLevel;
		var levelKey = PrefKeys.GetLevel(level);
		var maxScore = Mathf.Max(PlayerPrefs.GetInt(levelKey), score);

		PlayerPrefs.SetInt(levelKey, maxScore);

		if(level == GameManager.CurrentLevel)
		{
			var nextLevel = Mathf.Min(level + 1, GameManager.MaxLevel);
			PlayerPrefs.SetInt(PrefKeys.CurrentLevel, nextLevel);
			PlayerPrefs.SetInt(PrefKeys.GetLevel(nextLevel), 0);
			GameManager.CurrentLevel = nextLevel;
		}

		PlayerPrefs.Save();
	}

	public static void NextLevel()
	{
		if(SelectedLevel == MaxLevel)
		{
			Application.LoadLevel(Scenes.LevelSelect);
			return;
		}


		SelectLevel(SelectedLevel + 1);
		print ("pilih level: " + (SelectedLevel).ToString());
	}

	public static void InitGame()
	{
		if(!PlayerPrefs.HasKey(PrefKeys.CurrentLevel))
		{
			PlayerPrefs.SetInt(PrefKeys.CurrentLevel, 1);
			PlayerPrefs.SetInt(PrefKeys.GetLevel(1),  0);
			PlayerPrefs.Save();
		}

		GameManager.CurrentLevel = PlayerPrefs.GetInt(PrefKeys.CurrentLevel);
		GameManager.Levels = new int[GameManager.MaxLevel+1];

		for(int i=1;i<=GameManager.MaxLevel; i++)
		{
			var levelKey = PrefKeys.GetLevel(i);
			if(!PlayerPrefs.HasKey(levelKey))
			{
				PlayerPrefs.SetInt(levelKey, -1);
				PlayerPrefs.Save();
			}
			GameManager.Levels[i] = PlayerPrefs.GetInt(levelKey);
		}
	}
}
