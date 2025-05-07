using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class DurabilityCustomControl : VisualElement
{
    public new class UxmlFactory : UxmlFactory<DurabilityCustomControl, UxmlTraits> { };

    public new class UxmlTraits : VisualElement.UxmlTraits
    {
        UxmlIntAttributeDescription EstadoDurabilidad = new UxmlIntAttributeDescription { name = "EstadoDurabilidad", defaultValue = 0 };

        public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
        {
            base.Init(ve, bag, cc);
            var control = ve as DurabilityCustomControl;
            control.EstadoDurabilidad = EstadoDurabilidad.GetValueFromBag(bag, cc);
            Debug.Log("DURABILIDAD: " + control.EstadoDurabilidad);
        }
    }

    VisualElement[] Escudos = new VisualElement[5];
    int estadoDurabilidad;

    public int EstadoDurabilidad
    {
        get => estadoDurabilidad;
        set
        {
            estadoDurabilidad = value;
            actualizarDurabilidad();
        }
    }

    void actualizarDurabilidad()
    {
        for (int i = 0; i < 5; i++)
        {
            Color color = Escudos[i].style.backgroundColor.value;

            if (i < estadoDurabilidad) color.a = 1f;
            else color.a = 0.2f;

            Escudos[i].style.backgroundColor = color;
        }
    }

    public DurabilityCustomControl()
    {
        styleSheets.Add(Resources.Load<StyleSheet>("FP_DurabilityCustomControlStyleSheet"));

        Sprite Shields = Resources.Load<Sprite>("FP_Shield");
        Sprite Background = Resources.Load<Sprite>("FP_Marco");

        for (int i = 0; i < 5; i++)
        {
            Escudos[i] = new VisualElement();
            Escudos[i].style.width = 35;
            Escudos[i].style.height = 35;
            Escudos[i].style.backgroundColor = new Color(0f, 0f, 1f, 1f); // Azul con opacidad 100%
            Escudos[i].style.backgroundImage = new StyleBackground(Shields);
            Escudos[i].AddToClassList("panel_round");
            
            var contenedor = new VisualElement();
            contenedor.style.width = 35;
            contenedor.style.height = 35;
            contenedor.style.backgroundImage = new StyleBackground(Background);
            
            Escudos[i].Add(contenedor);
            hierarchy.Add(Escudos[i]);
        }
    }
}
