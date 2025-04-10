using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Lab5_2_namespace
{
    public class BaseDatos
    {
        public static List<Individuo> getData()
        {
            List<Individuo> datos = new List<Individuo>();

            Individuo perico = new Individuo("Perico", "Palotes");
            Individuo tornasol = new Individuo("Tornasol", "Tornasolado");
            Individuo luca = new Individuo("Luca", "Lucatell");
            Individuo ivan = new Individuo("Ivan", "Ivanovich");

            datos.Add(perico);
            datos.Add(tornasol);
            datos.Add(luca);
            datos.Add(ivan);

            return datos;
        }
    }
}
