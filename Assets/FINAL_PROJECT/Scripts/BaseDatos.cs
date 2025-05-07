using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Project
{
    [Serializable]
    public class BaseDatos
    {
        public static List<Objeto> getData()
        {
            
            List<Objeto> datos = new List<Objeto>();

            string jsonPath = "objetosJSON";
            TextAsset jsonFile = Resources.Load<TextAsset>(jsonPath);

            if (jsonFile != null)
            {
                string jsonString = jsonFile.text;
                datos = JsonHelper.FromJson<Objeto>(jsonString);
            }
            else
            {
                Debug.LogError("No se encontró el archivo JSON en Resources.");
            }

            return datos;
        }
    }
}