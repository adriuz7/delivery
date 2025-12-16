using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))] // Requerimos el Animator
public class ControladorPersonaje : MonoBehaviour
{
    [Header("Configuración de Movimiento")]
    public float velocidadCaminar = 5f;
    public float velocidadCorrer = 10f;
    public float alturaSalto = 1.5f;
    public float gravedad = -9.81f;

    [Header("Configuración de Animación")]
    public string parametroX = "posX";
    public string parametroZ = "posZ";

    public float valorAnimacionCaminar = 0.2f; 
    public float valorAnimacionCorrer = 1.0f; 

    [Header("Referencias de Input")]
    public InputActionReference movimientoAction;
    public InputActionReference correrAction;
    public InputActionReference saltarAction;

    private CharacterController controller;
    private Animator animator;
    private Vector3 velocity;
    private bool isGrounded;

    private int animIDPosX;
    private int animIDPosZ;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        
        animIDPosX = Animator.StringToHash(parametroX);
        animIDPosZ = Animator.StringToHash(parametroZ);
    }

    private void OnEnable()
    {
        movimientoAction.action.Enable();
        correrAction.action.Enable();
        saltarAction.action.Enable();
    }

    private void OnDisable()
    {
        movimientoAction.action.Disable();
        correrAction.action.Disable();
        saltarAction.action.Disable();
    }

    private void Update()
    {
        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        // --- LECTURA DE INPUTS ---
        Vector2 inputMovimiento = movimientoAction.action.ReadValue<Vector2>();
        bool estaCorriendo = correrAction.action.IsPressed();
        bool saltoPresionado = saltarAction.action.WasPressedThisFrame();

        // --- MOVIMIENTO FÍSICO ---
        float velocidadActual = estaCorriendo ? velocidadCorrer : velocidadCaminar;
        Vector3 move = transform.right * inputMovimiento.x + transform.forward * inputMovimiento.y;
        controller.Move(move * velocidadActual * Time.deltaTime);

        // --- ANIMACIÓN ---
        ActualizarAnimacion(inputMovimiento, estaCorriendo);

        // --- SALTO ---
        if (saltoPresionado && isGrounded)
        {
            velocity.y = Mathf.Sqrt(alturaSalto * -2f * gravedad);
            // Aquí podrías disparar un trigger de salto si lo agregas al animator
            // animator.SetTrigger("Jump"); 
        }

        // --- GRAVEDAD ---
        velocity.y += gravedad * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    void ActualizarAnimacion(Vector2 input, bool corriendo)
    {
        // Determinamos hasta qué valor debe llegar la animación
        // Si no hay input, el objetivo es 0
        float targetValue = 0f;

        if (input.magnitude > 0)
        {
            // Si corre, vamos a 1.0, si camina vamos a 0.2
            targetValue = corriendo ? valorAnimacionCorrer : valorAnimacionCaminar;
        }

        // Calculamos los valores objetivo para X y Z
        // Multiplicamos el input normalizado por el valor objetivo (0.2 o 1.0)
        float currentX = input.x * targetValue;
        float currentZ = input.y * targetValue;

        // Usamos SetFloat con "dampTime" (0.1f) para que la transición sea suave y no brusca
        animator.SetFloat(animIDPosX, currentX, 0.1f, Time.deltaTime);
        animator.SetFloat(animIDPosZ, currentZ, 0.1f, Time.deltaTime);
    }
}