using UnityEngine;
using UnityEngine.UIElements;
public class Lab4_1 : MonoBehaviour
{
    private void OnEnable() {
        VisualElement rootve = GetComponent<UIDocument>().rootVisualElement;

        VisualElement top       = rootve.Q("Top");
        VisualElement bottom    = rootve.Q("Bottom");

        VisualTreeAsset template_cajas_1 = Resources.Load<VisualTreeAsset>("Caja");
        VisualTreeAsset template_cajas_2 = Resources.Load<VisualTreeAsset>("Caja2");

        // for (int i = 0; i < 6; i++){
        //     top.Add(template_cajas_1.Instantiate());
        //     bottom.Add(template_cajas_2.Instantiate());
        // }

        Sprite im1 = Resources.Load<Sprite>("im1");
        Sprite im2 = Resources.Load<Sprite>("im2");
        Sprite im3 = Resources.Load<Sprite>("im3");

        VisualElement cajasTop1 = template_cajas_1.Instantiate();
        VisualElement cajasTop2 = template_cajas_1.Instantiate();
        VisualElement cajasBottom = template_cajas_2.Instantiate();
        
        top.Add(cajasTop1);
        top.Add(cajasTop2);
        bottom.Add(cajasBottom);

        VisualElement caja1 = cajasTop1.Q("Caja1");
        VisualElement caja2 = cajasTop2.Q("Caja2");
        VisualElement caja3 = cajasBottom.Q("Caja3");

        // Prueba
        // caja1.style.backgroundColor = new StyleColor(Color.red);


        // Imagenes (no funciona)
        caja1.style.backgroundImage = new StyleBackground(im1);
        caja2.style.backgroundImage = new StyleBackground(im2);
        caja3.style.backgroundImage = new StyleBackground(im3);
    }
}