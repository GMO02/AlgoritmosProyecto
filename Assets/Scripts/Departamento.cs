using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Para mostrar datos si usas UI

public class Departamento : MonoBehaviour
{
    public string Nombre;
    public SpriteRenderer spriteRenderer;
    public List<Departamento> Adyacentes;


    [SerializeField] private Jugador jugador1;  // Jugador 1, ahora visible en el Inspector
    [SerializeField] private Jugador jugador2;

    public AudioClip clickSound;
    private AudioSource audioSource;

    private static Departamento departamentoSeleccionado = null;
    
    public Jugador Dueño;
    public List<Tropa> Tropas = new List<Tropa>();

    // Edificios
    public bool TieneOficinaInfanteria = false;
    public bool TieneOficinaArtilleria = false;
    public bool TieneOficinaCaballeria = false;

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

        originalColor = spriteRenderer.color;

        Tropas = new List<Tropa>(); // Inicializar la lista de tropas
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

            MostrarDatos();
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
    }

    private void DesmarcarDepartamento()
    {
        spriteRenderer.color = originalColor;
        departamentoSeleccionado = null;
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
        if (jugador1.Controla(this))
        {
            SpriteJugador1();
        }
        else
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
        Debug.Log("Dueño: " + (Dueño != null ? Dueño.Nombre : "Sin dueño"));
        Debug.Log("Tropas: " + Tropas.Count);
        Debug.Log("Infantería: " + Tropas.FindAll(t => t is Infanteria).Count);
        Debug.Log("Artillería: " + Tropas.FindAll(t => t is Artilleria).Count);
        Debug.Log("Caballería: " + Tropas.FindAll(t => t is Caballeria).Count);
        Debug.Log("Oficinas: " +
            (TieneOficinaInfanteria ? "Infantería " : "") +
            (TieneOficinaArtilleria ? "Artillería " : "") +
            (TieneOficinaCaballeria ? "Caballería " : ""));
    }

    public void ConstruirOficina(string tipo)
    {
        // Aquí podrías verificar si el jugador tiene recursos
        switch (tipo.ToLower())
        {
            case "infanteria":
                TieneOficinaInfanteria = true;
                break;
            case "artilleria":
                TieneOficinaArtilleria = true;
                break;
            case "caballeria":
                TieneOficinaCaballeria = true;
                break;
        }

        Debug.Log($"Oficina de {tipo} construida en {Nombre}");
    }

    public void ReclutarTropa(string tipo)
    {
        switch (tipo.ToLower())
        {
            case "infanteria":
                if (TieneOficinaInfanteria)
                    Tropas.Add(new Infanteria(Dueño));
                break;
            case "artilleria":
                if (TieneOficinaArtilleria)
                    Tropas.Add(new Artilleria(Dueño));
                break;
            case "caballeria":
                if (TieneOficinaCaballeria)
                    Tropas.Add(new Caballeria(Dueño));
                break;
        }
    }

    public void Conquistar(Jugador nuevoDueño)
    {
        if (Dueño != null)
        {
            Dueño.RemoverDepartamento(this);
        }

        AsignarPropietario(nuevoDueño);
        Tropas.Clear();
        Debug.Log($"{Nombre} fue conquistado por {nuevoDueño.Nombre}");
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
        Dueño = nuevoPropietario;
        nuevoPropietario.AñadirDepartamento(this);
    }
    public void RemoverPropietario(Jugador antiguoPropietario)
    {
        Dueño = null;
        antiguoPropietario.RemoverDepartamento(this);
    }
}
