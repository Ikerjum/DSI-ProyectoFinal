using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Lab6_namespace
{
    [Serializable]
    public class BaseDatosP6
    {
        public static List<IndividuoP6> getData()
        {
            
            List<IndividuoP6> datos = new List<IndividuoP6>();

            Sprite imagen = Resources.Load<Sprite>("rojo");
            IndividuoP6 perico = new IndividuoP6("Perico", "Palotes", imagen);
            IndividuoP6 tornasol = new IndividuoP6("Tornasol", "Tornasolado", imagen);
            IndividuoP6 luca = new IndividuoP6("Luca", "Lucatell", imagen);
            IndividuoP6 ivan = new IndividuoP6("Ivan", "Ivanovich", imagen);

            datos.Add(perico);
            datos.Add(tornasol);
            datos.Add(luca);
            datos.Add(ivan);

            return datos;
        }
    }
}