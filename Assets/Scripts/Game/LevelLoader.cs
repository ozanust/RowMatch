using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class LevelLoader
{
	char[] dataSeparators = new char[] { ':', '\n' };
	char[] gridSeparator = new char[] { ',' };
	Dictionary<string, string> levelDataKeyValues = new Dictionary<string, string>();

	public LevelData Get(int index)
	{
		string path = Path.Combine(Application.dataPath, "Resources", "Levels", index.ToString());
		string rawData = File.ReadAllText(path);

		return GetLevelDataFromString(rawData);
	}

	public LevelData[] GetAllLevels()
	{
		List<LevelData> levelDataList = new List<LevelData>();

		TextAsset[] levelAssets = Resources.LoadAll<TextAsset>("Levels");
		for (int i = 0; i < levelAssets.Length; i++)
		{
			levelDataList.Add(GetLevelDataFromString(levelAssets[i].text));
		}

		string path = Path.Combine(Application.persistentDataPath, "Levels");

		if (Directory.Exists(path))
		{
			List<string> rawDataList = new List<string>();
			foreach (string file in Directory.EnumerateFiles(path))
			{
				string contents = File.ReadAllText(file);
				rawDataList.Add(contents);
			}

			for (int i = 0; i < rawDataList.Count; i++)
			{
				levelDataList.Add(GetLevelDataFromString(rawDataList[i]));
			}
		}

		return levelDataList.ToArray();
	}

	private LevelData GetLevelDataFromString(string rawData)
	{
		string trimmedData = rawData.Replace(" ", string.Empty);
		string[] splittedData = trimmedData.Split(dataSeparators, System.StringSplitOptions.RemoveEmptyEntries);

		for (int i = 0; i < splittedData.Length / 2; i++)
		{
			levelDataKeyValues.Add(splittedData[i * 2], splittedData[2 * i + 1]);
		}

		int levelNumber = 0;
		int width = 0;
		int height = 0;
		int moveCount = 0;
		string[] grid = null;

		if (levelDataKeyValues.ContainsKey(Constants.LevelNumber))
		{
			levelNumber = System.Convert.ToInt32(levelDataKeyValues[Constants.LevelNumber]);
			width = System.Convert.ToInt32(levelDataKeyValues[Constants.GridWidth]);
			height = System.Convert.ToInt32(levelDataKeyValues[Constants.GridHeight]);
			moveCount = System.Convert.ToInt32(levelDataKeyValues[Constants.MoveCount]);
			grid = levelDataKeyValues[Constants.Grid].Split(gridSeparator, System.StringSplitOptions.RemoveEmptyEntries);
		}

		LevelData data;
		data.LevelNumber = levelNumber;
		data.Width = width;
		data.Height = height;
		data.MoveCount = moveCount;
		data.Grid = grid;
		levelDataKeyValues.Clear();

		return data;
	}
}

public struct LevelData
{
	public int LevelNumber;
	public int Width;
	public int Height;
	public int MoveCount;
	public string[] Grid;
}
