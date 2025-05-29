using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnoAppear : MonoBehaviour
{
    public SpriteRenderer SiguienteTurno;
    public float intervalo = 30f;     // Cada cuántos segundos aparece
    public float duracionVisible = 3f; // Cuánto tiempo permanece visible

    // Start is called before the first frame update
    void Start()
    {
        if (SiguienteTurno != null)
        {
            SiguienteTurno.enabled = false; // Inicia invisible
        }

        StartCoroutine(AparecerPeriodicamente());
    }

    IEnumerator AparecerPeriodicamente()
    {
        while (true)
        {
            yield return new WaitForSeconds(intervalo);

            if (SiguienteTurno != null)
                SiguienteTurno.enabled = true; // Mostrar

            yield return new WaitForSeconds(duracionVisible);

            if (SiguienteTurno != null)
                SiguienteTurno.enabled = false; // Ocultar
        }
    }
}
