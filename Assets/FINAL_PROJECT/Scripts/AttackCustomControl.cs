using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class AttackCustomControl : VisualElement
{
    public new class UxmlFactory : UxmlFactory<AttackCustomControl, UxmlTraits> { };

    public new class UxmlTraits : VisualElement.UxmlTraits
    {
        UxmlIntAttributeDescription EstadoAtaque = new UxmlIntAttributeDescription { name = "EstadoAtaque", defaultValue = 0 };

        public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
        {
            base.Init(ve, bag, cc);
            var control = ve as AttackCustomControl;
            control.EstadoAtaque = EstadoAtaque.GetValueFromBag(bag, cc);
            Debug.Log("ATAQUE: " + control.EstadoAtaque);
        }
    }
    
    VisualElement[] Espadas = new VisualElement[5];
    int estadoAtaque;

    public int EstadoAtaque
    {
        get => estadoAtaque;
        set
        {
            estadoAtaque = value;
            actualizarAtaque();
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

    public AttackCustomControl()
    {
        styleSheets.Add(Resources.Load<StyleSheet>("FP_AttackCustomControlStyleSheet"));

        Sprite Swords = Resources.Load<Sprite>("FP_Sword");

        for (int i = 0; i < 5; i++)
        {
            Espadas[i] = new VisualElement();
            Espadas[i].style.width = 50;
            Espadas[i].style.height = 50;
            Espadas[i].style.backgroundColor = new Color(1f, 0f, 0f, 1f); // Rojo con opacidad 100%
            Espadas[i].style.backgroundImage = new StyleBackground(Swords);
            Espadas[i].AddToClassList("panel_round");
            hierarchy.Add(Espadas[i]);
        }
    }
}
