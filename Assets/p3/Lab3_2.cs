using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Lab3_2 : MouseManipulator
{
    Vector3 orScale;
    public Lab3_2()
    {
        activators.Add(new ManipulatorActivationFilter { button = MouseButton.RightMouse });
    }
    protected override void RegisterCallbacksOnTarget()
    {
        target.RegisterCallback<MouseDownEvent>(OnMouseDown);
        target.RegisterCallback<WheelEvent>(OnMouseWheel);
        target.RegisterCallback<MouseUpEvent>(OnMouseUp);
    }

    protected override void UnregisterCallbacksFromTarget()
    {
        target.UnregisterCallback<MouseDownEvent>(OnMouseDown);
        target.UnregisterCallback<WheelEvent>(OnMouseWheel);
        target.UnregisterCallback<MouseUpEvent>(OnMouseUp);
    }

    private void OnMouseDown(MouseDownEvent mev)
    {
        if (CanStartManipulation(mev)){
            orScale = target.transform.scale;
            Debug.Log("OnMouseDown, orScale: " + target.transform.scale);
            mev.StopPropagation();
        }
    }

    private void OnMouseUp(MouseUpEvent mev)
    {
        if (CanStartManipulation(mev)){
            target.transform.scale = orScale;
            Debug.Log("OnMouseUp, scale: " + target.transform.scale);
            mev.StopPropagation();
        }
    }

    private void OnMouseWheel(WheelEvent mev)
    {
        var scale = target.transform.scale;
        scale.x += mev.delta.y * 0.1f;
        scale.y += mev.delta.y * 0.1f;
        target.transform.scale = scale;
        Debug.Log("OnMouseWheel, scale: " + target.transform.scale);
        target.style.borderLeftColor = Color.yellow;
        target.style.borderRightColor = Color.yellow;
        target.style.borderTopColor = Color.yellow;
        target.style.borderBottomColor = Color.yellow;
        mev.StopPropagation();
    }
}