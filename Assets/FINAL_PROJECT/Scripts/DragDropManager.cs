using UnityEngine;
using UnityEngine.UIElements;

namespace Project
{
    public class DragDropManager : PointerManipulator
    {
        private VisualElement draggedElement;
        private VisualElement originalParent;
        private bool isDragging = false;

        public DragDropManager()
        {
            activators.Add(new ManipulatorActivationFilter { button = MouseButton.LeftMouse });
        }

        protected override void RegisterCallbacksOnTarget()
        {
            target.RegisterCallback<MouseDownEvent>(OnMouseDown);
            target.RegisterCallback<MouseMoveEvent>(OnMouseMove);
            target.RegisterCallback<MouseUpEvent>(OnMouseUp);
        }

        protected override void UnregisterCallbacksFromTarget()
        {
            target.UnregisterCallback<MouseDownEvent>(OnMouseDown);
            target.UnregisterCallback<MouseMoveEvent>(OnMouseMove);
            target.UnregisterCallback<MouseUpEvent>(OnMouseUp);
        }
        private void OnMouseDown(MouseDownEvent evt)
        {
            if (isDragging){
                evt.StopImmediatePropagation();
                return;
            }
            if (!CanStartManipulation(evt)/* && draggedElement != null*/) return;

            draggedElement = FindTarjetaElement(evt.target as VisualElement);
            if (draggedElement == null) return;

            originalParent = draggedElement.parent;
            isDragging = true;

            draggedElement.BringToFront();
            evt.StopPropagation();
        }

        private void OnMouseMove(MouseMoveEvent evt)
        {
            if (!isDragging || draggedElement == null || !CanStartManipulation(evt)) return;

            Vector2 mousePosition = evt.mousePosition;
            draggedElement.style.position = Position.Absolute;
            draggedElement.style.left = mousePosition.x - draggedElement.resolvedStyle.width / 2;
            draggedElement.style.top = mousePosition.y - draggedElement.resolvedStyle.height / 2;

            evt.StopPropagation();
        }

        private void OnMouseUp(MouseUpEvent evt)
        {
            if (!isDragging || draggedElement == null || !CanStopManipulation(evt)) return;

            VisualElement contenedor = FindContenedor(evt.target as VisualElement);

            if (contenedor != null && contenedor != originalParent)
            {
                contenedor.Add(draggedElement);
                draggedElement.style.position = Position.Relative;
                draggedElement.style.left = 0;
                draggedElement.style.top = 0;
            }
            else if (originalParent != null)
            {
                originalParent.Add(draggedElement);
                draggedElement.style.position = Position.Relative;
                draggedElement.style.left = 0;
                draggedElement.style.top = 0;
            }

            draggedElement.BringToFront();

            draggedElement = null;
            isDragging = false;


            target.ReleaseMouse();
            evt.StopPropagation();
        }

        private VisualElement FindTarjetaElement(VisualElement elem)
        {
            while (elem != null && elem.name != "Object" && elem != draggedElement)
                elem = elem.parent;
            return elem;
        }

        private VisualElement FindContenedor(VisualElement elem)
        {
            while (elem != null && elem.name != "contenedorObjetos")
                elem = elem.parent;
            return elem;
        }
    }
}