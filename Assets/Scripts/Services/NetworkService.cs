using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkService : SingletonManager<NetworkService>
{
	private WaitForSeconds internetCheckInterval = new WaitForSeconds(5);
	private Coroutine internetCheckCoroutine = null;

	private void Awake()
	{
		DontDestroyOnLoad(this);
	}

	IEnumerator DownloadLevelAsText(string levelUrl, Action<DownloadTextResponse> onResponse)
	{
		UnityWebRequest req = UnityWebRequest.Get(levelUrl);
		yield return req.SendWebRequest();

		DownloadTextResponse networkResponse;

		networkResponse.Error = req.error;
		networkResponse.Answer = req.downloadHandler.text;
		networkResponse.Data = req.downloadHandler.data;
		networkResponse.IsSuccess = req.result == UnityWebRequest.Result.Success;

		onResponse?.Invoke(networkResponse);
	}

	IEnumerator DownloadLevelToStorage(string levelUrl, Action<DownloadFileResponse> onResponse)
	{
		var req = UnityWebRequest.Get(levelUrl);
		string path = Path.Combine(Application.persistentDataPath, "Levels", levelUrl.Split('_')[1]);
		req.downloadHandler = new DownloadHandlerFile(path);
		yield return req.SendWebRequest();

		DownloadFileResponse networkResponse;

		networkResponse.Error = req.error;
		networkResponse.IsSuccess = req.result == UnityWebRequest.Result.Success;

		onResponse?.Invoke(networkResponse);
	}

	IEnumerator CheckInternet(Action onInternetAvailable)
	{
		while (Application.internetReachability == NetworkReachability.NotReachable)
		{
			yield return internetCheckInterval;
		}

		onInternetAvailable?.Invoke();
		internetCheckCoroutine = null;
	}

	public void DownloadLevelDataAsText(string levelUrl,  Action<DownloadTextResponse> onSuccess)
	{
		StartCoroutine(DownloadLevelAsText(levelUrl, onSuccess));
	}

	public void DownloadLevelDataToStorage(string levelUrl, Action<DownloadFileResponse> onSuccess)
	{
		StartCoroutine(DownloadLevelToStorage(levelUrl, onSuccess));
	}

	public void RequestInternetAvailabilityCheck(Action onInternetAvailable)
	{
		if (internetCheckCoroutine == null)
			internetCheckCoroutine = StartCoroutine(CheckInternet(onInternetAvailable));
	}
}

public struct DownloadTextResponse
{
	public bool IsSuccess;
	public string Answer;
	public string Error;
	public byte[] Data;
}

public struct DownloadFileResponse
{
	public bool IsSuccess;
	public string Error;
}
