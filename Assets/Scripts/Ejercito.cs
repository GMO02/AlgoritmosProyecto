using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Ejercito
{
    public Jugador Due�o { get; private set; }
    public List<Tropa> Tropas { get; private set; }

    public Ejercito(Jugador due�o)
    {
        Due�o = due�o;
        Tropas = new List<Tropa>();
    }

    // M�todo para a�adir una tropa al ej�rcito
    public void A�adirTropa(Tropa tropa)
    {
        Tropas.Add(tropa);
    }

    // M�todo para eliminar una tropa del ej�rcito
    public void EliminarTropa(Tropa tropa)
    {
        Tropas.Remove(tropa);
    }

    // M�todo que simula una pelea entre dos ej�rcitos
    public void Pelea(Ejercito enemigo)
    {
        while (Tropas.Count > 0 && enemigo.Tropas.Count > 0)
        {
            // Calcular el ataque total y la defensa total de cada ej�rcito
            int ataqueTotal = CalcularAtaqueTotal();
            int defensaTotal = CalcularDefensaTotal();

            int ataqueEnemigoTotal = enemigo.CalcularAtaqueTotal();
            int defensaEnemigoTotal = enemigo.CalcularDefensaTotal();

            // Aplicar el da�o entre ambos ej�rcitos
            Atacar(ataqueTotal, defensaEnemigoTotal, enemigo);
            Atacar(ataqueEnemigoTotal, defensaTotal, this);

            // Eliminar las tropas que hayan muerto (vida <= 0)
            EliminarTropasMuertas();
            enemigo.EliminarTropasMuertas();
        }

        // Determinar el ganador
        if (Tropas.Count > 0)
        {
            Debug.Log("�Nuestro ej�rcito ha ganado!");
        }
        else if (enemigo.Tropas.Count > 0)
        {
            Debug.Log("�El ej�rcito enemigo ha ganado!");
        }
        else
        {
            Debug.Log("�La batalla termin� en empate!");
        }
    }

    // Calcular el ataque total del ej�rcito
    private int CalcularAtaqueTotal()
    {
        return Tropas.Sum(tropa => tropa.Ataque);
    }

    // Calcular la defensa total del ej�rcito
    private int CalcularDefensaTotal()
    {
        return Tropas.Sum(tropa => tropa.Defensa);
    }

    // Aplicar da�o basado en el ataque total y la defensa total
    private void Atacar(int ataqueTotal, int defensaTotal, Ejercito enemigo)
    {
        // El da�o total se calcula como la diferencia entre el ataque total y la defensa total
        int da�oTotal = Mathf.Max(ataqueTotal - defensaTotal, 0);  // Asegurarse de que el da�o no sea negativo

        // Aplicar da�o uniformemente a las tropas del ej�rcito enemigo
        foreach (var tropa in enemigo.Tropas)
        {
            // Distribuir el da�o de manera uniforme entre las tropas enemigas
            int da�oPorTropa = da�oTotal / enemigo.Tropas.Count;
            tropa.SetVida(tropa.Vida - da�oPorTropa);
        }
    }

    // Eliminar las tropas que hayan muerto (vida <= 0)
    private void EliminarTropasMuertas()
    {
        Tropas.RemoveAll(tropa => tropa.Vida <= 0);
    }
}
