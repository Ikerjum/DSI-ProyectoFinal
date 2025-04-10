using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Lab3_1 : MonoBehaviour
{
    private void OnEnable() {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;

        // Busca los contenedores
        VisualElement izda = root.Q("Izda");
        VisualElement dcha = root.Q("Dcha");

        izda.AddManipulator(new Lab3_Manipulator());
        dcha.AddManipulator(new Lab3_Manipulator());

        izda.AddManipulator(new Lab3_2());
        dcha.AddManipulator(new Lab3_2());

        List<VisualElement> lveizda = new List<VisualElement>(izda.Children());
        List<VisualElement> lvedcha = new List<VisualElement>(dcha.Children());

        lveizda.ForEach(elem => elem.AddManipulator(new Lab3_Manipulator()));
        lvedcha.ForEach(elem => elem.AddManipulator(new Lab3_Manipulator()));
        lveizda.ForEach(elem => elem.AddManipulator(new Lab3_2()));
        lvedcha.ForEach(elem => elem.AddManipulator(new Lab3_2()));

        // En el contenedor de la izquierda, si hay un ClickEvent:
        izda.RegisterCallback<ClickEvent>(
        ev =>
        {
        Debug.Log("Contenedor Izquierda. Fase: " + ev.propagationPhase);
        Debug.Log("Contenedor Izquierda. Target: " + (ev.target as VisualElement).name);
        // Localiza el target del evento como un VisualElement y le cambia el color de fondo
        (ev.target as VisualElement).style.backgroundColor = Color.green;
        // Qué significa esto?
        }, TrickleDown.TrickleDown);

        dcha.RegisterCallback<ClickEvent>(
        ev =>
        {
        Debug. Log("Contenedor Derecha. Fase: " + ev.propagationPhase);
        Debug. Log("Contenedor Derecha. Target: " + (ev.target as VisualElement).name);
        (ev.target as VisualElement).style. backgroundColor = Color.blue;
        }, TrickleDown.TrickleDown);
    }
}