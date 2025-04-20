using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Lab4f : VisualElement
{
    public new class UxmlFactory : UxmlFactory<Lab4f, UxmlTraits> { };

    public new class UxmlTraits : VisualElement.UxmlTraits
    {
        UxmlIntAttributeDescription EstadoAtaque = new UxmlIntAttributeDescription { name = "EstadoAtaque", defaultValue = 0 };
        UxmlIntAttributeDescription EstadoDefensa = new UxmlIntAttributeDescription { name = "EstadoDefensa", defaultValue = 0 };

        public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
        {
            base.Init(ve, bag, cc);
            var control = ve as Lab4f;
            control.EstadoAtaque = EstadoAtaque.GetValueFromBag(bag, cc);
            control.EstadoDefensa = EstadoDefensa.GetValueFromBag(bag, cc);
            Debug.Log("ATAQUE: " + control.EstadoAtaque);
            Debug.Log("DEFENSA: " + control.EstadoDefensa);
        }
    }

    VisualElement[] Espadas = new VisualElement[5];
    VisualElement[] Escudos = new VisualElement[5];

    int estadoAtaque;
    int estadoDefensa;

    public int EstadoAtaque
    {
        get => estadoAtaque;
        set
        {
            estadoAtaque = value;
            actualizarAtaque();
        }
    }

    public int EstadoDefensa
    {
        get => estadoDefensa;
        set
        {
            estadoDefensa = value;
            actualizarDefensa();
        }
    }

    void actualizarAtaque()
    {
        for (int i = 0; i < 5; i++)
        {
            Color color = Espadas[i].style.backgroundColor.value;

            if (i < estadoAtaque) color.a = 1f;
            else color.a = 0.2f;

            Espadas[i].style.backgroundColor = color;
        }
    }

    void actualizarDefensa()
    {
        for (int i = 0; i < 5; i++)
        {
            Color color = Escudos[i].style.backgroundColor.value;
            
            if (i < estadoDefensa) color.a = 1f;
            else color.a = 0.2f;
            
            Escudos[i].style.backgroundColor = color;
        }
    }

    public Lab4f()
    {
        styleSheets.Add(Resources.Load<StyleSheet>("Lab4f"));

        Sprite Swords = Resources.Load<Sprite>("swords");
        Sprite Shields = Resources.Load<Sprite>("shields");

        for (int i = 0; i < 5; i++)
        {
            Espadas[i] = new VisualElement();
            Espadas[i].style.width = 60;
            Espadas[i].style.height = 60;
            Espadas[i].style.backgroundColor = new Color(1f, 0f, 0f, 1f); // Rojo con opacidad 100%
            Espadas[i].style.backgroundImage = new StyleBackground(Swords);
            Espadas[i].AddToClassList("panel_round");
            hierarchy.Add(Espadas[i]);
        }

        for (int i = 0; i < 5; i++)
        {
            Escudos[i] = new VisualElement();
            Escudos[i].style.width = 60;
            Escudos[i].style.height = 60;
            Escudos[i].style.backgroundColor = new Color(0f, 0f, 1f, 1f); // Azul con opacidad 100%
            Escudos[i].style.backgroundImage = new StyleBackground(Shields);
            Escudos[i].AddToClassList("panel_round");
            hierarchy.Add(Escudos[i]);
        }
    }
}
