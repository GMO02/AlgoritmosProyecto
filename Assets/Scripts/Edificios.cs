// ============================
// SistemaEdificios.cs  (archivo único y corregido)
// Las directivas "using" van al inicio, antes de cualquier espacio de nombres o código.
// También envolvemos todo en un namespace para evitar colisiones.
// ============================
using System.Collections.Generic;
using UnityEngine;

namespace GranColombia.Gameplay
{
    public enum TipoEdificioProvincia { Ninguno, Cuartel, Establo, Fabrica }

    // ---------- Clase base ----------
    public abstract class Edificio
    {
        public TipoEdificioProvincia Tipo { get; protected set; }
        public int Nivel { get; protected set; } = 1;
        public int NivelMaximo => 3;

        public abstract int CostoConstruccion(int nivelActual);
        public abstract int CostoMejora(int siguienteNivel);

        public bool Mejorar()
        {
            if (Nivel >= NivelMaximo) return false;
            Nivel++;
            return true;
        }

        public abstract void AplicarEfecto(Departamento provincia);
    }

    // ---------- Edificios concretos ----------
    public class Cuartel : Edificio
    {
        public Cuartel() { Tipo = TipoEdificioProvincia.Cuartel; }
        public override int CostoConstruccion(int _) => 100;
        public override int CostoMejora(int sig) => 50 * sig;
        public override void AplicarEfecto(Departamento p) => p.ReclutasPendientes += 2 * Nivel;
    }

    public class Establo : Edificio
    {
        public Establo() { Tipo = TipoEdificioProvincia.Establo; }
        public override int CostoConstruccion(int _) => 120;
        public override int CostoMejora(int sig) => 60 * sig;
        public override void AplicarEfecto(Departamento p) => p.ModificadorCostoCaballeria = 1f - 0.10f * Nivel;
    }

    public class Fabrica : Edificio
    {
        public Fabrica() { Tipo = TipoEdificioProvincia.Fabrica; }
        public override int CostoConstruccion(int _) => 150;
        public override int CostoMejora(int sig) => 75 * sig;
        public override void AplicarEfecto(Departamento p) => p.EquipamientoProducido += 3 * Nivel;
    }
}



