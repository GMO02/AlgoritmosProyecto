using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SeleccionPaisController : MonoBehaviour
{
    public TMP_Dropdown dropdownJugador1;
    public TMP_Dropdown dropdownJugador2;
    public Button botonConfirmar;

    private List<string> opcionesOriginales = new List<string> { "Nueva Granada", "Ecuador", "Venezuela" };

    void Start()
    {
        // Carga las opciones en los dropdowns
        dropdownJugador1.ClearOptions();
        dropdownJugador1.AddOptions(opcionesOriginales);

        dropdownJugador2.ClearOptions();
        dropdownJugador2.AddOptions(opcionesOriginales);

        // Cuando cambia la selecci�n de jugador 1, actualiza opciones de jugador 2
        dropdownJugador1.onValueChanged.AddListener(OnJugador1Cambio);

        // Controla el bot�n Confirmar: solo habil�talo si pa�ses son diferentes
        botonConfirmar.interactable = false;
        dropdownJugador1.onValueChanged.AddListener(delegate { ValidarConfirmar(); });
        dropdownJugador2.onValueChanged.AddListener(delegate { ValidarConfirmar(); });
    }

    void OnJugador1Cambio(int index)
    {
        string paisSeleccionado = opcionesOriginales[index];

        // Prepara opciones para jugador 2 sin el pa�s que escogi� jugador 1
        List<string> opcionesJugador2 = new List<string>(opcionesOriginales);
        opcionesJugador2.Remove(paisSeleccionado);

        // Guarda selecci�n previa de jugador 2 (si existe)
        string seleccionActualJugador2 = dropdownJugador2.options[dropdownJugador2.value].text;

        // Actualiza las opciones de jugador 2
        dropdownJugador2.ClearOptions();
        dropdownJugador2.AddOptions(opcionesJugador2);

        // Si la selecci�n previa sigue v�lida, mantenla, si no selecciona la primera opci�n
        int nuevoIndex = opcionesJugador2.IndexOf(seleccionActualJugador2);
        if (nuevoIndex >= 0)
            dropdownJugador2.value = nuevoIndex;
        else
            dropdownJugador2.value = 0;

        dropdownJugador2.RefreshShownValue();

        ValidarConfirmar();
    }

    void ValidarConfirmar()
    {
        // Habilita el bot�n Confirmar solo si las selecciones son diferentes
        botonConfirmar.interactable = dropdownJugador1.value != dropdownJugador2.value;
    }

    public void ConfirmarSeleccion()
    {
        var seleccion = ObtenerPaisesSeleccionados();
        Debug.Log("Jugador 1 eligi�: " + seleccion.Item1);
        Debug.Log("Jugador 2 eligi�: " + seleccion.Item2);

        // Aqu� contin�a la l�gica para empezar el juego con esos pa�ses.
        // Por ejemplo: ocultar el panel y activar el mapa, cargar datos, etc.
    }


    public (string, string) ObtenerPaisesSeleccionados()
    {
        string pais1 = opcionesOriginales[dropdownJugador1.value];
        List<string> opcionesJugador2 = new List<string>(opcionesOriginales);
        opcionesJugador2.Remove(pais1);
        string pais2 = opcionesJugador2[dropdownJugador2.value];
        return (pais1, pais2);
    }
}
