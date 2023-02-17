using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DreamButton : DreamUI
{
	void Start()
	{
		//Debug.Log("adding button listener");
		//inputService.AddOnMouseClickListener(OnClick);
	}

	private void OnClick(Vector3 clickedPos)
	{
		Debug.Log("on click button");
		if (IsMouseOver)
		{
			Debug.Log("clicked" + gameObject.name);
			onClick?.Invoke();
		}
	}

	public void AddListener(Action onClick)
	{
		Debug.Log("adding listenr");
		this.onClick += onClick;
	}
}
