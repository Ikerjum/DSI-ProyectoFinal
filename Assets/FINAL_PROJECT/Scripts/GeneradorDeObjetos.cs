using UnityEngine.UIElements;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace Project
{
    public class GeneradorDeObjetos : MonoBehaviour
    {
        VisualElement contenedorObjetos;
        Button botonCrear;
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

            // JSON (comentado para más adelante)
            //guardar.RegisterCallback<ClickEvent>(GuardarJson);
            //cargar.RegisterCallback<ClickEvent>(CargarJson);
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

                // Movimiento del objeto
                tarjeta.RegisterCallback<PointerDownEvent>(evt => IniciarArrastre(tarjeta, evt));
                tarjeta.RegisterCallback<PointerMoveEvent>(evt => ActualizarArrastre(evt));
                tarjeta.RegisterCallback<PointerUpEvent>(evt => FinalizarArrastre(evt));


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

        void IniciarArrastre(VisualElement tarjeta, PointerDownEvent evt)
        {
            tarjetaArrastrada = tarjeta;
            tarjeta.style.position = Position.Absolute;
            tarjeta.BringToFront();

            foreach (var contenedor in listaContenedores)
            {
                Debug.Log($"Contenedor: {contenedor.name}");
                foreach (var child in contenedor.Children())
                {
                    Debug.Log($"  Hijo: {child.name}");
                }
            }
        }

        void ActualizarArrastre(PointerMoveEvent evt)
        {
            if (tarjetaArrastrada != null)
            {
                tarjetaArrastrada.style.left = evt.position.x - tarjetaArrastrada.layout.width / 2;
                tarjetaArrastrada.style.top = evt.position.y - tarjetaArrastrada.layout.height / 2;
            }
        }

        void FinalizarArrastre(PointerUpEvent evt)
        {
            if (tarjetaArrastrada == null) return;

            VisualElement destino = listaContenedores.FirstOrDefault(c => 
                c.worldBound.Overlaps(tarjetaArrastrada.worldBound) && 
                (c.childCount == 0 || !c.Children().Contains(tarjetaArrastrada))
            );

            if (destino != null)
            {
                tarjetaArrastrada.style.position = Position.Relative;
                tarjetaArrastrada.style.left = tarjetaArrastrada.style.top = StyleKeyword.Null;
                destino.Add(tarjetaArrastrada);
            }
            else
            {
                // Vuelve al contenedor original si no se suelta en uno válido
                tarjetaArrastrada.style.position = Position.Relative;
                tarjetaArrastrada.style.left = tarjetaArrastrada.style.top = StyleKeyword.Null;
            }

            tarjetaArrastrada = null;
        }
    }
}