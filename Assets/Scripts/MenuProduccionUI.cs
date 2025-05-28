using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuProduccionUI : MonoBehaviour
{
    public TextMeshProUGUI nombreDepartamentoText;

    public Button botonArtilleria;
    public Button botonCaballeria;

    public Button botonCerrar;

    private Departamento departamentoActual;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);

        botonArtilleria.onClick.AddListener(() => ProducirTropa("artilleria"));
        botonCaballeria.onClick.AddListener(() => ProducirTropa("caballeria"));

        botonCerrar.onClick.AddListener(() => gameObject.SetActive(false));
    }

    public void MostrarMenu(Departamento depto)
    {
        departamentoActual = depto;
        gameObject.SetActive(true);
        nombreDepartamentoText.text = depto.Nombre;

        ActualizarBotones();
    }

    public void ActualizarBotones()
    {
        if (departamentoActual == null || departamentoActual.Dueno == null)
        {
            botonArtilleria.interactable = false;
            botonCaballeria.interactable = false;
            return;
        }

        Jugador jugador = departamentoActual.Dueno;
        RecursosJugador recursosJugador = jugador.Recursos;  // Suponiendo referencia

        // Para Artilleria: debe tener Fabrica y recursos para pagar
        bool puedeProducirArtilleria = departamentoActual.TieneFabrica &&
            recursosJugador.PuedePagar(new Artilleria(jugador));

        // Para Caballeria: debe tener Establo y recursos para pagar
        bool puedeProducirCaballeria = departamentoActual.TieneEstablo &&
            recursosJugador.PuedePagar(new Caballeria(jugador));

        botonArtilleria.interactable = puedeProducirArtilleria;
        botonCaballeria.interactable = puedeProducirCaballeria;
    }

    private void ProducirTropa(string tipo)
    {
        if (departamentoActual == null || departamentoActual.Dueno == null)
            return;

        Jugador jugador = departamentoActual.Dueno;
        RecursosJugador recursosJugador = jugador.Recursos;

        switch (tipo.ToLower())
        {
            case "artilleria":
                Artilleria art = new Artilleria(jugador);
                if (departamentoActual.TieneFabrica && recursosJugador.PuedePagar(art))
                {
                    recursosJugador.Pagar(art);
                    departamentoActual.AgregarProduccion(art);
                    Debug.Log($"Producción de Artillería iniciada en {departamentoActual.Nombre}");
                    ActualizarBotones();
                }
                break;

            case "caballeria":
                Caballeria cab = new Caballeria(jugador);
                if (departamentoActual.TieneEstablo && recursosJugador.PuedePagar(cab))
                {
                    recursosJugador.Pagar(cab);
                    departamentoActual.AgregarProduccion(cab);
                    Debug.Log($"Producción de Caballería iniciada en {departamentoActual.Nombre}");
                    ActualizarBotones();
                }
                break;
        }
    }
}
