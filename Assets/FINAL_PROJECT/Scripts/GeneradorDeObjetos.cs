using UnityEngine.UIElements;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace Project
{
    public class GeneradorDeObjetos : MonoBehaviour
    {
        VisualElement contenedorObjetos;
        Button botonCrear;
        Button botonGuardar;
        Button botonCargar;
        Button botonLimpiar;
        Button botonEliminar;
        Toggle toggleModificar;
        TextField inputNombre;
        IntegerField inputFuerza;
        IntegerField inputDefensa;
        DropdownField dropdownTipo;
        VisualElement tarjetaSeleccionada;
        Objeto objetoSeleccionado;
        List<Objeto> objetos = new List<Objeto>();
        List<VisualElement> listaContenedores = new List<VisualElement>();

        VisualElement tarjetaArrastrada;

        private void OnEnable()
        {
            VisualElement root = GetComponent<UIDocument>().rootVisualElement;

            // Referencias UI existentes
            contenedorObjetos = root.Q<VisualElement>("InventoryContainerList");
            botonCrear = root.Q<Button>("BotonCrear");
            toggleModificar = root.Q<Toggle>("ToggleModificar");

            inputNombre = root.Q<TextField>("InputNombre");
            inputFuerza = root.Q<IntegerField>("InputFuerza");
            inputDefensa = root.Q<IntegerField>("InputDefensa");
            dropdownTipo = root.Q<DropdownField>("DropdownTipo");

            // Opciones enum para el dropdown
            dropdownTipo.choices = System.Enum.GetNames(typeof(TipoObjeto)).ToList();

            // Anadir manipuladores a los contenedores
            // contenedorObjetos.AddManipulator(new DragDropManager());
            
            List<VisualElement> lista_objetos = new List<VisualElement>(contenedorObjetos.Children());
            lista_objetos.ForEach(elem => elem.AddManipulator(new DragDropManager()));

            // Callbacks
            botonCrear.RegisterCallback<ClickEvent>(CrearObjeto);
            //contenedorObjetos.RegisterCallback<ClickEvent>(SeleccionarTarjeta);
            inputNombre.RegisterCallback<ChangeEvent<string>>(CambioNombre);
            inputFuerza.RegisterCallback<ChangeEvent<int>>(CambioFuerza);
            inputDefensa.RegisterCallback<ChangeEvent<int>>(CambioDefensa);
            dropdownTipo.RegisterCallback<ChangeEvent<string>>(CambioTipo);

            // Referencia al contenedor de objetos del inventario
            VisualElement listaInventario = root.Q<VisualElement>("InventoryContainerList");

            // Obtener todos los Containers (VisualElements con instancia del template dentro)
            listaContenedores = listaInventario.Query<VisualElement>(className: "inventory-container").ToList();

            // JSON
            botonGuardar = root.Q<Button>("BotonGuardar");
            botonGuardar.RegisterCallback<ClickEvent>(GuardarJson);
            botonCargar = root.Q<Button>("BotonCargar");
            botonCargar.RegisterCallback<ClickEvent>(CargarJson);

            // Limpiar y eliminar objetos
            botonLimpiar = root.Q<Button>("BotonLimpiar");
            botonLimpiar.RegisterCallback<ClickEvent>(LimpiarInventarioClick);
            botonEliminar = root.Q<Button>("BotonEliminar");
            botonEliminar.RegisterCallback<ClickEvent>(EliminarObjeto);
            
            // Ocultar botón eliminar por defecto
            botonEliminar.style.display = DisplayStyle.None;
            
            // Añadir callback para mostrar/ocultar el botón eliminar según el toggle
            toggleModificar.RegisterValueChangedCallback(evt => {
                botonEliminar.style.display = evt.newValue ? DisplayStyle.Flex : DisplayStyle.None;
            });
            
            // Intenta cargar los objetos guardados al iniciar
            CargarObjetosGuardados();
        }


        void CrearObjeto(ClickEvent evt)
        {
            if (!toggleModificar.value)
            {
                // Encontrar un contenedor libre
                VisualElement contenedorLibre = listaContenedores.FirstOrDefault(c => c.childCount == 0 || c.Q("Object") == null);

                if (contenedorLibre == null)
                {
                    Debug.LogWarning("No hay contenedores libres disponibles.");
                    return;
                }

                VisualTreeAsset plantilla = Resources.Load<VisualTreeAsset>("FP_Object");
                VisualElement tarjeta = plantilla.Instantiate();
                tarjeta.RegisterCallback<ClickEvent>(SeleccionarTarjeta);


                contenedorLibre.Add(tarjeta);
                tarjetas_borde_negro();
                tarjeta_borde_blanco(tarjeta);


                if (inputFuerza.value >= 5) inputFuerza.value = 5;
                else if (inputFuerza.value <= 0) inputFuerza.value = 0;
                if (inputDefensa.value >= 5) inputDefensa.value = 5; 
                else if (inputDefensa.value <= 0) inputDefensa.value = 0;

                Objeto nuevo = new Objeto(
                    string.IsNullOrEmpty(inputNombre.value) ? "Sin nombre" : inputNombre.value,
                    (TipoObjeto)System.Enum.Parse(typeof(TipoObjeto), string.IsNullOrEmpty(dropdownTipo.value) ? TipoObjeto.Arma.ToString() : dropdownTipo.value),
                    inputFuerza.value,
                    inputDefensa.value
                );

                tarjeta.userData = nuevo;
                objetoSeleccionado = nuevo;
                tarjetaSeleccionada = tarjeta;

                objetos.Add(nuevo);

                // Mostrar info en la tarjeta
                //tarjeta.Q<Label>("LabelNombre").text = nuevo.Nombre;
                //tarjeta.Q<Label>("LabelFuerza").text = $"Fuerza: {nuevo.Fuerza}";
                //tarjeta.Q<Label>("LabelDefensa").text = $"Defensa: {nuevo.Defensa}";
                //tarjeta.Q<Label>("LabelTipo").text = nuevo.TipoObjeto.ToString();

                //VARIABLES DE LA TARJETA
                var labelN = tarjetaSeleccionada?.Q<Label>("LabelNombre");
                if (labelN != null) {labelN.text = objetoSeleccionado.Nombre;}

                var labelF = tarjetaSeleccionada?.Q<Label>("LabelFuerza");
                if (labelF != null) {labelF.text = $"Fuerza: {nuevo.Fuerza}";}

                var labelD = tarjetaSeleccionada?.Q<Label>("LabelDefensa");
                if (labelD != null) {labelD.text = $"Defensa: {nuevo.Defensa}";}

                Label tipoL = tarjeta.Q<Label>("LabelTipo");
                if (tipoL != null) {tipoL.text = nuevo.TipoObjeto.ToString();}

                VisualElement imagen = tarjeta.Q<VisualElement>("ImagenTipo");
                if (imagen != null)
                {
                    string ruta = ObtenerRutaImagen(nuevo.TipoObjeto);
                    imagen.style.backgroundImage = new StyleBackground(Resources.Load<Texture2D>(ruta));
                }

                //CUSTOM CONTROLS
                AttackCustomControl ataqueControl = tarjeta.Q<AttackCustomControl>();
                if (ataqueControl != null)
                {
                    ataqueControl.EstadoAtaque = nuevo.Fuerza;
                }

                DurabilityCustomControl defensaControl = tarjeta.Q<DurabilityCustomControl>();
                if (defensaControl != null)
                {
                    defensaControl.EstadoDurabilidad = nuevo.Defensa;
                }
            }
        }

        public bool CrearObjetoPar(string nombre, TipoObjeto tipoObjeto, int fuerza, int defensa)
        {
            try
            {
                // Encontrar un contenedor libre
                VisualElement contenedorLibre = listaContenedores.FirstOrDefault(c => c.childCount == 0 || c.Q("Object") == null);

                if (contenedorLibre == null)
                {
                    Debug.LogWarning("No hay contenedores libres disponibles.");
                    return false;
                }

                VisualTreeAsset plantilla = Resources.Load<VisualTreeAsset>("FP_Object");
                if (plantilla == null)
                {
                    Debug.LogError("No se pudo cargar la plantilla FP_Object");
                    return false;
                }

                VisualElement tarjeta = plantilla.Instantiate();
                tarjeta.RegisterCallback<ClickEvent>(SeleccionarTarjeta);

                contenedorLibre.Add(tarjeta);
                tarjetas_borde_negro();
                tarjeta_borde_blanco(tarjeta);

                // Validar valores
                fuerza = Mathf.Clamp(fuerza, 0, 5);
                defensa = Mathf.Clamp(defensa, 0, 5);

                Objeto nuevo = new Objeto(
                    string.IsNullOrEmpty(nombre) ? "Sin nombre" : nombre,
                    tipoObjeto,
                    fuerza,
                    defensa
                );

                tarjeta.userData = nuevo;
                objetos.Add(nuevo);

                // Mostrar info en la tarjeta
                ActualizarTarjeta(tarjeta, nuevo);

                return true;
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"Error al crear objeto: {ex.Message}");
                return false;
            }
        }

        // Método auxiliar para actualizar la tarjeta con los datos del objeto
        private void ActualizarTarjeta(VisualElement tarjeta, Objeto obj)
        {
            var labelN = tarjeta.Q<Label>("LabelNombre");
            if (labelN != null) { labelN.text = obj.Nombre; }

            var labelF = tarjeta.Q<Label>("LabelFuerza");
            if (labelF != null) { labelF.text = $"Fuerza: {obj.Fuerza}"; }

            var labelD = tarjeta.Q<Label>("LabelDefensa");
            if (labelD != null) { labelD.text = $"Defensa: {obj.Defensa}"; }

            Label tipoL = tarjeta.Q<Label>("LabelTipo");
            if (tipoL != null) { tipoL.text = obj.TipoObjeto.ToString(); }

            VisualElement imagen = tarjeta.Q<VisualElement>("ImagenTipo");
            if (imagen != null)
            {
                string ruta = ObtenerRutaImagen(obj.TipoObjeto);
                imagen.style.backgroundImage = new StyleBackground(Resources.Load<Texture2D>(ruta));
            }

            // CUSTOM CONTROLS
            AttackCustomControl ataqueControl = tarjeta.Q<AttackCustomControl>();
            if (ataqueControl != null)
            {
                ataqueControl.EstadoAtaque = obj.Fuerza;
            }

            DurabilityCustomControl defensaControl = tarjeta.Q<DurabilityCustomControl>();
            if (defensaControl != null)
            {
                defensaControl.EstadoDurabilidad = obj.Defensa;
            }
        }

        // Serialización/Deserialización JSON
        void GuardarJson(ClickEvent evt)
        {            
            string json = JsonHelper.ToJson(objetos, true);
            string path = Path.Combine(Application.dataPath, "Resources", "objetosJSON.json");
            File.WriteAllText(path, json);
            Debug.Log("Guardado en: " + path);
        }

        void CargarJson(ClickEvent evt)
        {
            // Limpiar objetos existentes antes de cargar
            LimpiarInventario();
            CargarObjetosGuardados();
        }

        void CargarObjetosGuardados()
        {
            string jsonPath = "objetosJSON";
            TextAsset jsonFile = Resources.Load<TextAsset>(jsonPath);
            string jsonString = jsonFile.text;
            
            List<Objeto> objetosCargados = JsonHelper.FromJson<Objeto>(jsonString);
            
            var copiaObjetos = new List<Objeto>(objetosCargados);
            
            foreach (var objeto in copiaObjetos)
            {
                CrearObjetoPar(objeto.Nombre, objeto.TipoObjeto, objeto.Fuerza, objeto.Defensa);
            }
        }

        void LimpiarInventarioClick(ClickEvent evt)
        {
            LimpiarInventario();
        }

        void LimpiarInventario()
        {
            // Elimina todos los objetos actuales
            foreach (var contenedor in listaContenedores)
            {
                contenedor.Clear();
            }
            objetos.Clear();
            objetoSeleccionado = null;
            tarjetaSeleccionada = null;
        }

        void SeleccionarTarjeta(ClickEvent evt)
        {
            VisualElement current = evt.target as VisualElement;

            while (current != null && current.userData as Objeto == null)
            {
                current = current.parent;
            }

            if (current != null)
            {
                Objeto obj = current.userData as Objeto;
                if (obj != null)
                {
                    objetoSeleccionado = obj;
                    tarjetaSeleccionada = current;

                    inputNombre.SetValueWithoutNotify(obj.Nombre);
                    inputFuerza.SetValueWithoutNotify(obj.Fuerza);
                    inputDefensa.SetValueWithoutNotify(obj.Defensa);
                    dropdownTipo.SetValueWithoutNotify(obj.TipoObjeto.ToString());

                    toggleModificar.value = true;

                    tarjetas_borde_negro();
                    tarjeta_borde_blanco(current);
                }
            }
        }

        void CambioNombre(ChangeEvent<string> evt)
        {
            if (toggleModificar.value && objetoSeleccionado != null)
            {
                objetoSeleccionado.Nombre = string.IsNullOrEmpty(evt.newValue) ? "Sin nombre" : evt.newValue;
                var label = tarjetaSeleccionada?.Q<Label>("LabelNombre");
                if (label != null) label.text = objetoSeleccionado.Nombre;
            }
        }

        void CambioFuerza(ChangeEvent<int> evt)
        {
            if (toggleModificar.value && objetoSeleccionado != null)
            {
                if (evt.newValue >= 5) objetoSeleccionado.Fuerza = 5;
                else if (evt.newValue <= 0) objetoSeleccionado.Fuerza = 0;
                else objetoSeleccionado.Fuerza = evt.newValue;
                
                var label = tarjetaSeleccionada?.Q<Label>("LabelFuerza");
                if (label != null) label.text = $"Fuerza: {objetoSeleccionado.Fuerza}";


                AttackCustomControl ataqueControl = tarjetaSeleccionada?.Q<AttackCustomControl>();
                if (ataqueControl != null)
                {
                    ataqueControl.EstadoAtaque = objetoSeleccionado.Fuerza;
                }
            }
        }

        void CambioDefensa(ChangeEvent<int> evt)
        {
            if (toggleModificar.value && objetoSeleccionado != null)
            {
                
                if (evt.newValue >= 5) objetoSeleccionado.Defensa = 5;
                else if (evt.newValue <= 0) objetoSeleccionado.Defensa = 0;
                else objetoSeleccionado.Defensa = evt.newValue;

                var label = tarjetaSeleccionada?.Q<Label>("LabelDefensa");
                if (label != null) label.text = $"Defensa: {objetoSeleccionado.Defensa}";


                DurabilityCustomControl defensaControl = tarjetaSeleccionada?.Q<DurabilityCustomControl>();
                if (defensaControl != null)
                {
                    defensaControl.EstadoDurabilidad = objetoSeleccionado.Defensa;
                }
            }
        }

        void CambioTipo(ChangeEvent<string> evt)
        {
            if (toggleModificar.value && objetoSeleccionado != null)
            {
                objetoSeleccionado.TipoObjeto = (TipoObjeto)System.Enum.Parse(
                    typeof(TipoObjeto), 
                    string.IsNullOrEmpty(evt.newValue) ? dropdownTipo.choices[0] : evt.newValue
                );
                var label = tarjetaSeleccionada?.Q<Label>("LabelTipo");
                if (label != null) label.text = objetoSeleccionado.TipoObjeto.ToString();

                VisualElement imagen = tarjetaSeleccionada?.Q<VisualElement>("ImagenTipo");
                if (imagen != null)
                {
                    string ruta = ObtenerRutaImagen(objetoSeleccionado.TipoObjeto);
                    imagen.style.backgroundImage = new StyleBackground(Resources.Load<Texture2D>(ruta));
                }
            }
        }

        void tarjetas_borde_negro()
        {
            foreach (var tarjeta in contenedorObjetos.Children())
            {
                VisualElement panel = tarjeta.Q("FP_Object");
                if (panel != null)
                {
                    panel.style.borderBottomColor = Color.black;
                    panel.style.borderTopColor = Color.black;
                    panel.style.borderLeftColor = Color.black;
                    panel.style.borderRightColor = Color.black;
                }
            }
            
        }

        void tarjeta_borde_blanco(VisualElement tarjeta)
        {
            VisualElement panel = tarjeta.Q("FP_Object");
            if (panel != null)
            {
                panel.style.borderBottomColor = Color.white;
                panel.style.borderTopColor = Color.white;
                panel.style.borderLeftColor = Color.white;
                panel.style.borderRightColor = Color.white;
            }
        }

        string ObtenerRutaImagen(TipoObjeto tipo)
        {
            switch (tipo)
            {
                case TipoObjeto.Arma: return "FP_iconos/FP_arma";
                case TipoObjeto.Armadura: return "FP_iconos/FP_armadura";
                case TipoObjeto.Consumible: return "FP_iconos/FP_consumible";
                case TipoObjeto.Herramienta: return "FP_iconos/FP_herramienta";
                case TipoObjeto.Material: return "FP_iconos/FP_material";
                case TipoObjeto.Accesorio: return "FP_iconos/FP_accesorio";
                case TipoObjeto.Pocion: return "FP_iconos/FP_pocion";
                case TipoObjeto.Libro: return "FP_iconos/FP_libro";
                case TipoObjeto.Llave: return "FP_iconos/FP_llave";
                case TipoObjeto.Miscelaneo: return "FP_iconos/FP_miscelaneo";
                default: return "FP_iconos/FP_default";
            }
        }

        void EliminarObjeto(ClickEvent evt)
        {
            if (toggleModificar.value && objetoSeleccionado != null && tarjetaSeleccionada != null)
            {
                VisualElement contenedor = tarjetaSeleccionada.parent;
                
                // Eliminar el objeto de la lista
                objetos.Remove(objetoSeleccionado);
                
                // Limpiar el contenedor visual
                if (contenedor != null)
                {
                    contenedor.Clear();
                }
                
                // Restablecer referencias
                objetoSeleccionado = null;
                tarjetaSeleccionada = null;
                toggleModificar.value = false;
                
                // Limpiar campos de entrada
                inputNombre.SetValueWithoutNotify("");
                inputFuerza.SetValueWithoutNotify(0);
                inputDefensa.SetValueWithoutNotify(0);
                dropdownTipo.SetValueWithoutNotify(TipoObjeto.Arma.ToString());
            }
        }
    }
}