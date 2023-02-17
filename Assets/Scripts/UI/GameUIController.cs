using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameUIController : MonoBehaviour
{
    [SerializeField]
    private BoardController boardController;
    [SerializeField]
    private TMP_Text moveCountText;
    [SerializeField]
    private TMP_Text scoreText;

	private int score = 0;

	private void Start()
	{
		boardController.OnMoveEvent += OnMove;
		boardController.RowCompleted += OnRowCompleted;

		moveCountText.text = string.Format("{0}: {1}", "Move", GameManager.Instance.CurrentLevelData.MoveCount);
		scoreText.text = string.Format("{0}: {1}", "Score", score.ToString());
	}

	private void OnMove(int moveCount)
	{
		moveCountText.text = string.Format("{0}: {1}", "Move", moveCount.ToString());
	}

	private void OnRowCompleted(int rowScore)
	{
		score += rowScore;
		scoreText.text = string.Format("{0}: {1}", "Score", score.ToString());
	}
}
