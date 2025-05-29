using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SistemaDeTurnos : MonoBehaviour
{
    public List<Jugador> jugadores = new List<Jugador>();  // Lista de jugadores
    private int turnoActual = 0;  // Índice del jugador que tiene el turno

    private MapaControlador mapaControlador;

    // Tiempo en segundos entre cada turno automático
    public float tiempoEntreTurnos = 40f;


    void Start()
    {
        // Buscar automáticamente el grafo en la escena
        mapaControlador = FindObjectOfType<MapaControlador>();

        if (mapaControlador == null)
        {
            Debug.LogError("No se encontró el MapaControlador en la escena.");
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

        // Procesar producción en todos los departamentos del jugador actual
        foreach (var dpto in jugadorActual.Departamentos)
        {
            dpto.ProcesarProduccionPorTurno(jugadorActual.Recursos);
            dpto.ProcesarConstruccionesPorTurno();  // Nueva llamada para construcción de edificios
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
