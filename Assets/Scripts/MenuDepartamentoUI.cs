using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static Departamento;

public class MenuDepartamentoUI : MonoBehaviour
{
    public TextMeshProUGUI nombreDepartamentoText;
    public Image dineroImage;
    public TextMeshProUGUI dineroCantidadText;

    public Image produccionImage;
    public TextMeshProUGUI produccionCantidadText;

    public List<Image> imagenesConstrucciones; // Aquí asignas las 4 imágenes fijas del panel central
    //public GameObject construccionPrefab; // Ya no se usa para instanciar

    public Button botonConstruccion;
    public Button botonProduccion;

    public GameObject menuConstruccion;
    public GameObject menuProduccion;

    private Departamento departamentoActual;

    void Start()
    {
        OcultarMenu();
        menuConstruccion.SetActive(false);
        menuProduccion.SetActive(false);

        botonConstruccion.onClick.AddListener(() =>
        {
            bool activo = menuConstruccion.activeSelf;
            menuConstruccion.SetActive(!activo);
            if (!activo) menuProduccion.SetActive(false);
        });

        botonProduccion.onClick.AddListener(() =>
        {
            bool activo = menuProduccion.activeSelf;
            menuProduccion.SetActive(!activo);
            if (!activo) menuConstruccion.SetActive(false);
        });
    }

    public void OcultarMenu()
    {
        gameObject.SetActive(false);
    }

    public void MostrarDepartamento(Departamento depto)
    {
        nombreDepartamentoText.text = depto.Nombre;
        // Mostrar el panel
        gameObject.SetActive(true);

        int produccionRecurso = CalcularProduccionRecurso(depto);
        int produccionDinero = CalcularProduccionDinero(depto);

        dineroCantidadText.text = ObtenerDineroProduccion(depto).ToString();
        produccionCantidadText.text = ObtenerProduccion(depto).ToString();
        produccionImage.sprite = ObtenerSpriteRecurso(depto.RecursoProducido);

        // Mostrar construcciones activas
        MostrarConstrucciones(depto);

        // Verificación de propiedad para botones
        Jugador jugadorActual = FindObjectOfType<SistemaDeTurnos>().ObtenerJugadorActual();
        bool esPropietario = depto.Dueno != null && depto.Dueno == jugadorActual;

        botonConstruccion.gameObject.SetActive(esPropietario);
        botonProduccion.gameObject.SetActive(esPropietario);

        // Cerrar submenús
        menuConstruccion.SetActive(false);
        menuProduccion.SetActive(false);
    }

    private int ObtenerDineroProduccion(Departamento depto)
    {
        return 1000;
    }

    private int ObtenerProduccion(Departamento depto)
    {
        return 500;
    }

    private Sprite ObtenerSpriteConstruccion(TipoEdificio tipo)
    {
        switch (tipo)
        {
            case TipoEdificio.Cuartel:
                return Resources.Load<Sprite>("Assets/Recursos/OficinaImagen.png"); // Ajusta la ruta según tus assets
            case TipoEdificio.Establo:
                return Resources.Load<Sprite>("Assets/Recursos/EstabloImagen.png");
            case TipoEdificio.Fabrica:
                return Resources.Load<Sprite>("Assets/Recursos/FabricaImagen.png");
            case TipoEdificio.Fortaleza:
                return Resources.Load<Sprite>("Assets/Recursos/FortalezaImagen.png");
            default:
                return null;
        }
    }

    private int CalcularProduccionRecurso(Departamento depto)
    {
        int baseProduccion = 500;
        if (depto.TieneFabrica) baseProduccion += 500;
        if (depto.TieneEstablo) baseProduccion += 200;
        return baseProduccion;
    }

    private int CalcularProduccionDinero(Departamento depto)
    {
        return 1000; // Fijo como en lógica de Departamento
    }

    private void MostrarConstrucciones(Departamento depto)
    {
        foreach (var img in imagenesConstrucciones)
            img.gameObject.SetActive(false);

        int i = 0;
        foreach (var edificio in depto.Edificios)
        {
            if (i >= imagenesConstrucciones.Count) break;
            imagenesConstrucciones[i].sprite = ObtenerSpriteConstruccion(edificio.Tipo);
            imagenesConstrucciones[i].gameObject.SetActive(true);
            i++;
        }
    }

    private Sprite ObtenerSpriteRecurso(TipoRecurso recurso)
    {
        string ruta = "";

        switch (recurso)
        {
            case TipoRecurso.Carbon:
                ruta = "carbon_circulo";
                break;
            case TipoRecurso.Comida:
                ruta = "maiz_circulo";
                break;
            case TipoRecurso.Dinero:
                ruta = "moneda_circulo";
                break;
            case TipoRecurso.Hierro:
                ruta = "hierro_circulo";
                break;
            case TipoRecurso.Madera:
                ruta = "madera_circulo";
                break;
        }

        return Resources.Load<Sprite>(ruta);
    }

}

