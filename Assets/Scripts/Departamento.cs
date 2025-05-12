using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Departamento : MonoBehaviour
{
    public string Nombre;
    public SpriteRenderer spriteRenderer;  // Referencia al SpriteRenderer que representa el departamento

    // Lista de departamentos adyacentes (territorios a los que puede atacar)
    public List<Departamento> Adyacentes;

    public AudioClip clickSound; // Sonido para cuando se haga clic en el departamento

    private Color originalColor; // Color original del departamento
    private AudioSource audioSource; // Componente de AudioSource para reproducir el sonido

    private static Departamento departamentoSeleccionado = null;  // Mantener solo un departamento seleccionado

    private Color seleccionadoColor = new Color(0.643f, 0.631f, 0.565f); // Color #A4A190 (RGB Normalizado)

    // Método para agregar un departamento adyacente
    public void AgregarAdyacente(Departamento adyacente)
    {
        if (!Adyacentes.Contains(adyacente))
        {
            Adyacentes.Add(adyacente);
        }
    }


    void Start()
    {
        // Asegúrate de que el spriteRenderer esté inicializado correctamente si se necesita
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        // Asegúrate de que el AudioSource esté asignado
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();  // Añadir un componente AudioSource si no existe
        }

        // Guardar el color original del departamento
        originalColor = spriteRenderer.color;
    }

    // Método para manejar el clic sobre el departamento
    void OnMouseDown()
    {
        // Si ya hay un departamento seleccionado, desmarcarlo
        if (departamentoSeleccionado != null && departamentoSeleccionado != this)
        {
            departamentoSeleccionado.DesmarcarDepartamento();  // Desmarcar el departamento previamente seleccionado
        }

        // Si el departamento actual no está seleccionado, marcarlo como seleccionado
        if (departamentoSeleccionado != this)
        {
            SeleccionarDepartamento();

            // Reproducir el sonido solo cuando el departamento es seleccionado
            if (audioSource != null && clickSound != null)
            {
                audioSource.PlayOneShot(clickSound);  // Reproducir el sonido de clic
            }
        }
        else
        {
            // Si el departamento ya está seleccionado, desmarcarlo
            DesmarcarDepartamento();
        }
    }

    // Método para seleccionar un departamento
    private void SeleccionarDepartamento()
    {
        // Cambiar el color a #A4A190 (más oscuro)
        spriteRenderer.color = seleccionadoColor;

        // Marcar este departamento como seleccionado
        departamentoSeleccionado = this;
    }

    // Método para desmarcar este departamento
    private void DesmarcarDepartamento()
    {
        // Restaurar el color original del departamento desmarcado
        spriteRenderer.color = originalColor;

        // Desmarcar el departamento
        departamentoSeleccionado = null;
    }

    // Detectar si el clic es fuera del mapa (fondo)
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // Hacer un Raycast en la escena
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            // Si no se ha tocado ningún departamento, desmarcar
            if (hit.collider == null && departamentoSeleccionado != null)
            {
                departamentoSeleccionado.DesmarcarDepartamento();
                departamentoSeleccionado = null;  // Resetear la selección
            }
        }
    }
}
