using System;
using UnityEngine;

public class BombScript : MonoBehaviour
{
    private GameObject gm = null;// variable privada gm para referenciar el game object con tag "GameManager"
    private float timer; //variable para controlar el tiempo
    public GameObject explote; //Game object que se instanciara cuando la bomba sea destruida
    private Collider bombCollider;
    public Color startColor = Color.blue;
    public Color middleColor = Color.yellow;
    public Color endColor = Color.red;
    public GameObject smokeEffectPrefab; // para el prefab de humo
    private GameObject smokeInstance;



    void Start()
    {
        GetComponent<MeshRenderer>().material.color = Color.green; //cambia el color del objeto a verde
        timer = 0.0f; //inicializo el contador a 0
        gm = GameObject.FindGameObjectWithTag("GameManager"); //busco el objeto con este tag
        bombCollider = GetComponent<Collider>();
        if (gm == null)//si no encuentra el objeto lo muestro por consola
        {
            Debug.Log("No hay ningun game manager con ese tag");
        }
    }

    void Update()//se llama el metodo a cada frame 
    {
        timer += Time.deltaTime;

        if (timer <= 2.0f) // Primera mitad de la transición
        {
            this.GetComponent<MeshRenderer>().material.color = Color.Lerp(startColor, middleColor, timer / 2.0f);
        }
        else if (timer <= 4.0f) // Segunda mitad de la transición
        {
            this.GetComponent<MeshRenderer>().material.color = Color.Lerp(middleColor, endColor, (timer - 2.0f) / 2.0f);
        }

        // para el efecto de humo antes que la bomba explote
        if (timer >= 3.0f && smokeInstance == null)
        {
            smokeInstance = Instantiate(smokeEffectPrefab, transform.position, Quaternion.identity);
            smokeInstance.transform.parent = this.transform; // Hago que el humo sea hijo de la bomba para que siga su posición
        }

        if (timer >= 4.0f)
        {
            DestroyBomb();
        }

        // Check for touch input
        if (Input.touchCount > 0) //devuelve el numero de toques (o dedos) que estan en la pantalla. Si es mayor a 0 representa que hay al menos un touch en la pantalla
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                Ray ray = Camera.main.ScreenPointToRay(touch.position);
                RaycastHit hit; //un ray es una estructura que representa un rayo infinito que comienza en un punto de orign (origin) y se extiende en una direccion. Se usa para detectar colisiones en un cierto camino o direccion
                if (bombCollider.Raycast(ray, out hit, float.MaxValue))
                {
                    HandleTouch();
                }
            }
        }
    }

    public void HandleTouch()
    {
        gm.GetComponent<GameManager>().AddScore();
        gm.GetComponent<GameManager>().PlayBombDesactivateSound(); // Llama al GameManager para reproducir el sonido
        Destroy(this.gameObject);
    }

    public void DestroyBomb()
    {
        // Se llama al GameManager para reproducir el sonido de la explosión
        gm.GetComponent<GameManager>().PlayExplosionSound();

        // se instancia el efecto de la explosión en la posición de la bomba
        Instantiate(explote, this.transform.position, Quaternion.identity);

        // Comunicacion con el GameManager para restar vida o puntuación
        gm.GetComponent<GameManager>().TakeDamage();

        // Destruir la bomba
        Destroy(this.gameObject);

        // Destruir el humo si existe
        if (smokeInstance != null)
        {
            // Para que el humo se destruya después de un cierto tiempo y no inmediatamente con la bomba
            Destroy(smokeInstance, 1.0f); // Destruye el humo después de 1 segundo
        }
    }


}
