using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class EntrarAuto : MonoBehaviour
{
    [Header("Camara")]
    public GameObject carCamera;

    [Header("Posición de Salida")]
    public Transform posicionDeSalida;

    [Header("Input System")]
    public InputActionReference entrarSalirAction;

    [Header("Contiene Conductor?")]
    [HideInInspector] public bool contieneConductor = false;
    [HideInInspector]
    [Tooltip("Objeto/Jugador que se activará o desactivará al salir y entrar del vehiculo")]
    public GameObject conductor;

    private vehiculoController carController;
    private Rigidbody rigid;
    private GameObject player;

    private bool insideCar = false;
    private bool playerInTrigger = false;

    private void Start()
    {
        insideCar = false;
        carController = GetComponent<vehiculoController>();
        rigid = GetComponent<Rigidbody>();

        if (carController != null)
            carController.enabled = false;

        if (contieneConductor && conductor != null)
            conductor.SetActive(false);
    }

    private void OnEnable()
    {
        if (entrarSalirAction != null)
            entrarSalirAction.action.Enable();
    }

    private void OnDisable()
    {
        if (entrarSalirAction != null)
            entrarSalirAction.action.Disable();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInTrigger = true;
            player = other.gameObject;
        }
        else if (other.transform.root.CompareTag("Player"))
        {
            playerInTrigger = true;
            player = other.transform.root.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInTrigger = false;
        }
    }

    private void Update()
    {
        if (entrarSalirAction == null)
            return;

        if (!entrarSalirAction.action.WasPressedThisFrame())
            return;

        // --- ENTRAR AL AUTO ---
        if (playerInTrigger && !insideCar)
        {
            EntrarAlAuto();
        }
        // --- SALIR DEL AUTO ---
        else if (insideCar)
        {
            SalirDelAuto();
        }
    }

    private void EntrarAlAuto()
    {
        if (player == null || carController == null || posicionDeSalida == null)
            return;

        carController.enabled = true;

        if (contieneConductor && conductor != null)
            conductor.SetActive(true);

        player.SetActive(false);
        player.transform.SetParent(posicionDeSalida);
        player.transform.position = posicionDeSalida.position;

        if (carCamera != null)
            carCamera.SetActive(true);

        StartCoroutine(EnableInsideCar(true));
    }



    private void SalirDelAuto()
    {
       if (carController != null)
           carController.enabled = false;

       if (contieneConductor && conductor != null)
           conductor.SetActive(false);

       player.SetActive(true);
       player.transform.position = posicionDeSalida.position;
       player.transform.SetParent(null);

       if (carCamera != null)
           carCamera.SetActive(false);

       StartCoroutine(EnableInsideCar(false));
    }


    private IEnumerator EnableInsideCar(bool option)
    {
        yield return new WaitForSeconds(0.2f);
        insideCar = option;
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(EntrarAuto))]
    public class EditorEntrarAuto : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            EntrarAuto script = (EntrarAuto)target;

            EditorGUILayout.LabelField("Opciones de Conductor", EditorStyles.boldLabel);
            script.contieneConductor = EditorGUILayout.Toggle("Hay un conductor?", script.contieneConductor);

            if (script.contieneConductor)
            {
                script.conductor = EditorGUILayout.ObjectField(
                    "Conductor", script.conductor, typeof(GameObject), true) as GameObject;
            }
            else
            {
                script.conductor = null;
            }

            EditorGUILayout.Space(6);
            EditorGUILayout.LabelField("Info", EditorStyles.boldLabel);

            GUIStyle style = new GUIStyle(EditorStyles.boldLabel);
            style.normal.textColor = script.insideCar ? Color.green : Color.red;

            EditorGUILayout.LabelField(
                script.insideCar ? "Dentro del Vehículo" : "Fuera del Vehículo",
                style
            );
        }
    }
#endif
}
