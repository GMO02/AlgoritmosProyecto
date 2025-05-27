using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

public enum TipoEdificio { Ninguno, Cuartel, Establo, Fabrica }

// ---------- Clase base ----------
public abstract class Edificios
{
    public TipoEdificio Tipo { get; protected set; }
    public int Nivel { get; protected set; } = 1;
    public int NivelMaximo => 3;

    public int Turnos;
    public int CostoCarbon;
    public int CostoComida;
    public int CostoDinero;
    public int CostoHierro;
    public int CostoMadera;
    public int sobrecosto_nivel;
    public int[] costo_total;
    public float modificador_Efecto;

    public abstract int[] CostoConstruccion(int nivelActual);
    public abstract float Efecto();

    public bool Mejorar()
    {
        if (Nivel >= NivelMaximo) return false;
        Nivel++;
        sobrecosto_nivel = 1 + (Nivel - 1) / 10;
        modificador_Efecto += 1;
        return true;
    }


}

// ---------- Edificios concretos ----------
public class Cuartel : Edificios
{
    public Cuartel() { 
        Tipo = TipoEdificio.Cuartel;
        Turnos = 1;
        CostoCarbon = 0;
        CostoComida = 1000;
        CostoDinero = 1000;
        CostoHierro = 0;
        CostoMadera = 1000;
        sobrecosto_nivel = 1;
        costo_total = new int[] { CostoCarbon * sobrecosto_nivel, CostoComida * sobrecosto_nivel, CostoDinero * sobrecosto_nivel, CostoHierro * sobrecosto_nivel, CostoMadera * sobrecosto_nivel };
        modificador_Efecto = 0;
    }
    public override int[] CostoConstruccion(int niv_Actual) {
        return costo_total;
    }
    public override float Efecto()
    {
        return modificador_Efecto;
    }
}

public class Establo : Edificios
{
    public Establo() { 
        Tipo = TipoEdificio.Establo;
        Turnos = 2;
        CostoCarbon = 0;
        CostoComida = 1500;
        CostoDinero = 4000;
        CostoHierro = 0;
        CostoMadera = 2000;
        sobrecosto_nivel = 1;
        costo_total = new int[] { CostoCarbon * sobrecosto_nivel, CostoComida * sobrecosto_nivel, CostoDinero * sobrecosto_nivel, CostoHierro * sobrecosto_nivel, CostoMadera * sobrecosto_nivel };
        modificador_Efecto = 0;
    }
    public override int[] CostoConstruccion(int niv_Actual)
    {
        return costo_total;
    }
    public override float Efecto()
    {
        return 1 - (0.1f * modificador_Efecto);
    }
}

public class Fabrica : Edificios
{
    public Fabrica() { 
        Tipo = TipoEdificio.Fabrica;
        Turnos = 4;
        CostoCarbon = 1000;
        CostoComida = 0;
        CostoDinero = 10000;
        CostoHierro = 1000;
        CostoMadera = 1500;
        sobrecosto_nivel = 1;
        costo_total = new int[] { CostoCarbon * sobrecosto_nivel, CostoComida * sobrecosto_nivel, CostoDinero * sobrecosto_nivel, CostoHierro * sobrecosto_nivel, CostoMadera * sobrecosto_nivel };
        modificador_Efecto = 0;
    }
    public override int[] CostoConstruccion(int niv_Actual)
    {
        return costo_total;
    }
    public override float Efecto()
    {
        return 1 - (0.1f * modificador_Efecto); ;
    }
}

public class Fortaleza : Edificios
{
    public Fortaleza()
    {
        Tipo = TipoEdificio.Fabrica;
        Turnos = 3;
        CostoCarbon = 0;
        CostoComida = 0;
        CostoDinero = 2000;
        CostoHierro = 4000;
        CostoMadera = 0;
        sobrecosto_nivel = 1;
        costo_total = new int[] { CostoCarbon * sobrecosto_nivel, CostoComida * sobrecosto_nivel, CostoDinero * sobrecosto_nivel, CostoHierro * sobrecosto_nivel, CostoMadera * sobrecosto_nivel };
        modificador_Efecto = 1; // Modificador de defensa
    }
    public override int[] CostoConstruccion(int niv_Actual)
    {
        return costo_total;
    }
    public override float Efecto()
    {
        return 1 - (modificador_Efecto * 0.2f);
    }
}