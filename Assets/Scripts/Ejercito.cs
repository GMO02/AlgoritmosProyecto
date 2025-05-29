using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Ejercito
{
    public Jugador Dueno { get; private set; }
    public List<Tropa> Tropas { get; private set; }

    public int ContarInfanteria() => Tropas.Count(t => t is Infanteria);
    public int ContarCaballeria() => Tropas.Count(t => t is Caballeria);
    public int ContarArtilleria() => Tropas.Count(t => t is Artilleria);

    public Ejercito(Jugador dueno)
    {
        Dueno = dueno;
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
        float modif =1f;
        if (departamentoDefendido.TieneFortaleza) {
            modif = 0.8f;
        }
        int ataqueTotal = Mathf.RoundToInt(atacante.Tropas.Sum(t => t.Ataque)*modif);
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

            departamentoDefendido.CambiarPropietario(atacante.Dueno);

            // Eliminar un edificio al azar
            if (departamentoDefendido.Edificios.Count > 0)
            {
                int indice = Random.Range(0, departamentoDefendido.Edificios.Count);
                departamentoDefendido.Edificios.RemoveAt(indice);
            }

            // Eliminar tropas del atacante según daño recibido y equivalencias
            int dañoRecibido = defensaTotal;

            // Orden de eliminación: artillería (4), caballería (2), infantería (1)
            EliminarTropasPorDaño(atacante.Tropas, dañoRecibido);

            atacante.ObtenerConteoTropas();
            defensor.ObtenerConteoTropas();

            departamentoDefendido.ActualizarImagenTropa();

            var ui = GameObject.FindObjectOfType<MenuDepartamentoUI>();
            if (ui != null && ui.gameObject.activeSelf)
            {
                ui.MostrarDepartamento(departamentoDefendido);
            }

        }
        else if (vidaRestanteDefensor > vidaRestanteAtacante)
        {
            // Gana el defensor
            Debug.Log("¡El defensor ha repelido el ataque!");
            atacante.Tropas.Clear(); // Todas las tropas atacantes mueren
            departamentoDefendido.ActualizarImagenTropa();

            atacante.ObtenerConteoTropas();
            defensor.ObtenerConteoTropas();

            var ui = GameObject.FindObjectOfType<MenuDepartamentoUI>();
            if (ui != null && ui.gameObject.activeSelf)
            {
                ui.MostrarDepartamento(departamentoDefendido);
            }
        }
        else
        {
            // Empate: todos pierden sus tropas
            Debug.Log("¡La batalla terminó en empate!");
            atacante.Tropas.Clear();
            defensor.Tropas.Clear();
            departamentoDefendido.ActualizarImagenTropa();

            atacante.ObtenerConteoTropas();
            defensor.ObtenerConteoTropas();

            var ui = GameObject.FindObjectOfType<MenuDepartamentoUI>();
            if (ui != null && ui.gameObject.activeSelf)
            {
                ui.MostrarDepartamento(departamentoDefendido);
            }
        }
    }

    private static void EliminarTropasPorDaño(List<Tropa> tropas, int daño)
    {
        // Conversión: artillería = 4, caballería = 2, infantería = 1
        int puntos = daño;

        // Eliminar artillería
        var artillerias = tropas.Where(t => t is Artilleria).ToList();
        foreach (var a in artillerias)
        {
            if (puntos >= 4)
            {
                tropas.Remove(a);
                puntos -= 4;
            }
        }

        // Eliminar caballería
        var caballerias = tropas.Where(t => t is Caballeria).ToList();
        foreach (var c in caballerias)
        {
            if (puntos >= 2)
            {
                tropas.Remove(c);
                puntos -= 2;
            }
        }

        // Eliminar infantería
        var infanterias = tropas.Where(t => t is Infanteria).ToList();
        foreach (var i in infanterias)
        {
            if (puntos >= 1)
            {
                tropas.Remove(i);
                puntos -= 1;
            }
        }

        // Lo que sobre no afecta tropas (por ejemplo, puntos = 1 pero no hay infantería)
    }

    public (int infanteria, int caballeria, int artilleria) ObtenerConteoTropas()
    {
        int inf = Tropas.Count(t => t is Infanteria);
        int cab = Tropas.Count(t => t is Caballeria);
        int art = Tropas.Count(t => t is Artilleria);
        return (inf, cab, art);
    }

}
