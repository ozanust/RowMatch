using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelDownloader
{
	private NetworkService networkService;
	private LevelURLLib levelURLs;
	private int downloadedLevelCount = 0;

	private Queue<string> levelRequestQueue;

	public Action OnNewLevelDownloaded;
	public Action OnAllLevelsDownloaded;

	public LevelDownloader(NetworkService networkService, LevelURLLib levelURLs)
	{
		this.networkService = networkService;
		this.levelURLs = levelURLs;

		InitializeRequestQueue();

		if (PlayerPrefs.HasKey(Constants.DownloadedLevelCount))
			downloadedLevelCount = PlayerPrefs.GetInt(Constants.DownloadedLevelCount);
	}

	public void DownloadMissingLevels()
	{
		if (Application.internetReachability != NetworkReachability.NotReachable)
		{
			networkService.DownloadLevelDataToStorage(levelRequestQueue.Dequeue(), OnResponse);
		}
		else
		{
			networkService.RequestInternetAvailabilityCheck(OnInternetConnectionEstablished);
		}
	}

	private void InitializeRequestQueue()
	{
		levelRequestQueue = new Queue<string>();
		for (int i = downloadedLevelCount; i < levelURLs.levelURLs.Length; i++)
		{
			levelRequestQueue.Enqueue(levelURLs.levelURLs[i]);
		}
	}

	private void OnResponse(DownloadFileResponse response)
	{
		if (response.IsSuccess)
		{
			OnNewLevelDownloaded?.Invoke();
			downloadedLevelCount++;
			PlayerPrefs.SetInt(Constants.DownloadedLevelCount, downloadedLevelCount);

			if (levelRequestQueue.Count > 0)
			{
				if (Application.internetReachability != NetworkReachability.NotReachable)
				{
					DownloadMissingLevels();
				}
				else
				{
					OnLevelDownloadFailed();
				}
			}
			else
			{
				OnAllLevelsDownloaded?.Invoke();
			}
		}
		else
		{
			OnLevelDownloadFailed();
			Debug.LogError(response.Error);
		}
	}

	private void OnLevelDownloadFailed()
	{
		if (Application.internetReachability != NetworkReachability.NotReachable)
		{
			DownloadMissingLevels();
		}
		else
		{
			networkService.RequestInternetAvailabilityCheck(OnInternetConnectionEstablished);
		}
	}

	private void OnInternetConnectionEstablished()
	{
		DownloadMissingLevels();
	}
}
