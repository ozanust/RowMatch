using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class DreamScrollContent : DreamUI
{
	List<Transform> childrenTransforms;
	public Action OnNewElementAdded;
}
