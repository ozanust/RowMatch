using System;
using UnityEngine;

public interface IInputService
{
    void AddOnMousePressedListener(Action<Vector3> onPress);
    void AddOnMouseClickListener(Action<Vector3> onClick);
    void AddOnMouseDragListener(Action<Vector3> onClick);
    void AddOnMouseReleasedListener(Action<Vector3> onClick);
    void AddOnMouseHeldListener(Action onClick);
}
