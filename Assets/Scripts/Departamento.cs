using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.VectorGraphics;


public class Departamento : MonoBehaviour
{
    public string Nombre;
    public SpriteRenderer spriteRenderer;
    public List<Departamento> Adyacentes;

    public List<Edificios> Edificios = new List<Edificios>();
    public Ejercito Ejer;

    public Jugador Dueno;

    public AudioClip clickSound;
    private AudioSource audioSource;

    private static Departamento departamentoSeleccionado = null;

    // Edificios
    public bool TieneOficinaReclutamiento = false;
    public bool TieneFabrica = false;
    public bool TieneEstablo = false;
    public bool TieneFortaleza = false;

    public MenuDepartamentoUI menuUI;  // Arrastrar desde el inspector

    public List<Tropa> ProduccionEnCurso = new List<Tropa>();
    public List<Edificios> ConstruccionesEnCurso = new List<Edificios>();

    public enum TipoRecurso { Carbon, Comida, Dinero, Hierro, Madera }

    public TipoRecurso RecursoProducido;  // Asignar en Inspector o en código

    public SpriteRenderer imagenTropa; // Imagen de tropas en el mapa

    private Color originalColor;
    private Color seleccionadoColor = new Color(0.643f, 0.631f, 0.565f);
    private Color jugador1Color = new Color(0.720f, 0.827f, 0.702f); // Color #B8D3B3 (RGB Normalizado)
    private Color jugador2Color = new Color(0.720f, 0.325f, 0.376f); // Color #B85360 (RGB Normalizado)

    void Start()
    {
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        if (menuUI == null)
        {
            menuUI = FindObjectOfType<MenuDepartamentoUI>();
        }

        originalColor = spriteRenderer.color;

        Ejer = new Ejercito(Dueno); // Inicializar la lista de tropas
    }

    void OnMouseDown()
    {
        if (departamentoSeleccionado != null && departamentoSeleccionado != this)
        {
            departamentoSeleccionado.DesmarcarDepartamento();
        }

        if (departamentoSeleccionado != this)
        {
            SeleccionarDepartamento();

            if (audioSource != null && clickSound != null)
            {
                audioSource.PlayOneShot(clickSound);
            }
        }
        else
        {
            DesmarcarDepartamento();
        }
    }

    private void SeleccionarDepartamento()
    {
        spriteRenderer.color = seleccionadoColor;
        departamentoSeleccionado = this;
        menuUI.MostrarDepartamento(this); // MOSTRAR EL MENÚ
    }

    private void DesmarcarDepartamento()
    {
        spriteRenderer.color = originalColor;
        departamentoSeleccionado = null;
        if (menuUI != null)
        {
            menuUI.OcultarMenu(); // Esta línea oculta el menú cuando se deselecciona
        }
    }

    void SpriteJugador1()
    {
        spriteRenderer.color = jugador1Color;
    }

    void SpriteJugador2()
    {
        spriteRenderer.color = jugador2Color;
    }

