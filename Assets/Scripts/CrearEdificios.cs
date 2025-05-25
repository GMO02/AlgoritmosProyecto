using UnityEngine;
using GranColombia.Gameplay;

public class ConstruirEdificios : MonoBehaviour
{
    public Departamento provincia;

    public void ConstruirCuartel()
    {
        Edificio cuartel = new Cuartel();
        provincia.Edificios.Add(cuartel);
        Debug.Log("¡Se construyó un Cuartel!");
    }

    public void ConstruirEstablo()
    {
        Edificio establo = new Establo();
        provincia.Edificios.Add(establo);
        Debug.Log("¡Se construyó un Establo!");
    }

    public void ConstruirFabrica()
    {
        Edificio fabrica = new Fabrica();
        provincia.Edificios.Add(fabrica);
        Debug.Log("¡Se construyó una Fábrica!");
    }
}
