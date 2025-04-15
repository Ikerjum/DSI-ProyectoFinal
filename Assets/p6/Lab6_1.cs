using UnityEngine.UIElements;
using UnityEngine;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace Lab6_namespace
{
    public class Lab6_1 : MonoBehaviour
    {
        VisualElement botonCrear;
        Toggle toggleModificar;
        VisualElement contenedor_dcha;
        TextField input_nombre;
        TextField input_apellido;
        VisualElement Color1;
        VisualElement Color2;
        VisualElement Color3;
        
        List<IndividuoP6> individuos;
        IndividuoP6 individuoSelec;
        VisualElement guardar;
        VisualElement cargar;

        List<IndividuoP6> list_individuos = new List<IndividuoP6>();
        private void OnEnable()
        {
            VisualElement root = GetComponent<UIDocument>().rootVisualElement;

            individuos = BaseDatosP6.getData();

            contenedor_dcha = root.Q<VisualElement>("Dcha");
            input_nombre = root.Q<TextField>("InputNombre");
            input_apellido = root.Q<TextField>("InputApellido");
            botonCrear = root.Q<Button>("BotonCrear");
            toggleModificar = root.Q<Toggle>("ToggleModificar");

            Color1 = root.Q<VisualElement>("Color1");
            Color2 = root.Q<VisualElement>("Color2");
            Color3 = root.Q<VisualElement>("Color3");

            guardar = root.Q<Button>("Guardar");
            cargar = root.Q<Button>("Cargar");
            
            contenedor_dcha.RegisterCallback<ClickEvent>(seleccionTarjeta);

            botonCrear.RegisterCallback<ClickEvent>(NuevaTarjeta);
            input_nombre.RegisterCallback<ChangeEvent<string>>(CambioNombre);
            input_apellido.RegisterCallback<ChangeEvent<string>>(CambioApellido);
            guardar.RegisterCallback<ClickEvent>(GuardarJson);
            cargar.RegisterCallback<ClickEvent>(CargarJson);

            Color1.RegisterCallback<ClickEvent>(CambiarColorFondoTarjeta);
            Color2.RegisterCallback<ClickEvent>(CambiarColorFondoTarjeta);
            Color3.RegisterCallback<ClickEvent>(CambiarColorFondoTarjeta);
        }

        void GuardarJson(ClickEvent evt)
        {
            string json = JsonHelperIndividuo.ToJson(list_individuos, true);
            string path = Path.Combine(Application.dataPath, "Resources", "individuos.json");
            File.WriteAllText(path, json);
            Debug.Log("Guardado en: " + path);
        }
        

        void CargarJson(ClickEvent evt)
        {
            Debug.Log("CargarJson");
            for (int i = 0; i < individuos.Count; i++)
            {
                VisualTreeAsset plantilla = Resources.Load<VisualTreeAsset>("Tarjeta");
                VisualElement tarjetaPlantilla = plantilla.Instantiate();

                contenedor_dcha.Add(tarjetaPlantilla);

                tarjetas_borde_negro();
                tarjeta_borde_blanco(tarjetaPlantilla);

                IndividuoP6 individuo = individuos[i];
                TarjetaP6 tarjeta = new TarjetaP6(tarjetaPlantilla, individuo);
                list_individuos.Add(individuo);
            }
        }

        void NuevaTarjeta(ClickEvent evt)
        {
            if (!toggleModificar.value)
            {
                VisualTreeAsset plantilla = Resources.Load<VisualTreeAsset>("Tarjeta");
                VisualElement tarjetaPlantilla = plantilla.Instantiate();

                contenedor_dcha.Add(tarjetaPlantilla);
                
                tarjetas_borde_negro();
                tarjeta_borde_blanco(tarjetaPlantilla);

                IndividuoP6 individuo = new IndividuoP6(input_nombre.value, input_apellido.value, Resources.Load<Sprite>("rojo"));
                TarjetaP6 tarjeta = new TarjetaP6(tarjetaPlantilla, individuo);
                individuoSelec = individuo;

                list_individuos.Add(individuo);
                // list_individuos.ForEach(elem => {
                //     Debug.Log(elem.Nombre + " " + elem.Apellido);
                //     // !! no pasa a json
                //     string jsonIndividuo = JsonUtility.ToJson(elem);
                //     Debug.Log(jsonIndividuo);
                // });

                string listaToJson = JsonHelperIndividuo.ToJson(list_individuos, true);
                Debug.Log(listaToJson);

                List<IndividuoP6> jsonToLista = JsonHelperIndividuo.FromJson<IndividuoP6>(listaToJson);
                jsonToLista.ForEach(elem => {
                    Debug.Log(elem.Nombre + " " + elem.Apellido);
                });
            }
        }

        void seleccionTarjeta(ClickEvent e)
        {
            if (e.target is VisualElement tarjeta){
                VisualElement miTarjeta = e.target as VisualElement;
                individuoSelec = miTarjeta.userData as IndividuoP6;
                
                // !! miTarjeta.userData es nulo (si se crea de antemano da error)
                // if (individuoSelec == null){
                //     individuoSelec = miTarjeta.Q<miTarjeta.userData>() as IndividuoP6;
                // }
                
                if (individuoSelec != null){
                    input_nombre.SetValueWithoutNotify(individuoSelec.Nombre);
                    input_apellido.SetValueWithoutNotify(individuoSelec.Apellido);
                    toggleModificar.value = true;
                    
                    tarjetas_borde_negro();
                    tarjeta_borde_blanco(miTarjeta);
                }
            }
        }

        void CambioNombre(ChangeEvent<string> evt)
        {
            if (toggleModificar.value)
            {
                individuoSelec.Nombre = evt.newValue;
            }
        }

        void CambioApellido(ChangeEvent<string> evt)
        {
            if (toggleModificar.value)
            {
                individuoSelec.Apellido = evt.newValue;
            }
        }

        void CambiarColorFondoTarjeta(ClickEvent e)
        {
            if (toggleModificar.value){

                if (e.target == Color1)
                {
                    individuoSelec.BgImage = Resources.Load<Sprite>("negro");
                }
                else if (e.target == Color2)
                {
                    individuoSelec.BgImage = Resources.Load<Sprite>("rojo");
                }
                else if (e.target == Color3)
                {
                    individuoSelec.BgImage = Resources.Load<Sprite>("verde");
                }
            }
        }

        void tarjetas_borde_negro(){
            List<VisualElement> lista_tarjetas = contenedor_dcha.Children().ToList();
            lista_tarjetas.ForEach(elem =>
            {
                VisualElement tarjeta = elem.Q("Tarjeta");

                tarjeta.style.borderBottomColor = Color.black;
                tarjeta.style.borderRightColor = Color.black;
                tarjeta.style.borderTopColor = Color.black;
                tarjeta.style.borderLeftColor = Color.black;

            });
        }

        void tarjeta_borde_blanco(VisualElement tar){
            VisualElement tarjeta = tar.Q("Tarjeta");

            tarjeta.style.borderBottomColor = Color.white;
            tarjeta.style.borderRightColor = Color.white;
            tarjeta.style.borderTopColor = Color.white;
            tarjeta.style.borderLeftColor = Color.white;
        }
    }
}
