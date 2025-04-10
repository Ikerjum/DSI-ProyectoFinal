using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class Lab2 : MonoBehaviour
{
private void OnEnable() {
    UIDocument uidoc = GetComponent<UIDocument>();
    VisualElement rootve = uidoc.rootVisualElement;
    
// Builder hace una queery de todo root
    UQueryBuilder<VisualElement> builder = new(rootve);

// Esto mete en una lista todos elementos
    // List<VisualElement> lista_ve = builder.ToList();

// Esto selecciona todos los hijos del contenedor middle, los anade a lista_ve y anade todos los elementos de lista_ve a .amarillo
    // VisualElement contenedor = builder.Name("middle");
    // List<VisualElement> lista_ve = contenedor.Children().ToList();


// Query() es consulta de todo
// Query(par).First es lo mismo que Q()
// Parámetros de Query: className: " ", .First(), .Last(), Type (Button, VisualElement, ...)
    // List<VisualElement> lista_ve = rootve.Query().ToList();
    List<VisualElement> lista_ve = rootve.Query(className: "azul").ToList();

// Busca en lista el hijo de cada elemento que sea un type (boton)
    List<VisualElement> lista_buttons = lista_ve.SelectMany(elem => elem.Children()).Where(elem => elem.GetType() == typeof(Button)).ToList();

    lista_ve.ForEach(elem => {
        // Debug.Log(elem.name);
        elem.style.backgroundColor = Color.red;
    });
    
    lista_buttons.ForEach(elem => { elem.AddToClassList("amarillo"); });
}
}