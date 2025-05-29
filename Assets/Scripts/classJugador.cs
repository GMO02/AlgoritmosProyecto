using System.Collections.Generic;
using UnityEngine;

public class Jugador:MonoBehaviour
{
    public string Nombre { get; private set; }
    public int ID;
    public List<Departamento> Departamentos { get; private set; }

    // Referencia al sistema que maneja los recursos de este jugador
    public RecursosJugador Recursos;
    public Jugador jugador;

    private void Start()
    {
        if (jugador == null)
        {
            jugador = GetComponent<Jugador>();
        }
    }
    public Jugador(string nombre, int iD)
    {
        Nombre = nombre;
        Departamentos = new List<Departamento>();
        ID = iD;
    }

    public int getID()
    {
        return ID;
    }

    public void AnadirDepartamento(Departamento dpto)
    {
        if (!Departamentos.Contains(dpto))
        {
            Departamentos.Add(dpto);
        }
    }

    public void RemoverDepartamento(Departamento dpto)
    {
        if (Departamentos.Contains(dpto))
        {
            Departamentos.Remove(dpto);
        }
    }

    public bool Controla(Departamento dpto)
    {
        return Departamentos.Contains(dpto);
    }

}