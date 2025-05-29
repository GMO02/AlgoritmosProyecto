using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SistemaDeTurnos : MonoBehaviour
{
    public List<Jugador> jugadores = new List<Jugador>();  // Lista de jugadores
    private int turnoActual = 0;  // �ndice del jugador que tiene el turno

    private MapaControlador mapaControlador;

    // Tiempo en segundos entre cada turno autom�tico
    public float tiempoEntreTurnos = 40f;


    void Start()
    {
        // Buscar autom�ticamente el grafo en la escena
        mapaControlador = FindObjectOfType<MapaControlador>();

        if (mapaControlador == null)
        {
            Debug.LogError("No se encontr� el MapaControlador en la escena.");
            return;
        }

        if (jugadores.Count == 0)
        {
            Debug.LogError("No hay jugadores asignados al SistemaDeTurnos.");
            return;
        }

        Debug.Log("Comienza el turno de: " + jugadores[turnoActual].Nombre);
    }

    public void SiguienteTurno()
    {
        turnoActual = (turnoActual + 1) % jugadores.Count;
        Jugador jugadorActual = jugadores[turnoActual];
        Debug.Log("Turno de: " + jugadores[turnoActual].Nombre);

        // Procesar producci�n en todos los departamentos del jugador actual
        foreach (var dpto in jugadorActual.Departamentos)
        {
            dpto.ProcesarProduccionPorTurno(jugadorActual.Recursos);
            dpto.ProcesarConstruccionesPorTurno();  // Nueva llamada para construcci�n de edificios
        }

    }

    public Jugador ObtenerJugadorActual()
    {
        return jugadores[turnoActual];
    }

    IEnumerator CicloTurnosAutomatico()
    {
        while (true) // Bucle infinito
        {
            yield return new WaitForSeconds(tiempoEntreTurnos);
            SiguienteTurno();
        }
    }
    
}
