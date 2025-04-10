using UnityEngine;
using UnityEngine.UIElements;

public class Lab3_Manipulator : MouseManipulator
{
    public Lab3_Manipulator()
    {
        activators.Add(new ManipulatorActivationFilter { button = MouseButton.RightMouse });
    }
    protected override void RegisterCallbacksOnTarget()
    {
        target.RegisterCallback<MouseDownEvent>(OnMouseDown);
        target.RegisterCallback<MouseEnterEvent>(MouseEnterEvent);
        target.RegisterCallback<MouseLeaveEvent>(MouseLeaveEvent);
    }

    protected override void UnregisterCallbacksFromTarget()
    {
        target.UnregisterCallback<MouseDownEvent>(OnMouseDown);
        target.UnregisterCallback<MouseEnterEvent>(MouseEnterEvent);
        target.UnregisterCallback<MouseLeaveEvent>(MouseLeaveEvent);
    }

    private void OnMouseDown(MouseDownEvent mev)
    {
        if (CanStartManipulation(mev)){
            target.style.borderBottomColor = Color.white;
            target.style.borderLeftColor    = Color.white;
            target.style.borderRightColor   = Color.white;
            target.style.borderTopColor     = Color.white;
            mev.StopPropagation();
        }
    }

    // HOVER ENTER
    protected void MouseEnterEvent(MouseEnterEvent e) {
            target.style.borderBottomColor =    Color.red;
            target.style.borderLeftColor =      Color.red;
            target.style.borderRightColor =     Color.red;
            target.style.borderTopColor =       Color.red;
            e.StopPropagation();
    }
    // HOVER EXIT
    protected void MouseLeaveEvent(MouseLeaveEvent e) {
        target.style.borderBottomColor =    Color.black;
        target.style.borderLeftColor =      Color.black;
        target.style.borderRightColor =     Color.black;
        target.style.borderTopColor =       Color.black;
        e.StopPropagation();
    }
}