using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControladorCombate : MonoBehaviour
{
    public GameObject mensajeWin;
    public GameObject mensajeLoose;

    public void IniciarCombate(Departamento atacante, Departamento deptoObjetivo)
    {
        if (deptoObjetivo.Dueno == atacante.Dueno)
        {
            Debug.LogWarning("No puedes atacar un territorio sin dueño o que ya es tuyo.");
            return;
        }

        Ejercito ejercitoAtacante = atacante.Ejer;
        Ejercito ejercitoDefensor = deptoObjetivo.Ejer;

        if (ejercitoAtacante.Tropas.Count == 0)
        {
            Debug.LogWarning("No puedes atacar sin tropas.");
            return;
        }

        // Captura estado previo de tropas para decidir qué mensaje mostrar
        int tropasAntes = ejercitoAtacante.Tropas.Count;

        Ejercito.ResolverBatalla(ejercitoAtacante, ejercitoDefensor, deptoObjetivo);

        if (ejercitoAtacante.Tropas.Count > 0 && deptoObjetivo.Dueno == atacante)
        {
            mensajeWin.SetActive(true);
            mensajeLoose.SetActive(false);
        }
        else
        {
            mensajeWin.SetActive(false);
            mensajeLoose.SetActive(true);
        }

        StartCoroutine(DesactivarMensajes());
    }

    private IEnumerator DesactivarMensajes()
    {
        yield return new WaitForSeconds(2.5f);
        mensajeWin.SetActive(false);
        mensajeLoose.SetActive(false);
    }
}
