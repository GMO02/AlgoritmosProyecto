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

    // M�todo para a�adir una tropa al ej�rcito
    public void AnadirTropa(Tropa tropa)
    {
        Tropas.Add(tropa);
    }

    // M�todo para eliminar una tropa del ej�rcito
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
            Debug.Log("�El atacante ha ganado la batalla!");
            defensor.Tropas.Clear(); // Todas las tropas defensoras mueren

            departamentoDefendido.CambiarPropietario(atacante.Dueno);

            // Eliminar un edificio al azar
            if (departamentoDefendido.Edificios.Count > 0)
            {
                int indice = Random.Range(0, departamentoDefendido.Edificios.Count);
                departamentoDefendido.Edificios.RemoveAt(indice);
            }

            // Eliminar tropas del atacante seg�n da�o recibido y equivalencias
            int da�oRecibido = defensaTotal;

            // Orden de eliminaci�n: artiller�a (4), caballer�a (2), infanter�a (1)
            EliminarTropasPorDa�o(atacante.Tropas, da�oRecibido);

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
            Debug.Log("�El defensor ha repelido el ataque!");
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
            Debug.Log("�La batalla termin� en empate!");
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

    private static void EliminarTropasPorDa�o(List<Tropa> tropas, int da�o)
    {
        // Conversi�n: artiller�a = 4, caballer�a = 2, infanter�a = 1
        int puntos = da�o;

        // Eliminar artiller�a
        var artillerias = tropas.Where(t => t is Artilleria).ToList();
        foreach (var a in artillerias)
        {
            if (puntos >= 4)
            {
                tropas.Remove(a);
                puntos -= 4;
            }
        }

        // Eliminar caballer�a
        var caballerias = tropas.Where(t => t is Caballeria).ToList();
        foreach (var c in caballerias)
        {
            if (puntos >= 2)
            {
                tropas.Remove(c);
                puntos -= 2;
            }
        }

        // Eliminar infanter�a
        var infanterias = tropas.Where(t => t is Infanteria).ToList();
        foreach (var i in infanterias)
        {
            if (puntos >= 1)
            {
                tropas.Remove(i);
                puntos -= 1;
            }
        }

        // Lo que sobre no afecta tropas (por ejemplo, puntos = 1 pero no hay infanter�a)
    }

    public (int infanteria, int caballeria, int artilleria) ObtenerConteoTropas()
    {
        int inf = Tropas.Count(t => t is Infanteria);
        int cab = Tropas.Count(t => t is Caballeria);
        int art = Tropas.Count(t => t is Artilleria);
        return (inf, cab, art);
    }

}
