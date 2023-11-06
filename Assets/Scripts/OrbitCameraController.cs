using UnityEngine;

public class OrbitCameraController : MonoBehaviour
{
    public Transform target; // El objeto alrededor del cual la cámara orbitará

    public float distance = 10f; // Distancia inicial entre la cámara y el objetivo
    public float maxDistance = 25f; // Distancia máxima permitida desde el objetivo
    public float minDistance = 5f; // Distancia mínima permitida desde el objetivo

    public float xSpeed = 0.2f; // Velocidad de rotación horizontal
    public float ySpeed = 0.2f; // Velocidad de rotación vertical
    public float zoomSpeed = 5f; // Velocidad de zoom (acercar/alejar)
    public float smoothingZoom = 0.1f; // Suavizado del zoom

    public bool limitY = true; // Limitar la rotación vertical
    public float yMinLimit = -60f; // Límite inferior de rotación vertical
    public float yMaxLimit = 60f; // Límite superior de rotación vertical
    public float yLimitOffset = 0f; // Desplazamiento del límite vertical

    public bool limitX = false; // Limitar la rotación horizontal
    public float xMinLimit = -60f; // Límite izquierdo de rotación horizontal
    public float xMaxLimit = 60f; // Límite derecho de rotación horizontal
    public float xLimitOffset = 0f; // Desplazamiento del límite horizontal

    private float targetDistance = 10f; // Distancia objetivo hacia la que se hará el zoom
    private float x = 0f; // Posición horizontal actual de la cámara
    private float y = 0f; // Posición vertical actual de la cámara
    private float xVelocity = 0f; // Velocidad horizontal actual
    private float yVelocity = 0f; // Velocidad vertical actual
    private Vector3 position; // Posición actual de la cámara
    private float pinchDist = 0f; // Distancia entre dos dedos al pellizcar la pantalla

    private Transform _transform; // Referencia al transform de esta cámara

    private float damping = 5.0f; // Amortiguamiento para suavizar la rotación

    void Start()
    {
        _transform = transform; // Inicializar la referencia al transform de la cámara

        // Si el objeto tiene un Rigidbody, congelamos su rotación
        if (GetComponent<Rigidbody>() != null)
            GetComponent<Rigidbody>().freezeRotation = true;

        // Valores iniciales para x e y
        x = 30f;
        y = 45f;
        // Establecer la distancia inicial
        targetDistance = distance;
        // Calcular la posición inicial de la cámara en base a la rotación y la distancia
        position = _transform.rotation * new Vector3(0.0f, 0.0f, -distance) + target.position;
    }

    void Update()
    {
        if (target != null)
        {
            // Si hay un solo toque en la pantalla
            if (Input.touchCount == 1)
            {
                // Calcula la velocidad de la rotación en función del desplazamiento del dedo en la pantalla
                xVelocity = Mathf.Lerp(xVelocity, Input.GetTouch(0).deltaPosition.x * xSpeed, Time.deltaTime * damping);
                yVelocity = Mathf.Lerp(yVelocity, -Input.GetTouch(0).deltaPosition.y * ySpeed, Time.deltaTime * damping);
            }
            else
            {
                // Si no hay toques, las velocidades son 0
                xVelocity = 0f;
                yVelocity = 0f;
            }

            // Si está habilitado, limitamos la rotación horizontal
            if (limitX)
            {
                x = Mathf.Clamp(x + xVelocity, xMinLimit + xLimitOffset, xMaxLimit + xLimitOffset);
            }
            else
            {
                x += xVelocity;
            }

            // Si está habilitado, limitamos la rotación vertical
            if (limitY)
            {
                y = Mathf.Clamp(y + yVelocity, yMinLimit + yLimitOffset, yMaxLimit + yLimitOffset);
            }
            else
            {
                y += yVelocity;
            }

            // Si hay dos toques en la pantalla (gesto de pellizco)
            if (Input.touchCount == 2)
            {
                // Calcula la distancia entre los dos toques
                float newPinchDist = Vector2.Distance(Input.GetTouch(0).position, Input.GetTouch(1).position);
                // Si había una distancia previa, calcula el cambio y ajusta la distancia objetivo de la cámara
                if (pinchDist != 0f)
                {
                    targetDistance += ((pinchDist - newPinchDist) * 0.005f) * zoomSpeed;
                }
                pinchDist = newPinchDist;

                // Limita la distancia objetivo entre minDistance y maxDistance
                targetDistance = Mathf.Clamp(targetDistance, minDistance, maxDistance);
            }
            else
            {
                // Si no hay pellizco, resetea la distancia
                pinchDist = 0f;
            }

            // Suaviza la distancia hacia la distancia objetivo
            distance = Mathf.Lerp(distance, targetDistance, smoothingZoom);
            // Calcula la posición de la cámara en base a la rotación y la distancia
            position = _transform.rotation * new Vector3(0.0f, 0.0f, -distance) + target.position;
            // Establece la posición de la cámara
            _transform.position = position;

            // Establece la rotación de la cámara
            _transform.rotation = Quaternion.Euler(y, x, 0);
        }
        else
        {
            // Advierte si no se ha establecido un objetivo
            Debug.LogWarning("Touch Orbit Cam - No Target Given");
        }
    }
}
