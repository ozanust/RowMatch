using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelsContent : MonoBehaviour
{
	[SerializeField]
	private LevelButtonItem levelButtonPrototype;
	[SerializeField]
	private DreamScrollContent content;

	private GameManager gameManager;
	private ScoreManager scoreManager = new ScoreManager();
	private Dictionary<int, LevelButtonItem> levelButtons = new Dictionary<int, LevelButtonItem>();

	void Start()
	{
		gameManager = GameManager.Instance;
		gameManager.OnLevelsDownloaded += FillContentWithLevelButtons;
		FillContentWithLevelButtons();
		UnlockLevelButtons();
	}

	private void FillContentWithLevelButtons()
	{
		foreach (LevelData ld in gameManager.LevelDatas.Values)
		{
			LevelButtonItem tempItem = Instantiate(levelButtonPrototype, content.transform);
			int score = scoreManager.GetScore(ld.LevelNumber);

			tempItem.SetupButton(ld.LevelNumber, ld.MoveCount, score, true);
			tempItem.OnClick += OnClickLevelButton;
			tempItem.gameObject.SetActive(true);
			tempItem.transform.localPosition = new Vector3(1.5f, 3 - (ld.LevelNumber + 0.2f * ld.LevelNumber), 0);

			if (!levelButtons.ContainsKey(ld.LevelNumber))
				levelButtons.Add(ld.LevelNumber, tempItem);
		}
	}

	private void UnlockLevelButtons()
	{
		// first level will always be unlocked
		int lastUnlockedLevel = 0;
		levelButtons[1].Unlock();
		foreach (LevelData ld in gameManager.LevelDatas.Values)
		{
			if (scoreManager.GetScore(ld.LevelNumber) > 0)
			{
				if (levelButtons.ContainsKey(ld.LevelNumber))
				{
					lastUnlockedLevel = ld.LevelNumber;
					levelButtons[ld.LevelNumber].Unlock();
				}
			}
		}

		if (levelButtons.ContainsKey(lastUnlockedLevel + 1))
		{
			levelButtons[lastUnlockedLevel + 1].Unlock();
		}
	}

	public void UnlockLevelButton(int levelNumber)
	{
		if (levelButtons.ContainsKey(levelNumber))
		{
			levelButtons[levelNumber].Unlock();
		}
	}

	private void OnClickLevelButton(int levelNumber)
	{
		gameManager.PlayLevel(levelNumber);
	}
}
