using UnityEngine;

public class ConstruirEdificios : MonoBehaviour
{
    public Departamento provincia;
    public RecursosJugador recursosJugador;

    public void ConstruirCuartel()
    {
        if (provincia.TieneEdificio(TipoEdificio.Cuartel))
        {
            Debug.Log("Ya existe un Cuartel en esta provincia.");
            return;
        }

        Edificios cuartel = new Cuartel();
        if (recursosJugador.PuedePagar(cuartel))
        {
            recursosJugador.Pagar(cuartel);
            provincia.IniciarConstruccion(cuartel);
            Debug.Log("Se inició la construcción del Cuartel.");
        }
        else
        {
            Debug.Log("No tienes recursos suficientes para construir Cuartel.");
        }
    }

    public void ConstruirEstablo()
    {
        if (provincia.TieneEdificio(TipoEdificio.Establo))
        {
            Debug.Log("Ya existe un Establo en esta provincia.");
            return;
        }

        Edificios establo = new Establo();
        if (recursosJugador.PuedePagar(establo))
        {
            recursosJugador.Pagar(establo);
            provincia.IniciarConstruccion(establo);
            Debug.Log("Se inició la construcción del Establo.");
        }
        else
        {
            Debug.Log("No tienes recursos suficientes para construir Establo.");
        }
    }

    public void ConstruirFabrica()
    {
        if (provincia.TieneEdificio(TipoEdificio.Fabrica))
        {
            Debug.Log("Ya existe una Fábrica en esta provincia.");
            return;
        }

        Edificios fabrica = new Fabrica();
        if (recursosJugador.PuedePagar(fabrica))
        {
            recursosJugador.Pagar(fabrica);
            provincia.IniciarConstruccion(fabrica);
            Debug.Log("Se inició la construcción de la Fábrica.");
        }
        else
        {
            Debug.Log("No tienes recursos suficientes para construir Fábrica.");
        }
    }

    public void ConstruirFortaleza()
    {
        if (provincia.TieneEdificio(TipoEdificio.Fortaleza))
        {
            Debug.Log("Ya existe una Fortaleza en esta provincia.");
            return;
        }

        Edificios fortaleza = new Fortaleza();
        if (recursosJugador.PuedePagar(fortaleza))
        {
            recursosJugador.Pagar(fortaleza);
            provincia.IniciarConstruccion(fortaleza);
            Debug.Log("Se inició la construcción de la Fortaleza.");
        }
        else
        {
            Debug.Log("No tienes recursos suficientes para construir Fortaleza.");
        }
    }

}
