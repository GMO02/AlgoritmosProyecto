using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Variables para controlar el movimiento y el zoom de la c�mara
    public float moveSpeed = 5f;        // Velocidad de movimiento de la c�mara
    public float zoomSpeed = 5f;        // Velocidad de zoom
    public float minZoom = 2f;          // Zoom m�nimo (ajustado para acercarse m�s)
    public float maxZoom = 20f;         // Zoom m�ximo (ajustado para alejarse m�s)

    public SpriteRenderer delineadoSpriteRenderer;  // El SpriteRenderer del delineado
    public float zoomParaAparecerDelineado = 5f;     // Zoom en el que el delineado aparece

    // Variables para la animaci�n de la visibilidad de los nombres
    public SpriteRenderer nombreSpriteRenderer;  // El SpriteRenderer de los nombres de los departamentos
    public float zoomParaAparecerNombreMin = 5f;  // Valor de zoom m�nimo para aparecer el nombre
    public float zoomParaAparecerNombreMax = 4f;  // Valor de zoom m�ximo para aparecer el nombre
    private float targetAlpha2 = 0f;   // Objetivo de opacidad (0 = invisible, 1 = visible)
    private float currentAlpha2 = 0f;  // Opacidad actual

    private Vector3 dragOrigin;         // Punto de origen cuando el mouse es presionado

    // Variables para el origen del mapa (centro)
    private Vector3 origin;             // El origen del mapa donde debe estar el zoom m�ximo

    private float minX = -8.9f;   // L�mite m�nimo en X (izquierda)
    private float maxX = 8.9f;    // L�mite m�ximo en X (derecha)
    private float minY = -5f;   // L�mite m�nimo en Y (abajo)
    private float maxY = 5f;    // L�mite m�ximo en Y (arriba)

    // Variables para la animaci�n de la visibilidad del delineado
    private float targetAlpha = 0f;   // Objetivo de opacidad (0 = invisible, 1 = visible)
    private float currentAlpha = 0f;  // Opacidad actual
    public float animSpeed = 6f;      // Velocidad de la animaci�n de aparici�n/desaparici�n


    void Start()
    {
        // Establecer el origen del mapa, que podr�a ser el centro del mapa
        origin = new Vector3(0f, 0f, -10f); // Ajusta las coordenadas del origen de acuerdo al centro de tu mapa

        // Ajustar la c�mara al inicio para que se vea correctamente el zoom y el origen
        AjustarCamara();

        // Inicializar la opacidad del delineado (invisible al principio)
        currentAlpha = 0f;
        currentAlpha2 = 0f;
        delineadoSpriteRenderer.color = new Color(delineadoSpriteRenderer.color.r, delineadoSpriteRenderer.color.g, delineadoSpriteRenderer.color.b, currentAlpha);

        
        nombreSpriteRenderer.color = new Color(nombreSpriteRenderer.color.r, nombreSpriteRenderer.color.g, nombreSpriteRenderer.color.b, currentAlpha2);

    }

    void Update()
    {
        // Llamar a las funciones de movimiento y zoom
        MoverCamaraConMouse();
        ZoomConRuedaMouse();

        // Limitar el movimiento de la c�mara dentro de los l�mites de la cuadr�cula
        LimitarMovimientoCamara();

        // Controlar la visibilidad del delineado seg�n el zoom
        ControlarAparicionDelineado();

        // Controlar la visibilidad de los nombres seg�n el zoom
        ControlarAparicionNombre();
    }

    // Funci�n para mover la c�mara con el mouse al hacer clic y arrastrar
    void MoverCamaraConMouse()
    {
        if (Input.GetMouseButtonDown(0)) // Si se hace clic con el bot�n izquierdo del mouse
        {
            dragOrigin = Camera.main.ScreenToWorldPoint(Input.mousePosition); // Almacenar la posici�n inicial del mouse
        }

        if (Input.GetMouseButton(0)) // Si el bot�n izquierdo del mouse est� presionado
        {
            Vector3 dif = Camera.main.ScreenToWorldPoint(Input.mousePosition) - dragOrigin; // Calcular la diferencia del movimiento del mouse
            Camera.main.transform.position = Camera.main.transform.position - new Vector3(dif.x, dif.y, 0); // Mover la c�mara
        }
    }

    // Funci�n para hacer zoom con la rueda del mouse
    void ZoomConRuedaMouse()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel"); // Obtener el valor de la rueda del mouse
        if (scroll != 0f)
        {
            Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize - scroll * zoomSpeed, minZoom, maxZoom); // Modificar el tama�o ortogr�fico de la c�mara (Zoom)
        }
    }

    // Funci�n para ajustar la c�mara, centrando el origen y ajustando el zoom
    void AjustarCamara()
    {
        // Colocar la c�mara en el origen (centro del mapa)
        Camera.main.transform.position = origin;

        // Ajustar el zoom m�ximo (ver el mapa completo)
        Camera.main.orthographicSize = maxZoom; // El zoom m�ximo ser� el m�s alejado
    }

    void LimitarMovimientoCamara()
    {
        // Obtener el tama�o visible de la c�mara en funci�n del zoom actual
        float camWidth = Camera.main.orthographicSize * Screen.width / Screen.height; // Ancho visible
        float camHeight = Camera.main.orthographicSize; // Altura visible

        // Limitar la posici�n de la c�mara seg�n los l�mites de la cuadr�cula
        float camPosX = Mathf.Clamp(Camera.main.transform.position.x, minX + camWidth, maxX - camWidth);
        float camPosY = Mathf.Clamp(Camera.main.transform.position.y, minY + camHeight, maxY - camHeight);

        // Aplicar la nueva posici�n limitada a la c�mara
        Camera.main.transform.position = new Vector3(camPosX, camPosY, Camera.main.transform.position.z);
    }

    // Funci�n para controlar la aparici�n del delineado basado en el zoom
    void ControlarAparicionDelineado()
    {
        // Verificar si el zoom es mayor o igual al valor establecido para aparecer el delineado
        if (Camera.main.orthographicSize <= zoomParaAparecerDelineado)
        {
            // Gradualmente hacer que el delineado aparezca (aumentando su opacidad)
            targetAlpha = 1f;
        }
        else
        {
            // Gradualmente hacer que el delineado desaparezca (reduciendo su opacidad)
            targetAlpha = 0f;
        }

        // Interpolar entre la opacidad actual y la opacidad objetivo
        currentAlpha = Mathf.Lerp(currentAlpha, targetAlpha, Time.deltaTime * animSpeed);


        // Aplicar la nueva opacidad al SpriteRenderer
        Color color = delineadoSpriteRenderer.color;
        delineadoSpriteRenderer.color = new Color(color.r, color.g, color.b, currentAlpha);
    }

    // Funci�n para controlar la aparici�n del nombre basado en el zoom
    void ControlarAparicionNombre()
    {
        // Verificar si el zoom est� dentro del rango para aparecer el nombre
        if (Camera.main.orthographicSize <= zoomParaAparecerNombreMin && Camera.main.orthographicSize >= zoomParaAparecerNombreMax)
        {
            // Gradualmente hacer que el nombre aparezca (aumentando su opacidad)
            targetAlpha2 = 1f;
        }
        else
        {
            // Gradualmente hacer que el nombre desaparezca (reduciendo su opacidad)
            targetAlpha2 = 0f;
        }

        // Interpolar entre la opacidad actual y la opacidad objetivo
        currentAlpha2 = Mathf.Lerp(currentAlpha2, targetAlpha2, Time.deltaTime * animSpeed);

        // Aplicar la nueva opacidad al SpriteRenderer del nombre
        Color color = nombreSpriteRenderer.color;
        nombreSpriteRenderer.color = new Color(color.r, color.g, color.b, currentAlpha2);
    }
}
