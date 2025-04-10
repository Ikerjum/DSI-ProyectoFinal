using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Lab6_namespace
{
    [Serializable]
    public class TarjetaP6
    {
        IndividuoP6 miIndividuo;
        VisualElement tarjetaRoot;

        Label nombreLabel;
        Label apellidoLabel;
        VisualElement image;

        public TarjetaP6(VisualElement tarjetaRoot, IndividuoP6 individuo)
        {
            this.tarjetaRoot = tarjetaRoot;
            this.miIndividuo = individuo;

            nombreLabel = tarjetaRoot.Q<Label>("Nombre");
            apellidoLabel = tarjetaRoot.Q<Label>("Apellido");
            image = tarjetaRoot.Q<VisualElement>("top");
            
            tarjetaRoot.userData = miIndividuo;

            UpdateUI();

            miIndividuo.Cambio += UpdateUI;
        }

        void UpdateUI()
        {
            nombreLabel.text = miIndividuo.Nombre;
            apellidoLabel.text = miIndividuo.Apellido;
            
            image.style.backgroundImage = new StyleBackground(miIndividuo.BgImage);
        }
    }
}