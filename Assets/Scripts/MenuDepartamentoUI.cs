using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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


    void Start()
    {
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

        dineroCantidadText.text = ObtenerDineroProduccion(depto).ToString();
        produccionCantidadText.text = ObtenerProduccion(depto).ToString();

        // Primero ocultamos todas las imágenes de construcciones
        foreach (var img in imagenesConstrucciones)
        {
            img.gameObject.SetActive(false);
        }

        // Obtenemos las construcciones hechas en el departamento (debes implementar este método en Departamento)
        /*
        List<Construccion> construcciones = depto.ObtenerConstrucciones();

        // Activamos y asignamos sprite a las imágenes para cada construcción hecha, máximo 4
        int count = Mathf.Min(construcciones.Count, imagenesConstrucciones.Count);
        for (int i = 0; i < count; i++)
        {
            imagenesConstrucciones[i].sprite = construcciones[i].Sprite;
            imagenesConstrucciones[i].gameObject.SetActive(true);
        }
        */
        // Cerrar submenús
        menuConstruccion.SetActive(false);
        menuProduccion.SetActive(false);

        // Mostrar el panel
        gameObject.SetActive(true);
    }

    private int ObtenerDineroProduccion(Departamento depto)
    {
        return 100;
    }

    private int ObtenerProduccion(Departamento depto)
    {
        return 50;
    }
}

