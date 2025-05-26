using System.Collections.Generic;
using UnityEngine;

public class Jugador:MonoBehaviour
{
    public string Nombre { get; private set; }
    public List<Departamento> Departamentos { get; private set; }

    public Jugador(string nombre)
    {
        Nombre = nombre;
        Departamentos = new List<Departamento>();
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