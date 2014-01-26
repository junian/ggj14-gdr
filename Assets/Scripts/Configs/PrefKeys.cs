public class PrefKeys {

	public const string CurrentLevel = "CurrentLevel";
	public const string LevelPrefix = "Level";

	public static string GetLevel(int level)
	{
		return LevelPrefix + level.ToString("00");
	}
}
