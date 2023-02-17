using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DreamScrollView : DreamUI
{
	[SerializeField]
	private bool horizontal;
	[SerializeField]
	private bool vertical;
	[SerializeField]
	private DreamScrollContent dreamScrollContent;
	
	bool dragStartedOnMe = false;

	// Start is called before the first frame update
	void Start()
	{
		inputService.AddOnMouseDragListener(OnDrag);
		inputService.AddOnMousePressedListener(OnPress);
		inputService.AddOnMouseReleasedListener(OnRelease);
	}

	private void Update()
	{
		// add slow down
	}

	private void OnDisable()
	{
		inputService.RemoveOnMouseDragListener(OnDrag);
		inputService.RemoveOnMousePressedListener(OnPress);
		inputService.RemoveOnMouseReleasedListener(OnRelease);
	}

	private void OnDrag(Vector3 onDrag)
	{
		if (IsMouseOver)
		{
			if (vertical)
				dreamScrollContent.transform.Translate(Vector3.up * onDrag.y, Space.Self);

			if (horizontal)
				dreamScrollContent.transform.Translate(Vector3.right * onDrag.x, Space.Self);
		}
	}

	private void OnPress(Vector3 pressedPosition)
	{
		if (IsMouseOnMe(pressedPosition))
		{
			dragStartedOnMe = true;
		}
	}

	private void OnRelease(Vector3 releasedPosition)
	{
		dragStartedOnMe = false;
	}
}
