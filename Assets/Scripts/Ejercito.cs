using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Ejercito
{
    public Jugador Dueño { get; private set; }
    public List<Tropa> Tropas { get; private set; }

    public Ejercito(Jugador dueño)
    {
        Dueño = dueño;
        Tropas = new List<Tropa>();
    }

    // Método para añadir una tropa al ejército
    public void AnadirTropa(Tropa tropa)
    {
        Tropas.Add(tropa);
    }

    // Método para eliminar una tropa del ejército
    public void EliminarTropa(Tropa tropa)
    {
        Tropas.Remove(tropa);
    }

    public static void ResolverBatalla(Ejercito atacante, Ejercito defensor, Departamento departamentoDefendido)
    {
        int ataqueTotal = atacante.Tropas.Sum(t => t.Ataque);
        int defensaTotal = defensor.Tropas.Sum(t => t.Defensa);

        int vidaAtacanteTotal = atacante.Tropas.Sum(t => t.Vida);
        int vidaDefensorTotal = defensor.Tropas.Sum(t => t.Vida);

        int vidaRestanteDefensor = vidaDefensorTotal - ataqueTotal;
        int vidaRestanteAtacante = vidaAtacanteTotal - defensaTotal;

        vidaRestanteDefensor = Mathf.Max(vidaRestanteDefensor, 0);
        vidaRestanteAtacante = Mathf.Max(vidaRestanteAtacante, 0);

        if (vidaRestanteAtacante > vidaRestanteDefensor)
        {
            // Gana el atacante
            Debug.Log("¡El atacante ha ganado la batalla!");
            defensor.Tropas.Clear(); // Todas las tropas defensoras mueren

            departamentoDefendido.CambiarPropietario(atacante.Dueño);

            // Eliminar un edificio al azar
            if (departamentoDefendido.Edificios.Count > 0)
            {
                int indice = Random.Range(0, departamentoDefendido.Edificios.Count);
                departamentoDefendido.Edificios.RemoveAt(indice);
            }

            // Las tropas sobrevivientes del atacante quedan tal cual
        }
        else if (vidaRestanteDefensor > vidaRestanteAtacante)
        {
            // Gana el defensor
            Debug.Log("¡El defensor ha repelido el ataque!");
            atacante.Tropas.Clear(); // Todas las tropas atacantes mueren
        }
        else
        {
            // Empate: todos pierden sus tropas
            Debug.Log("¡La batalla terminó en empate!");
            atacante.Tropas.Clear();
            defensor.Tropas.Clear();
        }
    }
}
