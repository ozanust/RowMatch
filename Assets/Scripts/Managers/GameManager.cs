using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : SingletonManager<GameManager>
{
	[SerializeField]
	private LevelURLLib levelURLs;
	private Dictionary<int, LevelData> levelDataIndex = new Dictionary<int, LevelData>();

	LevelLoader levelLoader = new LevelLoader();
	LevelDownloader levelDownloader;

	public Action<LevelData> OnStartLevel;
	public Action OnLevelsDownloaded;
	private LevelData currentLevelData;
	public LevelData CurrentLevelData => currentLevelData;
	public Dictionary<int, LevelData> LevelDatas => levelDataIndex;
	public LevelEndData levelEndData;

	private void Awake()
	{
		DontDestroyOnLoad(this);
		levelDownloader = new LevelDownloader(NetworkService.Instance, levelURLs);
	}

	void Start()
	{
		//PlayerPrefs.DeleteAll();
		CacheLevelDatas();

		if (SceneManager.GetActiveScene().buildIndex == 0)
			SceneManager.LoadScene(Constants.MenuSceneIndexNumber);
	}

	private void CheckAndDownloadMissingLevels(Action doExtraAfterDownload = null)
	{
		Debug.Log(levelDataIndex.Count);
		if (levelDataIndex.Count < levelURLs.levelURLs.Length)
		{
			levelDownloader.DownloadMissingLevels();
			levelDownloader.OnAllLevelsDownloaded += OnAllLevelsDownloaded;

			if (doExtraAfterDownload != null)
				levelDownloader.OnAllLevelsDownloaded += doExtraAfterDownload;
		}
	}

	private void OnAllLevelsDownloaded()
	{
		CacheLevelDatas();
	}

	private void CacheLevelDatas()
	{
		LevelData[] levelDatas = levelLoader.GetAllLevels();
		Debug.Log(levelDatas.Length);
		foreach (LevelData ld in levelDatas)
		{
			if (!levelDataIndex.ContainsKey(ld.LevelNumber))
				levelDataIndex.Add(ld.LevelNumber, ld);
		}

		CheckAndDownloadMissingLevels();
	}

	public void PlayLevel(int levelNumber)
	{
		Debug.Log("play level: " + levelNumber);
		if (levelDataIndex.ContainsKey(levelNumber))
		{
			LevelData data = levelDataIndex[levelNumber];
			currentLevelData = data;
			OnStartLevel?.Invoke(data);
			SceneManager.LoadScene(Constants.GameSceneIndexNumber);
		}
		else
		{
			//show UI warning
			Debug.Log("no level in cache");
			CheckAndDownloadMissingLevels(() =>
			{
				if (levelDataIndex.ContainsKey(levelNumber))
				{
					LevelData data = levelDataIndex[levelNumber];
					currentLevelData = data;
					OnStartLevel?.Invoke(data);
					SceneManager.LoadScene(Constants.GameSceneIndexNumber);
				}
			});
		}
	}
}
