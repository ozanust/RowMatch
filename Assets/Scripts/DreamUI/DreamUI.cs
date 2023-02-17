using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class DreamUI : MonoBehaviour, IDreamUI
{
	protected InputService inputService;
	public Action onClick;

	private bool isMouseOver = false;
	private Vector4 bounds;
	protected bool IsMouseOver { get { return isMouseOver; } }

	/// <summary>
	/// Order of the bounds; left, right, down, up.
	/// </summary>
	protected Vector4 Bounds { get { return GetBounds(); } }

	private void Awake()
	{
		inputService = InputService.Instance;
	}

	/// <summary>
	/// Returns true if mouse is currently on the UI object.
	/// </summary>
	/// <param name="mousePositionInWorld">World coordinates for the mouse position.</param>
	/// <returns></returns>
	public bool IsMouseOnMe(Vector3 mousePositionInWorld)
	{
		Vector2 objectStartPoint = new Vector2(transform.position.x - transform.lossyScale.x / 2f, transform.position.y - transform.lossyScale.y / 2f);
		Vector2 objectEndPoint = new Vector2(transform.position.x + transform.lossyScale.x / 2f, transform.position.y + transform.lossyScale.y / 2f);

		if (mousePositionInWorld.x > objectStartPoint.x &&
			mousePositionInWorld.x < objectEndPoint.x &&
			mousePositionInWorld.y > objectStartPoint.y &&
			mousePositionInWorld.y < objectEndPoint.y)
		{
			return true;
		}

		return false;
	}

	private Vector4 GetBounds()
	{
		Vector2 scrollStartPoint = new Vector2(transform.position.x - transform.lossyScale.x / 2f, transform.position.y - transform.lossyScale.y / 2f);
		Vector2 scrollEndPoint = new Vector2(transform.position.x + transform.lossyScale.x / 2f, transform.position.y + transform.lossyScale.y / 2f);

		return new Vector4(scrollStartPoint.x, scrollEndPoint.x, scrollStartPoint.y, scrollEndPoint.y);
	}

	private void OnMouseEnter()
	{
		isMouseOver = true;
	}

	private void OnMouseExit()
	{
		isMouseOver = false;
	}
}
