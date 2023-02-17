using UnityEngine;

public class ScoreManager
{
    public void SaveScore(int levelNumber, int levelScore)
	{
		string levelKey = GetKey(levelNumber);
		PlayerPrefs.SetInt(levelKey, levelScore);
	}

	public int GetScore(int levelNumber)
	{
		string levelKey = GetKey(levelNumber);
		if (PlayerPrefs.HasKey(levelKey))
		{
			return PlayerPrefs.GetInt(levelKey);
		}
		else
		{
			return 0;
		}
	}

	private string GetKey(int levelNumber)
	{
		return string.Format("{0}-{1}", Constants.LevelScore, levelNumber.ToString());
	}
}
