using UnityEngine;
using UnityEngine.InputSystem;

public class vehiculoController : MonoBehaviour
{
    public float fuerzaMotor = 1500f;
    public float fuerzaFrenado = 3000f;
    public float maxAnguloDireccion = 30f;

    [Header("Colisionadores de Ruedas")]
    public WheelCollider ruedaDelanteraIzquierda;
    public WheelCollider ruedaDelanteraDerecha;
    public WheelCollider ruedaTraseraIzquierda;
    public WheelCollider ruedaTraseraDerecha;

    [Header("Modelos de Ruedas")]
    public Transform modeloRuedaDelanteraIzquierda;
    public Transform modeloRuedaDelanteraDerecha;
    public Transform modeloRuedaTraseraIzquierda;
    public Transform modeloRuedaTraseraDerecha;

    private float inputHorizontal;
    private float inputVertical;
    private float actualDireccionAngulo;
    private float fuerzaFrenadoActual;
    private bool frenando;

    [Header("Referencias de Input")]
    public InputActionReference movimientoAction;
    public InputActionReference brakeAction;
    //public InputActionReference salirAction;


    private void OnEnable()
    {
        movimientoAction.action.Enable();
        brakeAction.action.Enable();
        //salirAction.action.Enable();
    }

    private void OnDisable()
    {
        movimientoAction.action.Disable();
        brakeAction.action.Disable();
        // salirAction.action.Disable();
    }

    private void Update() {
        GetInput();
        ManejoMotor();
        AplicarFrenado();
        ManejoDireccion();
        ActualizarRuedas();        
    }
    private void GetInput()
    {
        Vector2 move = movimientoAction.action.ReadValue<Vector2>();
        inputHorizontal = move.x;
        inputVertical = move.y;

        frenando = brakeAction.action.IsPressed();
    }
    private void ManejoMotor()
    {
        ruedaTraseraIzquierda.motorTorque = inputVertical * fuerzaMotor;
        ruedaTraseraDerecha.motorTorque = inputVertical * fuerzaMotor;

        if (frenando)
        {
            fuerzaFrenadoActual = fuerzaFrenado;
        }
        else
        {
            fuerzaFrenadoActual = 0f;
        }
    }

    private void AplicarFrenado()
    {
        ruedaTraseraIzquierda.brakeTorque = fuerzaFrenadoActual;
        ruedaTraseraDerecha.brakeTorque = fuerzaFrenadoActual;
        ruedaDelanteraIzquierda.brakeTorque = fuerzaFrenadoActual;
        ruedaDelanteraDerecha.brakeTorque = fuerzaFrenadoActual;
    }  

    private void ManejoDireccion()
    {
        actualDireccionAngulo = maxAnguloDireccion * inputHorizontal;
        ruedaDelanteraIzquierda.steerAngle = actualDireccionAngulo;
        ruedaDelanteraDerecha.steerAngle = actualDireccionAngulo;
    }

    private void ActualizarRuedaUnada(WheelCollider collider, Transform modeloRueda)
    {
        Vector3 posicion;
        Quaternion rotacion;
        collider.GetWorldPose(out posicion, out rotacion);
        modeloRueda.position = posicion;
        // modeloRueda.rotation = rotacion;
        modeloRueda.rotation = rotacion * Quaternion.Euler(0, 0, 0);
    }

    private void ActualizarRuedas()
    {
        ActualizarRuedaUnada(ruedaDelanteraIzquierda, modeloRuedaDelanteraIzquierda);
        ActualizarRuedaUnada(ruedaDelanteraDerecha, modeloRuedaDelanteraDerecha);
        ActualizarRuedaUnada(ruedaTraseraIzquierda, modeloRuedaTraseraIzquierda);
        ActualizarRuedaUnada(ruedaTraseraDerecha, modeloRuedaTraseraDerecha);
    }
    
}
