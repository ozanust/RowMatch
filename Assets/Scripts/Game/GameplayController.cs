using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameplayController : MonoBehaviour
{
	[SerializeField]
	private BoardController boardController;

	private GameManager gameManager;
	private ScoreManager scoreManager = new ScoreManager();
	
	private int currentLevelScore;
	private LevelData currentLevelData;

	// Start is called before the first frame update
	void Awake()
	{
		gameManager = GameManager.Instance;
		OnStartLevel(gameManager.CurrentLevelData);

		boardController.NoMoveLeft += OnGameFinish;
		boardController.BoardCompleted += OnGameFinish;
		boardController.RowCompleted += OnRowCompleted;
	}

	private void OnStartLevel(LevelData data)
	{
		currentLevelScore = 0;
		currentLevelData = data;
		boardController.CreateBoard(data);
	}

	private void OnGameFinish()
	{
		if (scoreManager.GetScore(currentLevelData.LevelNumber) < currentLevelScore)
		{
			gameManager.levelEndData = new LevelEndData(currentLevelData.LevelNumber, true);
			scoreManager.SaveScore(currentLevelData.LevelNumber, currentLevelScore);
		}
		else
		{
			gameManager.levelEndData = new LevelEndData(currentLevelData.LevelNumber, false);
		}

		SceneManager.LoadScene(Constants.MenuSceneIndexNumber);
	}

	private void OnRowCompleted(int rowScore)
	{
		currentLevelScore += rowScore;
	}
}
