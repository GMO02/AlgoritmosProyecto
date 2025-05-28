using System.Collections.Generic;
using UnityEngine;

public class SistemaDeTurnos : MonoBehaviour
{
    public List<Jugador> jugadores = new List<Jugador>();  // Lista de jugadores
    private int turnoActual = 0;  // Índice del jugador que tiene el turno

    private MapaControlador mapaControlador;

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

    // Ejemplo opcional: Asignar departamentos aleatoriamente (si no se han asignado aún)
    public void AsignarDepartamentosIniciales()
    {
        Dictionary<string, Departamento> todos = mapaControlador.ObtenerDepartamentos(); // Usar la instancia de mapaControlador
        int i = 0;
        foreach (var depto in todos.Values)
        {
            Jugador jugadorAsignado = jugadores[i % jugadores.Count];
            jugadorAsignado.AnadirDepartamento(depto);
            depto.AsignarPropietario(jugadorAsignado); // Este método debe existir en tu clase Departamento
            i++;
        }
    }
}
