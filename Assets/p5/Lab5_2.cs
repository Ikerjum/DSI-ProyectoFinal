using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Lab5_2_namespace
{
    public class Lab5_2 : MonoBehaviour
    {
        //VisualElement plantilla;

        List<Individuo> individuos;
        Individuo selecIndividuo;

        VisualElement tarjeta1;
        VisualElement tarjeta2;
        VisualElement tarjeta3;
        VisualElement tarjeta4;

        TextField input_nombre;
        TextField input_apellido;

        //Individuo individuoPrueba;
        private void OnEnable()
        {
            VisualElement root = GetComponent<UIDocument>().rootVisualElement;

            //plantilla = root.Q("plantilla");
            //input_nombre = root.Q<TextField>("InputNombre");
            //input_apellido = root.Q<TextField>("InputApellido");

            //individuoPrueba = new Individuo("Perico", "Palotes");

            //Tarjeta tarjetaPrueba = new Tarjeta(plantilla, individuoPrueba);

            tarjeta1 = root.Q("Tarjeta1");
            tarjeta2 = root.Q("Tarjeta2");
            tarjeta3 = root.Q("Tarjeta3");
            tarjeta4 = root.Q("Tarjeta4");

            input_nombre = root.Q<TextField>("InputNombre");
            input_apellido = root.Q<TextField>("InputApellido");

            individuos = BaseDatos.getData();

            VisualElement panelDcha = root.Q("Dcha");
            panelDcha.RegisterCallback<ClickEvent>(seleccionTarjeta);

            //plantilla.RegisterCallback<ClickEvent>(SeleccionIndividuo);
            input_nombre.RegisterCallback<ChangeEvent<string>>(CambioNombre);
            input_apellido.RegisterCallback<ChangeEvent<string>>(CambioApellido);

            //input_nombre.SetValueWithoutNotify(individuoPrueba.Nombre);
            //input_apellido.SetValueWithoutNotify(individuoPrueba.Apellido);

            InitializeUI();
        }

        //void SeleccionIndividuo(ClickEvent evt)
        //{
        //    string nombre = plantilla.Q<Label>("Nombre").text;
        //    string apellido = plantilla.Q<Label>("Apellido").text;

        //    Debug.Log(nombre);

        //    input_nombre.SetValueWithoutNotify(nombre);
        //    input_apellido.SetValueWithoutNotify(apellido);
        //}

        void CambioNombre(ChangeEvent<string> evt)
        {
            if (selecIndividuo != null)
            {
                selecIndividuo.Nombre = evt.newValue;
            }
            else
            {
                Debug.LogWarning("selecIndividuo is null. Please select an individual first.");
            }
        }

        void CambioApellido(ChangeEvent<string> evt)
        {
            //Label apellidoLabel = plantilla.Q<Label>("Apellido");
            //apellidoLabel.text = evt.newValue;

            //individuoPrueba.Apellido = evt.newValue;


            
            if (selecIndividuo != null)
            {
                selecIndividuo.Apellido = evt.newValue;
            }
            else
            {
                Debug.LogWarning("selecIndividuo is null. Please select an individual first.");
            }
        }

        void seleccionTarjeta(ClickEvent e)
        {
            VisualElement tarjeta = e.target as VisualElement;
            selecIndividuo = tarjeta.userData as Individuo;
            // selecIndividuo = tarjeta.getIndividuo();

            input_nombre.SetValueWithoutNotify(selecIndividuo.Nombre);
            input_apellido.SetValueWithoutNotify(selecIndividuo.Apellido);

            Debug.Log($"Selected: {selecIndividuo.Nombre} {selecIndividuo.Apellido}");
        }

        // void seleccionTarjeta(ClickEvent e)
        // {
        //     VisualElement tarjeta = e.target as VisualElement;
        //     if (tarjeta != null) {
        //         // Ensure userData is assigned to the VisualElement
        //         if (tarjeta.userData == null) {
        //             Debug.LogWarning("userData is null. Assigning userData to the VisualElement.");
        //             int index = int.Parse(tarjeta.name.Replace("Tarjeta", "")) - 1; // Assuming names are "Tarjeta1", "Tarjeta2", etc.
        //             tarjeta.userData = individuos[index];
        //         }

        //         if (tarjeta.userData is Individuo individuo) {
        //             selecIndividuo = individuo;

        //             input_nombre.SetValueWithoutNotify(selecIndividuo.Nombre);
        //             input_apellido.SetValueWithoutNotify(selecIndividuo.Apellido);
        //         }
        //         else {
        //             Debug.LogWarning("userData is not of type Individuo.");
        //         }
        //     }
        //     else {
        //         Debug.LogWarning("Tarjeta is null. Ensure the target is a VisualElement.");
        //     }
        // }

        void InitializeUI()
        {
            Tarjeta tar1 = new Tarjeta(tarjeta1, individuos[0]);
            Tarjeta tar2 = new Tarjeta(tarjeta2, individuos[1]);
            Tarjeta tar3 = new Tarjeta(tarjeta3, individuos[2]);
            Tarjeta tar4 = new Tarjeta(tarjeta4, individuos[3]);

        }
    }
}
