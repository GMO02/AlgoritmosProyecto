using UnityEngine;
using GranColombia.Gameplay;

public class ConstruirEdificios : MonoBehaviour
{
    public Departamento provincia;

    public void ConstruirCuartel()
    {
        Edificio cuartel = new Cuartel();
        provincia.edificios.Add(cuartel);
        Debug.Log("¡Se construyó un Cuartel!");
    }

    public void ConstruirEstablo()
    {
        Edificio establo = new Establo();
        provincia.edificios.Add(establo);
        Debug.Log("¡Se construyó un Establo!");
    }

    public void ConstruirFabrica()
    {
        Edificio fabrica = new Fabrica();
        provincia.edificios.Add(fabrica);
        Debug.Log("¡Se construyó una Fábrica!");
    }
}