    void Update()
    {
        if (Dueno.getID() == 1)
        {
            SpriteJugador1();
        }
        else if (Dueno.getID() == 2)
        {
            SpriteJugador2();
        }

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit.collider == null && departamentoSeleccionado != null)
            {
                departamentoSeleccionado.DesmarcarDepartamento();
                departamentoSeleccionado = null;
            }
        }
    }

    public void MostrarDatos()
    {
        Debug.Log("Departamento: " + Nombre);
        Debug.Log("Dueno: " + (Dueno != null ? Dueno.Nombre : "Sin dueno"));
        Debug.Log("Tropas: " + Ejer.Tropas.Count);
        Debug.Log("Infanteria: " + Ejer.Tropas.FindAll(t => t is Infanteria).Count);
        Debug.Log("Artilleria: " + Ejer.Tropas.FindAll(t => t is Artilleria).Count);
        Debug.Log("Caballeria: " + Ejer.Tropas.FindAll(t => t is Caballeria).Count);
        Debug.Log("Edificios: " +
            (TieneOficinaReclutamiento ? "Infanteria " : "") +
            (TieneFabrica ? "Fabrica " : "") +
            (TieneEstablo ? "Establo " : ""));
    }

    public void ConstruirOficina(string tipo)
    {
        // Aqu� podr�as verificar si el jugador tiene recursos
        switch (tipo.ToLower())
        {
            case "infanteria":
                TieneOficinaReclutamiento = true;
                break;
            case "artilleria":
                TieneFabrica = true;
                break;
            case "caballeria":
                TieneEstablo = true;
                break;
        }

        Debug.Log($"Oficina de {tipo} construida en {Nombre}");
    }

    public void ReclutarTropa(string tipo, int cantidad = 1)
    {
        for (int i = 0; i < cantidad; i++)
        {
            switch (tipo.ToLower())
            {
                case "infanteria":
                    if (TieneOficinaReclutamiento)
                        Ejer.AnadirTropa(new Infanteria(Dueno));
                    break;
                case "artilleria":
                    if (TieneFabrica)
                        Ejer.AnadirTropa(new Artilleria(Dueno));
                    break;
                case "caballeria":
                    if (TieneEstablo)
                        Ejer.AnadirTropa(new Caballeria(Dueno));
                    break;
            }
        }
        ActualizarImagenTropa();

        //Recuenta tropas por tipo (esto lo defines en Ejercito)
        Ejer.ObtenerConteoTropas();

        MenuDepartamentoUI ui = FindObjectOfType<MenuDepartamentoUI>();
        
        if (ui != null && ui.gameObject.activeSelf)
        {
            ui.MostrarDepartamento(this); // Refresca conteos
        }
            
    }


    public void Conquistar(Jugador nuevoDueno)
    {
        if (Dueno != null)
        {
            Dueno.RemoverDepartamento(this);

            AsignarPropietario(nuevoDueno);
            Ejer.Tropas.Clear();
            Debug.Log($"{Nombre} fue conquistado por {nuevoDueno.Nombre}");
        }
    }


    public void AgregarAdyacente(Departamento adyacente)
    {
        if (!Adyacentes.Contains(adyacente))
        {
            Adyacentes.Add(adyacente);
        }
    }

    public void AsignarPropietario(Jugador nuevoPropietario)
    {
        Dueno = nuevoPropietario;
        nuevoPropietario.AnadirDepartamento(this);
    }
    public void RemoverPropietario(Jugador antiguoPropietario)
    {
        Dueno = null;
        antiguoPropietario.RemoverDepartamento(this);
    }

    public void ProcesarProduccionPorTurno(RecursosJugador recursosJugador)
    {
        int dineroProducido = 1000;  // Base fija (puede hacerse variable)
        int recursoProducido = 500;  // Base fija de producción del recurso asignado

        // Ajustes según edificios construidos
        foreach (var edificio in Edificios)
        {
            switch (edificio.Tipo)
            {
                case TipoEdificio.Cuartel:
                    // Ejemplo: +1 tropa infantería por turno
                    ReclutarTropa("infanteria", 1);
                    break;
                case TipoEdificio.Fabrica:
                    recursoProducido += 500;
                    break;
                case TipoEdificio.Establo:
                    recursoProducido += 200;
                    break;
                case TipoEdificio.Fortaleza:
                    // El efecto de fortaleza se aplica en la lógica de combate, no aquí!!!
                    break;
            }
        }

        // Añadir dinero
        recursosJugador.AñadirRecurso("dinero", dineroProducido);

        // Añadir recurso producido según tipo
        switch (RecursoProducido)
        {
            case TipoRecurso.Carbon:
                recursosJugador.AñadirRecurso("carbon", recursoProducido);
                break;
            case TipoRecurso.Comida:
                recursosJugador.AñadirRecurso("comida", recursoProducido);
                break;
            case TipoRecurso.Dinero:
                recursosJugador.AñadirRecurso("dinero", recursoProducido);
                break;
            case TipoRecurso.Hierro:
                recursosJugador.AñadirRecurso("hierro", recursoProducido);
                break;
            case TipoRecurso.Madera:
                recursosJugador.AñadirRecurso("madera", recursoProducido);
                break;
        }

        // Procesar tropas en producción
        ProcesarTurnoProduccion();
    }


    public void ProcesarTurnoProduccion()
    {
        List<Tropa> produccionFinalizada = new List<Tropa>();

        foreach (var tropa in ProduccionEnCurso)
        {
            tropa.TurnosRestantes--;  // Decrementa turnos para cada tropa en producción

            if (tropa.TurnosRestantes <= 0)
                produccionFinalizada.Add(tropa);  // Cuando terminen de construirse, pasarán a producidas
        }

        foreach (var tropa in produccionFinalizada)
        {
            ProduccionEnCurso.Remove(tropa);
            Ejer.AnadirTropa(tropa);  // Agrega la tropa terminada a tropas disponibles
            Debug.Log($"Producción terminada: {tropa.Tipo} en {Nombre}");
        }

        // Actualizar visualmente el mapa con tropas listas (método que debes implementar)
        //MostrarTropasVisuales();
    }

    public void AgregarProduccion(Tropa tropa)
    {
        ProduccionEnCurso.Add(tropa);
    }

    public bool TieneEdificio(TipoEdificio tipo)
    {
        foreach (var edificio in Edificios)
        {
            if (edificio.Tipo == tipo)
                return true;
        }
        return false;
    }

    public void IniciarConstruccion(Edificios edificio)
    {
        ConstruccionesEnCurso.Add(edificio);
    }

    public void ProcesarConstruccionesPorTurno()
    {
        List<Edificios> construccionesFinalizadas = new List<Edificios>();

        foreach (var edificio in ConstruccionesEnCurso)
        {
            edificio.Turnos--;  // Disminuye el contador de turnos

            if (edificio.Turnos <= 0)
            {
                construccionesFinalizadas.Add(edificio);
            }
        }

        foreach (var edificio in construccionesFinalizadas)
        {
            ConstruccionesEnCurso.Remove(edificio);
            Edificios.Add(edificio);

            // Actualiza los flags según el tipo de edificio construido
            switch (edificio.Tipo)
            {
                case TipoEdificio.Cuartel:
                    TieneOficinaReclutamiento = true;
                    break;
                case TipoEdificio.Establo:
                    TieneEstablo = true;
                    break;
                case TipoEdificio.Fabrica:
                    TieneFabrica = true;
                    break;
                case TipoEdificio.Fortaleza:
                    TieneFortaleza = true;
                    break;
            }

            Debug.Log($"Construcción terminada: {edificio.Tipo} en {Nombre}");
        }
    }

    public void CambiarPropietario(Jugador nuevoDueno)
    {
        if (Dueno != null)
        {
            Dueno.Departamentos.Remove(this);
            Dueno.Recursos.Departamentos.Remove(this);
        }

        Dueno = nuevoDueno;
        nuevoDueno.Departamentos.Add(this);
        nuevoDueno.Recursos.Departamentos.Add(this);

        Debug.Log($"El departamento {Nombre} ahora pertenece a {nuevoDueno.Nombre}");
    }

    public void ActualizarImagenTropa()
    {
        if (imagenTropa == null) return;

        string rutaSprite = "infanteria"; // Por defecto

        // Prioridad: Artillería > Caballería > Infantería
        if (Ejer.Tropas.Exists(t => t is Artilleria))
        {
            rutaSprite = "artilleria"; // Asegúrate que el nombre del archivo en Resources sea exactamente este
        }
        else if (Ejer.Tropas.Exists(t => t is Caballeria))
        {
            rutaSprite = "caballeria";
        }

        Sprite nuevoSprite = Resources.Load<Sprite>(rutaSprite);
        if (nuevoSprite != null)
        {
            imagenTropa.sprite = nuevoSprite;
        }
        else
        {
            Debug.LogWarning("No se encontró el sprite: " + rutaSprite);
        }
    }

}