using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardController : MonoBehaviour
{
	[SerializeField]
	private Stone prototypeStone;
	[SerializeField]
	private StoneScoreSO stoneScoreData;
	[SerializeField]
	private Transform board;

	bool isUserActionStarted;
	int initialGridStoneId = -1;
	int targetGridStoneId = -1;
	int completedRowCount = 0;
	int moveCount = 0;
	int score = 0;
	HashSet<int> unMovableRowSet = null;

	Stone[] gridStones;
	LevelData levelData;

	/// <summary>
	/// Move event with remaining move count.
	/// </summary>
	public Action<int> OnMoveEvent;
	/// <summary>
	/// Called if move right left or no possible matchese.
	/// </summary>
	public Action NoMoveLeft;
	/// <summary>
	/// Called if all the rows completed.
	/// </summary>
	public Action BoardCompleted;
	/// <summary>
	/// Called on row completion with the score that row.
	/// </summary>
	public Action<int> RowCompleted;

	public void CreateBoard(LevelData data)
	{
		gridStones = new Stone[data.Width * data.Height];
		levelData = data;
		moveCount = data.MoveCount;

		for (int i = 0; i < data.Height; i++)
		{
			for (int j = 0; j < data.Width; j++)
			{
				Stone tempStone = Instantiate(prototypeStone, new Vector3(prototypeStone.gameObject.transform.lossyScale.x * j, prototypeStone.gameObject.transform.lossyScale.y * i), Quaternion.identity, board);
				tempStone.SetStone(data.Grid[j + i * data.Width], j + i * data.Width);
				tempStone.MousePressed += OnMousePressedStone;
				tempStone.MouseDragged += OnMouseDraggedStone;
				gridStones[j + i * data.Width] = tempStone;
			}
		}

		float boardWidth = data.Width * prototypeStone.gameObject.transform.lossyScale.x;
		float boardHeight = data.Height * prototypeStone.gameObject.transform.lossyScale.y;

		if (Camera.main.orthographicSize < boardWidth)
		{
			float ratio = Camera.main.orthographicSize / boardWidth;
			board.transform.localScale = new Vector3(board.transform.lossyScale.x * ratio, board.transform.lossyScale.y * ratio, board.transform.lossyScale.z * ratio);
		}

		if (Camera.main.orthographicSize * 2 < boardHeight)
		{
			float ratio = (Camera.main.orthographicSize * 2) / boardHeight;
			board.transform.localScale = new Vector3(board.transform.lossyScale.x * ratio, board.transform.lossyScale.y * ratio, board.transform.lossyScale.z * ratio);
			board.transform.position = new Vector3(0, -1.5f, 0);
		}
	}

	private void OnMousePressedStone(int gridId)
	{
		if (moveCount > 0)
		{
			isUserActionStarted = true;
			initialGridStoneId = gridId;
		}
	}

	private void OnMouseDraggedStone(int gridId)
	{
		if (isUserActionStarted && initialGridStoneId != gridId)
		{
			targetGridStoneId = gridId;
			isUserActionStarted = false;
			SwapIcons(gridId, gridStones[initialGridStoneId].Icon, gridStones[targetGridStoneId].Icon);
		}
	}

	private void SwapIcons(int gridId, GameObject icon1, GameObject icon2)
	{
		Vector3 icon1InitialPos = icon1.transform.position;
		Vector3 icon2InitialPos = icon2.transform.position;
		LTDescr tween1 = LeanTween.move(icon1, icon2InitialPos, 0.3f).setEaseOutExpo();
		LTDescr tween2 = LeanTween.move(icon2, icon1InitialPos, 0.3f).setEaseOutExpo();

		int completedTweenCount = 0;

		tween1.setOnComplete(() => {
			completedTweenCount++;
			OnIconSwapAnimationComplete(completedTweenCount);
		});

		tween2.setOnComplete(() => {
			completedTweenCount++;
			OnIconSwapAnimationComplete(completedTweenCount);
		});
	}

	private void OnIconSwapAnimationComplete(int completedTweenCount)
	{
		if (completedTweenCount == 2)
		{
			StoneType initialStoneType = gridStones[initialGridStoneId].Type;
			StoneType targetStoneType = gridStones[targetGridStoneId].Type;

			gridStones[targetGridStoneId].SetStoneType(initialStoneType);
			gridStones[initialGridStoneId].SetStoneType(targetStoneType);

			OnMove(new int[] { initialGridStoneId, targetGridStoneId });
		}
	}

	private void OnMove(int[] movedStoneIds)
	{
		moveCount--;
		OnMoveEvent?.Invoke(moveCount);

		int rowIndexOfFirstStone = movedStoneIds[0] / levelData.Width;
		int rowIndexOfSecondStone = movedStoneIds[1] / levelData.Width;

		CheckRow(rowIndexOfFirstStone);
		CheckRow(rowIndexOfSecondStone);

		if (IsWin())
		{
			BoardCompleted?.Invoke();
			Debug.LogWarning("Board completed");
			return;
		}

		if (!IsMoveAvailable())
		{
			NoMoveLeft?.Invoke();
			Debug.LogWarning("No move left");
		}
	}

	private void CheckRow(int rowIndex)
	{
		int sameColorStoneCount = 0;
		StoneType type = StoneType.None;

		for (int i = 0; i < levelData.Width; i++)
		{
			if (gridStones[i + rowIndex * levelData.Width].Type == type)
				sameColorStoneCount++;

			type = gridStones[i + rowIndex * levelData.Width].Type;
		}

		if (sameColorStoneCount == levelData.Width - 1)
		{
			SetRowCompleted(rowIndex);
		}
	}

	private void SetRowCompleted(int rowIndex)
	{
		int rowScore = stoneScoreData.GetScoreOfType(gridStones[rowIndex * levelData.Width].Type) * levelData.Width;
		for (int i = 0; i < levelData.Width; i++)
		{
			gridStones[i + rowIndex * levelData.Width].SetAsCompleted();
		}

		SetNewUnMovableRow(rowIndex * levelData.Width);
		score += rowScore;
		RowCompleted?.Invoke(rowScore);

		completedRowCount++;
	}

	private bool IsWin()
	{
		return completedRowCount == levelData.Height;
	}

	private bool IsMoveAvailable()
	{
		if (moveCount == 0)
			return false;

		if (gridStones[0].Type != StoneType.Completed && gridStones[levelData.Width].Type == StoneType.Completed)
		{
			SetNewUnMovableRow(0);
		}

		if (gridStones[levelData.Width * (levelData.Height - 1)].Type != StoneType.Completed && gridStones[levelData.Width * (levelData.Height - 2)].Type == StoneType.Completed)
		{
			SetNewUnMovableRow(levelData.Width * (levelData.Height - 1));
		}

		for (int i = 1; i < levelData.Height - 1; i++)
		{
			if (gridStones[i * levelData.Width].Type != StoneType.Completed && gridStones[(i - 1) * levelData.Width].Type == StoneType.Completed && gridStones[(i + 1) * levelData.Width].Type == StoneType.Completed)
			{
				SetNewUnMovableRow(i * levelData.Width);
			}
		}

		if (unMovableRowSet != null)
		{
			if (unMovableRowSet.Count != levelData.Height)
			{
				Dictionary<StoneType, int> colorCounts = new Dictionary<StoneType, int>();
				for (int i = 0; i < levelData.Height; i++)
				{
					if (!unMovableRowSet.Contains(i * levelData.Width))
					{
						for (int j = 0; j < levelData.Width; j++)
						{
							if (!colorCounts.ContainsKey(gridStones[j + i * levelData.Width].Type))
							{
								colorCounts.Add(gridStones[j + i * levelData.Width].Type, 1);
							}
							else
							{
								colorCounts[gridStones[j + i * levelData.Width].Type] += 1;

								if (colorCounts[gridStones[j + i * levelData.Width].Type] == levelData.Width)
								{
									return true;
								}
							}
						}
					}
					else if (colorCounts.Count > 0)
					{
						colorCounts.Clear();
					}
				}

				return false;
			}
			else
			{
				return false;
			}
		}
		else
		{
			return true;
		}
	}

	void SetNewUnMovableRow(int rowIndex)
	{
		if (unMovableRowSet == null)
			unMovableRowSet = new HashSet<int>();

		if (!unMovableRowSet.Contains(rowIndex))
			unMovableRowSet.Add(rowIndex);
	}
}
