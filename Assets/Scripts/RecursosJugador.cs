using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecursosJugador : MonoBehaviour
{
    // Recursos totales que tiene el jugador
    public int Carbon { get; private set; } = 2000;
    public int Comida { get; private set; } = 2000;
    public int Dinero { get; private set; } = 2000;
    public int Hierro { get; private set; } = 2000;
    public int Madera { get; private set; } = 2000;


    // Para conectar con los departamentos de este jugador(debe asignarse desde Jugador)
    public List<Departamento> Departamentos = new List<Departamento>();
    public Jugador jugador;

    private void Start()
    {
        if (jugador == null)
        {
            jugador = GetComponent<Jugador>();
        }
    }

    // Añade recursos según tipo (se usará cuando un departamento produzca)
    public void AñadirRecurso(string recurso, int cantidad)
    {
        switch (recurso.ToLower())
        {
            case "carbon":
                Carbon += cantidad;
                break;
            case "comida":
                Comida += cantidad;
                break;
            case "dinero":
                Dinero += cantidad;
                break;
            case "hierro":
                Hierro += cantidad;
                break;
            case "madera":
                Madera += cantidad;
                break;
            default:
                Debug.LogWarning("Recurso no reconocido: " + recurso);
                break;
        }
    }

    // Verifica si puede pagar una tropa según sus recursos actuales
    public bool PuedePagar(Tropa tropa)
    {
        if (tropa is Artilleria)
        {
            return (Dinero >= 10000 && Hierro >= 4000 && Carbon >= 2000);
        }
        else if (tropa is Caballeria)
        {
            return (Dinero >= 6000 && Madera >= 3000);
        }
        else if (tropa is Infanteria)
        {
            // Aquí puedes definir coste para infantería si quieres
            return true; // Por ejemplo sin coste para infantería
        }
        return false;
    }

    // Verifica si puede pagar un edificio según sus recursos actuales
    public bool PuedePagar(Edificios edificio)
    {
        int[] costo = edificio.CostoConstruccion(edificio.Nivel);
        return Carbon >= costo[0] && Comida >= costo[1] && Dinero >= costo[2] && Hierro >= costo[3] && Madera >= costo[4];
    }

    // Descuenta recursos para construir una tropa
    public void Pagar(Tropa tropa)
    {
        if (tropa is Artilleria)
        {
            Dinero -= 10000;
            Hierro -= 4000;
            Carbon -= 2000;
        }
        else if (tropa is Caballeria)
        {
            Dinero -= 6000;
            Madera -= 3000;
        }
    }

    // Descuenta recursos para construir un edificio
    public void Pagar(Edificios edificio)
    {
        int[] costo = edificio.CostoConstruccion(edificio.Nivel);
        Carbon -= costo[0];
        Comida -= costo[1];
        Dinero -= costo[2];
        Hierro -= costo[3];
        Madera -= costo[4];
    }

    // Llama a todos los departamentos para que produzcan sus recursos por turno
    public void ProducirRecursosDeDepartamentos()
    {
        foreach (var depto in Departamentos)
        {
            depto.ProcesarProduccionPorTurno(this);
        }
    }
}
