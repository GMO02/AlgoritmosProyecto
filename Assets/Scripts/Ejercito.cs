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
    public void AñadirTropa(Tropa tropa)
    {
        Tropas.Add(tropa);
    }

    // Método para eliminar una tropa del ejército
    public void EliminarTropa(Tropa tropa)
    {
        Tropas.Remove(tropa);
    }

    // Método que simula una pelea entre dos ejércitos
    public void Pelea(Ejercito enemigo)
    {
        while (Tropas.Count > 0 && enemigo.Tropas.Count > 0)
        {
            // Calcular el ataque total y la defensa total de cada ejército
            int ataqueTotal = CalcularAtaqueTotal();
            int defensaTotal = CalcularDefensaTotal();

            int ataqueEnemigoTotal = enemigo.CalcularAtaqueTotal();
            int defensaEnemigoTotal = enemigo.CalcularDefensaTotal();

            // Aplicar el daño entre ambos ejércitos
            Atacar(ataqueTotal, defensaEnemigoTotal, enemigo);
            Atacar(ataqueEnemigoTotal, defensaTotal, this);

            // Eliminar las tropas que hayan muerto (vida <= 0)
            EliminarTropasMuertas();
            enemigo.EliminarTropasMuertas();
        }

        // Determinar el ganador
        if (Tropas.Count > 0)
        {
            Debug.Log("¡Nuestro ejército ha ganado!");
        }
        else if (enemigo.Tropas.Count > 0)
        {
            Debug.Log("¡El ejército enemigo ha ganado!");
        }
        else
        {
            Debug.Log("¡La batalla terminó en empate!");
        }
    }

    // Calcular el ataque total del ejército
    private int CalcularAtaqueTotal()
    {
        return Tropas.Sum(tropa => tropa.Ataque);
    }

    // Calcular la defensa total del ejército
    private int CalcularDefensaTotal()
    {
        return Tropas.Sum(tropa => tropa.Defensa);
    }

    // Aplicar daño basado en el ataque total y la defensa total
    private void Atacar(int ataqueTotal, int defensaTotal, Ejercito enemigo)
    {
        // El daño total se calcula como la diferencia entre el ataque total y la defensa total
        int dañoTotal = Mathf.Max(ataqueTotal - defensaTotal, 0);  // Asegurarse de que el daño no sea negativo

        // Aplicar daño uniformemente a las tropas del ejército enemigo
        foreach (var tropa in enemigo.Tropas)
        {
            // Distribuir el daño de manera uniforme entre las tropas enemigas
            int dañoPorTropa = dañoTotal / enemigo.Tropas.Count;
            tropa.SetVida(tropa.Vida - dañoPorTropa);
        }
    }

    // Eliminar las tropas que hayan muerto (vida <= 0)
    private void EliminarTropasMuertas()
    {
        Tropas.RemoveAll(tropa => tropa.Vida <= 0);
    }
}
