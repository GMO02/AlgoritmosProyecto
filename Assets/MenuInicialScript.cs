using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuInicialScript : MonoBehaviour
{
    public GameObject PanelMenuInicial;   // Panel con botones Jugar y Salir
    public GameObject SeleccionPaisPanel;   // Panel de selección de país

    void Start()
    {
        PanelMenuInicial.SetActive(true);     // Mostrar menú principal
        SeleccionPaisPanel.SetActive(false);    // Ocultar selección país
    }

    public void Jugar()
    {
        PanelMenuInicial.SetActive(false);    // Oculta el menú principal
        SeleccionPaisPanel.SetActive(true);
    }

    public void Salir()
    {
        Debug.Log("Saliendo...");
        Application.Quit();
    }

}
