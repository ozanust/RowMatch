using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stone : DreamUI
{
	[SerializeField]
	private SpriteRenderer iconRenderer;
	private StoneType stoneType;
	private int gridId = -1;

	[SerializeField]
	private Sprite red;
	[SerializeField]
	private Sprite green;
	[SerializeField]
	private Sprite blue;
	[SerializeField]
	private Sprite yellow;
	[SerializeField]
	private Sprite check;

	public StoneType Type => stoneType;
	public int GridId => gridId;
	public GameObject Icon => iconRenderer.gameObject;

	public Action<int> MousePressed;
	public Action<int> MouseDragged;

	private void Start()
	{
		inputService.AddOnMousePressedListener(OnMousePress);
		inputService.AddOnMouseDragListener(OnMouseDraged);
	}

	private void OnMousePress(Vector3 pressedPosition)
	{
		if (IsMouseOver)
		{
			MousePressed?.Invoke(gridId);
		}
	}

	private void OnMouseDraged(Vector3 pressedPosition)
	{
		if (IsMouseOver)
		{
			MouseDragged?.Invoke(gridId);
		}
	}

	public void SetStone(string stoneType, int gridId)
	{
		switch (stoneType)
		{
			case "r":
				this.stoneType = StoneType.Red;
				iconRenderer.sprite = red;
				break;
			case "g":
				this.stoneType = StoneType.Green;
				iconRenderer.sprite = green;
				break;
			case "b":
				this.stoneType = StoneType.Blue;
				iconRenderer.sprite = blue;
				break;
			case "y":
				this.stoneType = StoneType.Yellow;
				iconRenderer.sprite = yellow;
				break;
		}

		this.gridId = gridId;
	}

	public void SetStoneType(StoneType stoneType)
	{
		this.stoneType = stoneType;

		switch (stoneType)
		{
			case StoneType.Red:
				iconRenderer.sprite = red;
				break;
			case StoneType.Green:
				iconRenderer.sprite = green;
				break;
			case StoneType.Blue:
				iconRenderer.sprite = blue;
				break;
			case StoneType.Yellow:
				iconRenderer.sprite = yellow;
				break;
		}

		iconRenderer.transform.localPosition = new Vector3(0, 0, 0);
	}

	public void SetAsCompleted()
	{
		GetComponent<BoxCollider2D>().enabled = false;
		iconRenderer.sprite = check;
		stoneType = StoneType.Completed;
	}
}
