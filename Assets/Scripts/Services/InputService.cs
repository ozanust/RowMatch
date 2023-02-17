using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;

public class InputService : SingletonManager<InputService>, IInputService
{
	Action<Vector3> OnLeftMouseButtonPressed;
	Action<Vector3> OnLeftMouseButtonReleased;
	Action OnLeftMouseButtonHeld;
	Action<Vector3> OnLeftMouseButtonDrag;
	Action<Vector3> OnLeftMouseButtonClicked;

	Vector3 mousePosition = new Vector3(0, 0, 0);
	Vector3 worldMousePosition = new Vector3(0, 0, 0);

	bool isLeftMouseButtonDown = false;
	float leftMouseButtonHeldTimer = 0;
	const float leftMouseButtonClickTimer = 0.2f;

	DreamButton interactingUIObject = null;

	// Update is called once per frame
	void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			isLeftMouseButtonDown = true;
			mousePosition = Input.mousePosition;
			worldMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			RaycastHit2D hit = Physics2D.Raycast(worldMousePosition, Vector3.forward, 2);
			if (hit)
				hit.collider.gameObject.TryGetComponent(out interactingUIObject);
			OnLeftMouseButtonPressed?.Invoke(worldMousePosition);
		}

		if (Input.GetMouseButtonUp(0))
		{
			isLeftMouseButtonDown = false;
			worldMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			OnLeftMouseButtonReleased?.Invoke(worldMousePosition);

			if (leftMouseButtonHeldTimer > Mathf.Epsilon && leftMouseButtonHeldTimer < leftMouseButtonClickTimer)
			{
				OnLeftMouseButtonClicked?.Invoke(worldMousePosition);

				if (interactingUIObject != null)
				{
					interactingUIObject.onClick?.Invoke();
					interactingUIObject = null;
				}
			}

			leftMouseButtonHeldTimer = 0;
		}

		if (Input.GetMouseButton(0))
		{
			leftMouseButtonHeldTimer += Time.deltaTime;
			OnLeftMouseButtonHeld?.Invoke();
		}

		if (isLeftMouseButtonDown && Mathf.Abs((Input.mousePosition - mousePosition).magnitude) > Mathf.Epsilon)
		{
			OnLeftMouseButtonDrag?.Invoke(Camera.main.ScreenToWorldPoint(Input.mousePosition) - Camera.main.ScreenToWorldPoint(mousePosition));
			mousePosition = Input.mousePosition;
		}
	}

	public void AddOnMousePressedListener(Action<Vector3> onPress)
	{
		OnLeftMouseButtonPressed += onPress;
	}

	public void AddOnMouseClickListener(Action<Vector3> onClick)
	{
		OnLeftMouseButtonClicked += onClick;
	}

	public void AddOnMouseDragListener(Action<Vector3> onDrag)
	{
		OnLeftMouseButtonDrag += onDrag;
	}

	public void AddOnMouseReleasedListener(Action<Vector3> onReleased)
	{
		OnLeftMouseButtonReleased += onReleased;
	}

	public void AddOnMouseHeldListener(Action onHeld)
	{
		OnLeftMouseButtonHeld += onHeld;
	}

	public void RemoveOnMousePressedListener(Action<Vector3> onPress)
	{
		OnLeftMouseButtonPressed -= onPress;
	}

	public void RemoveOnMouseClickListener(Action<Vector3> onClick)
	{
		OnLeftMouseButtonClicked -= onClick;
	}

	public void RemoveOnMouseDragListener(Action<Vector3> onDrag)
	{
		OnLeftMouseButtonDrag -= onDrag;
	}

	public void RemoveOnMouseReleasedListener(Action<Vector3> onReleased)
	{
		OnLeftMouseButtonReleased -= onReleased;
	}

	public void RemoveOnMouseHeldListener(Action onHeld)
	{
		OnLeftMouseButtonHeld -= onHeld;
	}
}
