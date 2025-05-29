using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerSelectionManager : MonoBehaviour
{
    public Button[] botonesJugador1;
    public Button[] botonesJugador2;
    public Button botonConfirmar;

    private string paisJugador1 = "";
    private string paisJugador2 = "";

    // Lista de países en el mismo orden que los botones
    private string[] nombresPaises = { "Nueva Granada", "Ecuador", "Venezuela" };

    void Start()
    {
        botonConfirmar.interactable = false;

        for (int i = 0; i < botonesJugador1.Length; i++)
        {
            string paisLocal = nombresPaises[i];

            // Asignar listeners de selección
            botonesJugador1[i].onClick.AddListener(() => SeleccionarPaisJugador1(paisLocal));
            botonesJugador2[i].onClick.AddListener(() => SeleccionarPaisJugador2(paisLocal));

            // Desactivar botones del Jugador 2 inicialmente
            botonesJugador2[i].interactable = false;
        }

        botonConfirmar.onClick.AddListener(() => IrAEscenaJuego());
    }

    public void SeleccionarPaisJugador1(string pais)
    {
        paisJugador1 = pais;
        Debug.Log("Jugador 1 selecciono: " + paisJugador1);

        // Desactivar todos los botones del Jugador 1
        foreach (Button boton in botonesJugador1)
            boton.interactable = false;

        // Activar botones del Jugador 2 excepto el país elegido por el Jugador 1
        for (int i = 0; i < botonesJugador2.Length; i++)
        {
            if (nombresPaises[i] == paisJugador1)
            {
                botonesJugador2[i].interactable = false;
                ColorBlock cb = botonesJugador2[i].colors;
                cb.normalColor = new Color(1, 1, 1, 0.3f); // Hacerlo más opaco visualmente
                botonesJugador2[i].colors = cb;
            }
            else
            {
                botonesJugador2[i].interactable = true;
            }
        }
    }

    public void SeleccionarPaisJugador2(string pais)
    {
        paisJugador2 = pais;
        Debug.Log("Jugador 2 seleccionó: " + paisJugador2);

        // Desactivar todos los botones del Jugador 2
        foreach (Button boton in botonesJugador2)
            boton.interactable = false;

        VerificarConfirmacion();
    }

    private void VerificarConfirmacion()
    {
        if (!string.IsNullOrEmpty(paisJugador1) && !string.IsNullOrEmpty(paisJugador2))
        {
            botonConfirmar.interactable = true;
            Debug.Log("Ambos países seleccionados, botón Confirmar activado.");
        }
        else
        {
            botonConfirmar.interactable = false;
        }
    }

    public void IrAEscenaJuego()
    {
        PlayerPrefs.SetString("PaisJugador1", paisJugador1);
        PlayerPrefs.SetString("PaisJugador2", paisJugador2);
        Debug.Log("Cambiando a escena con: " + paisJugador1 + " y " + paisJugador2);
        SceneManager.LoadScene("SampleScene"); // Cambia esto si tu escena tiene otro nombre
    }
}
