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

            string jsonPath = "individuos";
            TextAsset jsonFile = Resources.Load<TextAsset>(jsonPath);

            if (jsonFile != null)
            {
                string jsonString = jsonFile.text;
                datos = JsonHelperIndividuo.FromJson<IndividuoP6>(jsonString);
            }
            else
            {
                Debug.LogError("No se encontró el archivo JSON en Resources.");
            }

            return datos;
        }
    }
}