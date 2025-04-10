using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Lab6_namespace
{
    [Serializable]
    public class IndividuoP6
    {
        public event Action Cambio;

        [SerializeField] private string nombre;
        public string Nombre
        {
            get { return nombre; }
            set
            {
                if (value != nombre)
                {
                    nombre = value;
                    Cambio?.Invoke();
                }
            }
        }
        [SerializeField] private string apellido;
        public string Apellido
        {
            get { return apellido; }
            set
            {
                if (value != apellido)
                {
                    apellido = value;
                    Cambio?.Invoke();
                }
            }
        }

        [SerializeField] private Sprite bgImage;
        public Sprite BgImage
        {
            get { return bgImage; }
            set
            {
                if (value != bgImage)
                {
                    bgImage = value;
                    Cambio?.Invoke();
                }
            }
        }


        public IndividuoP6(string nombre, string apellido, Sprite im)
        {
            this.nombre = nombre;
            this.apellido = apellido;
            this.bgImage = im;
        }
    }
}