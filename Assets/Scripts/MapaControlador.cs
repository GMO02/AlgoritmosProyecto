using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapaControlador : MonoBehaviour
{
    // Diccionario para almacenar los departamentos por nombre
    private Dictionary<string, Departamento> departamentos;

    // Diccionario para almacenar los sprites de los departamentos
    public Sprite[] spritesDepartamentos;

    // Conexiones entre departamentos (usando el nombre como clave)
    private Dictionary<string, List<string>> conexiones;

    private List<string> provinciasVenezuela = new List<string> { "venezuela (1)", "orinoco (1)", "apure (1)", "zulia (1)" };
    private List<string> provinciasNuevaGranada = new List<string> {"cauca (1)", "cundinamarca (1)", "boyaca (1)", "magdalena (1)", "panama (1)" };
    private List<string> provinciasEcuador = new List<string>{ "ecuador (1)", "guayaquil (1)", "asuay (1)" };

    public List<Departamento> todosLosDepartamentos;

    public Jugador jugador1;
    public Jugador jugador2;
    public Dictionary<string, Departamento> ObtenerDepartamentos()
    {
        return departamentos;
    }

    // Start is called before the first frame update
    void Start()
    {
        departamentos = new Dictionary<string, Departamento>();
        conexiones = new Dictionary<string, List<string>>();

        // Inicializa los departamentos
        CrearDepartamentos();

        // Establecer las conexiones entre departamentos
        EstablecerConexiones();

        //AsignarDepartamentosSegunPaisSeleccionado();
    }

    void CrearDepartamentos()
    {
        // Usar un arreglo o lista para los nombres de los departamentos
        string[] nombresDepartamentos = { "asuay (1)", "guayaquil (1)", "ecuador (1)", "cauca (1)", "cundinamarca (1)", "orinoco (1)", "boyaca (1)", "apure (1)", "venezuela (1)", "zulia (1)", "magdalena (1)", "panama (1)" };

        // Iterar sobre los nombres de departamentos y asignar los GameObjects ya existentes
        foreach (var nombre in nombresDepartamentos)
        {
            // Buscar el GameObject correspondiente en la escena
            GameObject deptoObj = GameObject.Find(nombre);  // Buscar el GameObject por nombre

            // Verificar si el GameObject existe
            if (deptoObj != null)
            {
                // Obtener el componente Departamento del GameObject
                Departamento depto = deptoObj.GetComponent<Departamento>();

                // Asignar el sprite correspondiente al departamento
                if (depto != null && depto.spriteRenderer == null)
                {
                    depto.spriteRenderer = deptoObj.GetComponent<SpriteRenderer>();  // Asegurarse de que el SpriteRenderer esté asignado
                    depto.spriteRenderer.sprite = ObtenerSpritePorNombre(nombre); // Asignar el sprite
                }

                // Agregar el departamento al diccionario de departamentos
                departamentos.Add(nombre, depto);
            }
            else
            {
                Debug.LogWarning("GameObject no encontrado para el departamento: " + nombre);
            }
        }
    }

    Sprite ObtenerSpritePorNombre(string nombre)
    {
        // Asignar un sprite según el nombre del departamento
        switch (nombre)
        {
            case "asuay (1)":
                return spritesDepartamentos[0];
            case "guayaquil (1)":
                return spritesDepartamentos[1];
            case "ecuador (1)":
                return spritesDepartamentos[2];
            case "cauca (1)":
                return spritesDepartamentos[3];
            case "cundinamarca (1)":
                return spritesDepartamentos[4];
            case "orinoco (1)":
                return spritesDepartamentos[5];
            case "boyaca (1)":
                return spritesDepartamentos[6];
            case "apure (1)":
                return spritesDepartamentos[7];
            case "venezuela (1)":
                return spritesDepartamentos[8];
            case "zulia (1)":
                return spritesDepartamentos[9];
            case "magdalena (1)":
                return spritesDepartamentos[10];
            case "panama (1)":
                return spritesDepartamentos[11];
            default:
                return null;
        }
    }

    void EstablecerConexiones()
    {
        conexiones.Add("asuay (1)", new List<string> { "guayaquil (1)", "ecuador (1)", "cauca (1)" });
        conexiones.Add("guayaquil (1)", new List<string> { "asuay (1)", "ecuador (1)" });
        conexiones.Add("ecuador (1)", new List<string> { "guayaquil (1)", "asuay (1)", "cauca (1)" });
        conexiones.Add("cauca (1)", new List<string> { "cundinamarca (1)", "ecuador (1)", "panama (1)", "asuay (1)", "orinoco (1)" });
        conexiones.Add("cundinamarca (1)", new List<string> { "magdalena (1)", "boyaca (1)", "cauca (1)", "apure (1)", "orinoco (1)" });
        conexiones.Add("boyaca (1)", new List<string> { "cundinamarca (1)", "magdalena (1)", "zulia (1)", "apure (1)" });
        conexiones.Add("magdalena (1)", new List<string> { "cundinamarca (1)", "boyaca (1)", "zulia (1)" });
        conexiones.Add("zulia (1)", new List<string> { "magdalena (1)", "boyaca (1)", "venezuela (1)", "apure (1)" });
        conexiones.Add("apure (1)", new List<string> { "venezuela (1)", "boyaca (1)", "zulia (1)", "cundinamarca (1)", "orinoco (1)" });
        conexiones.Add("venezuela (1)", new List<string> { "zulia (1)", "apure (1)", "orinoco (1)" });
        conexiones.Add("orinoco (1)", new List<string> { "venezuela (1)", "cundinamarca (1)", "cauca (1)", "apure (1)" });
        conexiones.Add("panama (1)", new List<string> { "cauca (1)" });


        // Establecer las relaciones adyacentes en los departamentos
        foreach (var depto in conexiones)
        {
            Departamento deptoActual = departamentos[depto.Key];
            foreach (var adyacente in depto.Value)
            {
                deptoActual.AgregarAdyacente(departamentos[adyacente]);
            }
        }
    }

    public void AsignarDepartamentosSegunPaisSeleccionado()
    {
        string paisJ1 = PlayerPrefs.GetString("PaisJugador1");
        string paisJ2 = PlayerPrefs.GetString("PaisJugador2");

        List<string> provinciasJ1 = ObtenerProvinciasPorPais(paisJ1);
        List<string> provinciasJ2 = ObtenerProvinciasPorPais(paisJ2);

        List<Departamento> departamentosLibres = new List<Departamento>();
        int i = 0;
        while (i < todosLosDepartamentos.Count)
        {
            var depto = todosLosDepartamentos[i];
            string nombreDepto = depto.Nombre.ToLower();

            if (provinciasJ1.Exists(p => p.ToLower() == nombreDepto))
            {
                depto.AsignarPropietario(jugador1);
            }
            else if (provinciasJ2.Exists(p => p.ToLower() == nombreDepto))
            {
                depto.AsignarPropietario(jugador2);
            }
            else
            {
                departamentosLibres.Add(depto);
            }
            i++;
        }

        // Repartir equitativamente los departamentos libres
        for (int j = 0; j < departamentosLibres.Count; j++)
        {
            if (j % 2 == 0)
                departamentosLibres[j].AsignarPropietario(jugador1);
            else
                departamentosLibres[j].AsignarPropietario(jugador2);
        }
    }

    private List<string> ObtenerProvinciasPorPais(string pais)
    {
        switch (pais.ToLower())
        {
            case "Venezuela":
                return provinciasVenezuela;
            case "Nueva Granada":
                return provinciasNuevaGranada;
            case "Ecuador":
                return provinciasEcuador;
            default:
                return new List<string>();
        }
    }
}
