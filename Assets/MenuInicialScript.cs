using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuInicialScript : MonoBehaviour
{
    public GameObject PanelMenuInicial;   // Panel con botones Jugar y Salir
    public GameObject SeleccionPaisPanel;   // Panel de selecci�n de pa�s

    void Start()
    {
        PanelMenuInicial.SetActive(true);     // Mostrar men� principal
        SeleccionPaisPanel.SetActive(false);    // Ocultar selecci�n pa�s
    }

    public void Jugar()
    {
        PanelMenuInicial.SetActive(false);    // Oculta el men� principal
        SeleccionPaisPanel.SetActive(true);
    }

    public void Salir()
    {
        Debug.Log("Saliendo...");
        Application.Quit();
    }

}
