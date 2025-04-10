using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class Lab2_2 : MonoBehaviour
{
    VisualElement contenido_azul;
    VisualElement contenido_verde;
    VisualElement contenido_ambar;
    VisualElement pestanya_azul;
    VisualElement pestanya_verde;
    VisualElement pestanya_ambar;

    private void NoContenido(){
        contenido_azul.style.display = DisplayStyle.None;
        contenido_verde.style.display = DisplayStyle.None;
        contenido_ambar.style.display = DisplayStyle.None;
    }

    private void OnEnable(){
        UIDocument uidoc = GetComponent<UIDocument>();
        VisualElement rootve = uidoc.rootVisualElement;
        UQueryBuilder<VisualElement> builder = new(rootve);

        VisualElement contenido = rootve.Q("Contenido");
        VisualElement pestanyas = rootve.Q("Pestanyas");

        pestanya_azul = pestanyas.Q("Azul");
        pestanya_verde = pestanyas.Q("Verde");
        pestanya_ambar = pestanyas.Q("Ambar");

        contenido_azul = contenido.Q("Azul");
        contenido_verde = contenido.Q("Verde");
        contenido_ambar = contenido.Q("Ambar");


        pestanya_azul.RegisterCallback<MouseDownEvent>(evt => {
            Debug.Log("Pestaña azul");
            NoContenido();
            contenido_azul.style.display = DisplayStyle.Flex;
        });

        pestanya_verde.RegisterCallback<MouseDownEvent>(evt => {
            Debug.Log("Pestaña verde");
            NoContenido();
            contenido_verde.style.display = DisplayStyle.Flex;
        });

        pestanya_ambar.RegisterCallback<MouseDownEvent>(evt => {
            Debug.Log("Pestaña ambar");
            NoContenido();
            contenido_ambar.style.display = DisplayStyle.Flex;
        });


// Busca las diferentes partes del slider y les cambia el color
        VisualElement mslider = rootve.Q<Slider>("mana");
        Debug.Log(mslider.name);
        mslider.style.backgroundColor = Color.magenta;

        VisualElement mdragger = rootve.Q<VisualElement>("unity-dragger");
        Debug.Log(mdragger.name);
        mdragger.style.backgroundColor = Color.yellow;

        VisualElement mtracker = rootve.Q<VisualElement>("unity-tracker");
        Debug.Log(mtracker.name);
        mtracker.style.backgroundColor = Color.green;
    }
}