using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class Lab2_3 : MonoBehaviour
{

    private void OnEnable(){
        UIDocument uidoc = GetComponent<UIDocument>();
        VisualElement rootve = uidoc.rootVisualElement;
        UQueryBuilder<VisualElement> builder = new(rootve);

        // Busca las diferentes partes del slider y les cambia el color
        VisualElement item1 = rootve.Q<VisualElement>("item1");
        item1.style.backgroundColor = Color.white;
        Label texto = rootve.Q<Label>("textoitem");
        texto.text = @"<line-indent=15%>En un lugar de <smallcaps>La Mancha</smallcaps> </line-indent><br>
        de cuyo nombre <rotate=""45"">no quiero acordarme</rotate>,
        <b><gradient=""color_gradient"">no hacia mucho que vivia un hidalgo</gradient></b>
        de los de lanza en astillero,
        <b><color=""black""><gradient=""color_gradient"">adarga antigua</gradient></b>,
        <i>rocin flaco y galgo corredor.";
    }
}