using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuConstruccionUI : MonoBehaviour
{

    public TextMeshProUGUI nombreDepartamentoText;

    public Button botonConstruirCuartel;
    public Button botonConstruirEstablo;
    public Button botonConstruirFabrica;
    public Button botonConstruirFortaleza;

    public Button botonCerrar;

    private Departamento departamentoActual;

    private bool edificioConstruidoEsteTurno = false;  // Para evitar m�ltiples construcciones por turno



    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);

        botonConstruirCuartel.onClick.AddListener(() => ConstruirEdificio(TipoEdificio.Cuartel));
        botonConstruirEstablo.onClick.AddListener(() => ConstruirEdificio(TipoEdificio.Establo));
        botonConstruirFabrica.onClick.AddListener(() => ConstruirEdificio(TipoEdificio.Fabrica));
        botonConstruirFortaleza.onClick.AddListener(() => ConstruirEdificio(TipoEdificio.Fortaleza));

        botonCerrar.onClick.AddListener(() => gameObject.SetActive(false));
    }

    public void MostrarMenu(Departamento depto)
    {
        departamentoActual = depto;
        gameObject.SetActive(true);
        nombreDepartamentoText.text = depto.Nombre;

        edificioConstruidoEsteTurno = false; // Resetea al abrir el men�
        ActualizarBotones();
    }

    public void ActualizarBotones()
    {
        if (departamentoActual == null || departamentoActual.Dueno == null)
        {
            SetBotonesInteractivos(false);
            return;
        }

        Jugador jugador = departamentoActual.Dueno;
        RecursosJugador recursosJugador = jugador.Recursos;

        // Solo permitir construir si no se construy� nada este turno
        if (edificioConstruidoEsteTurno)
        {
            SetBotonesInteractivos(false);
            return;
        }

        // Cuartel
        botonConstruirCuartel.interactable = !departamentoActual.TieneOficinaReclutamiento &&
            recursosJugador.PuedePagar(new Cuartel());

        // Establo
        botonConstruirEstablo.interactable = !departamentoActual.TieneEstablo &&
            recursosJugador.PuedePagar(new Establo());

        // Fabrica
        botonConstruirFabrica.interactable = !departamentoActual.TieneFabrica &&
            recursosJugador.PuedePagar(new Fabrica());

        // Fortaleza
        // Asumo que fortaleza es una sola y no se puede construir m�s de una vez
        botonConstruirFortaleza.interactable = !departamentoActual.Edificios.Exists(e => e.Tipo == TipoEdificio.Fortaleza) &&
            recursosJugador.PuedePagar(new Fortaleza());
    }

    private void SetBotonesInteractivos(bool estado)
    {
        botonConstruirCuartel.interactable = estado;
        botonConstruirEstablo.interactable = estado;
        botonConstruirFabrica.interactable = estado;
        botonConstruirFortaleza.interactable = estado;
    }

    private void ConstruirEdificio(TipoEdificio tipo)
    {
        if (departamentoActual == null || departamentoActual.Dueno == null)
            return;

        Jugador jugador = departamentoActual.Dueno;
        RecursosJugador recursosJugador = jugador.Recursos;

        Edificios edificio = null;

        switch (tipo)
        {
            case TipoEdificio.Cuartel:
                edificio = new Cuartel();
                break;
            case TipoEdificio.Establo:
                edificio = new Establo();
                break;
            case TipoEdificio.Fabrica:
                edificio = new Fabrica();
                break;
            case TipoEdificio.Fortaleza:
                edificio = new Fortaleza();
                break;
        }

        if (edificio != null && recursosJugador.PuedePagar(edificio))
        {
            recursosJugador.Pagar(edificio);
            departamentoActual.Edificios.Add(edificio);
            edificioConstruidoEsteTurno = true;

            Debug.Log($"Construcci�n iniciada: {tipo} en {departamentoActual.Nombre}");

            ActualizarBotones();
        }
    }
}
