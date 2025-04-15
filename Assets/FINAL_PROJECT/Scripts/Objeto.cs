using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Project
{
    public enum TipoObjeto
    {
        Arma,
        Armadura,
        Consumible,
        Herramienta,
        Material,
        Accesorio,
        Pocion,
        Libro,
        Llave,
        Miscelaneo
    }

    [Serializable]
    public class Objeto
    {

        [SerializeField] private string nombre;
        public string Nombre
        {
            get { return nombre; }
            set
            {
                if (value != nombre)
                {
                    nombre = value;
                }
            }
        }

        [SerializeField] private TipoObjeto tipoObjeto;
        public TipoObjeto TipoObjeto
        {
            get { return tipoObjeto; }
            set
            {
                if (value != tipoObjeto)
                {
                    tipoObjeto = value;
                }
            }
        }


        [SerializeField] private int fuerza;
        public int Fuerza
        {
            get { return fuerza; }
            set
            {
                if (value != fuerza)
                {
                    fuerza = value;
                }
            }
        }

        [SerializeField] private int defensa;
        public int Defensa
        {
            get { return defensa; }
            set
            {
                if (value != defensa)
                {
                    defensa = value;
                }
            }
        }


        public Objeto(string nombre, TipoObjeto tipoObjeto, int fuerza, int defensa)
        {
            this.nombre = nombre;
            this.tipoObjeto = tipoObjeto;
            this.fuerza = fuerza;
            this.defensa = defensa;
        }
    }
}