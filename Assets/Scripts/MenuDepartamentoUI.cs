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

    public List<Image> imagenesConstrucciones; // Aqu� asignas las 4 im�genes fijas del panel central
    //public GameObject construccionPrefab; // Ya no se usa para instanciar

    public Button botonConstruccion;
    public Button botonProduccion;

    public GameObject menuConstruccion;
    public GameObject menuProduccion;

    private Departamento departamentoActual;

    public TextMeshProUGUI textoInfanteria;
    public TextMeshProUGUI textoCaballeria;
    public TextMeshProUGUI textoArtilleria;

    public Button botonAtacar;
    public Button botonMover;


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

        botonConstruccion.gameObject.SetActive(true);
        botonProduccion.gameObject.SetActive(true);

        // Cerrar submen�s
        menuConstruccion.SetActive(false);
        menuProduccion.SetActive(false);

        var conteos = depto.Ejer.ObtenerConteoTropas();
        textoInfanteria.text = $"Infanter�a: {conteos.infanteria}";
        textoCaballeria.text = $"Caballer�a: {conteos.caballeria}";
        textoArtilleria.text = $"Artiller�a: {conteos.artilleria}";

        MostrarBotonSegunDepartamento(depto);


        depto.ActualizarImagenTropa();

    }

    public void MostrarBotonSegunDepartamento(Departamento depto)
    {
        // Por defecto ocultar ambos
        botonAtacar.gameObject.SetActive(false);
        botonMover.gameObject.SetActive(false);

        // Seg�n el nombre del departamento, mostrar un bot�n espec�fico
        switch (depto.Nombre)
        {
            case "Cauca":
                botonMover.gameObject.SetActive(true);
                break;
            case "Cundinamarca":
                botonMover.gameObject.SetActive(true);
                break;
            case "Boyac�":
                botonMover.gameObject.SetActive(true);
                break;
            case "Magdalena":
                botonMover.gameObject.SetActive(true);
                break;
            case "Panam�":
                botonMover.gameObject.SetActive(true);
                break;
            case "Apure":
                botonAtacar.gameObject.SetActive(true);
                break;
            case "Zulia":
                botonAtacar.gameObject.SetActive(true);
                break;
            case "Orinoco":
                botonAtacar.gameObject.SetActive(true);
                break;
            case "Ecuador":
                botonAtacar.gameObject.SetActive(true);
                break;
            case "Asuay":
                botonAtacar.gameObject.SetActive(true);
                break;
            default:
                // Ning�n bot�n visible
                break;
        }
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
                return Resources.Load<Sprite>("Assets/Recursos/OficinaImagen.png"); // Ajusta la ruta seg�n tus assets
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
        return 1000; // Fijo como en l�gica de Departamento
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

