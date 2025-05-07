using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project
{
    [SerializeField]
    public static class JsonHelper
    {
        [SerializeField]
        public static List<Objeto> FromJson<Objeto>(string json)
        {
            ListaObjetos<Objeto> listaObjetos = JsonUtility.FromJson<ListaObjetos<Objeto>>(json);
            return listaObjetos.Objetos;
        }

        [SerializeField]
        public static string ToJson<Objeto>(List<Objeto> lista)
        {
            ListaObjetos<Objeto> listaObjetos = new ListaObjetos<Objeto>();
            listaObjetos.Objetos = lista;
            return JsonUtility.ToJson(listaObjetos);
        }

        [SerializeField]
        public static string ToJson<Objeto>(List<Objeto> lista, bool prettyPrint)
        {
            ListaObjetos<Objeto> listaObjetos = new ListaObjetos<Objeto>();
            listaObjetos.Objetos = lista;
            return JsonUtility.ToJson(listaObjetos, prettyPrint);
        }

        [Serializable]
        private class ListaObjetos<Objeto>
        {
            public List<Objeto> Objetos;
        }
    }
}