using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerEntrega : MonoBehaviour
{
    public Inventory inventario;
    public InputActionReference interactuarAction;

    private IReceptorEntrega receptorActual;

    private void OnEnable()
    {
        interactuarAction.action.Enable();
    }

    private void OnDisable()
    {
        interactuarAction.action.Disable();
    }

    private void Update()
    {
        if (receptorActual == null) return;

        if (interactuarAction.action.WasPressedThisFrame())
        {
            IntentarEntrega();
        }
    }

    private void IntentarEntrega()
    {
        ItemWorld item = inventario.ObtenerItemSeleccionado();

        if (item == null)
        {
            Debug.Log("No tienes ningún objeto para entregar");
            return;
        }

        if (!receptorActual.PuedeRecibir(item))
        {
            Debug.Log("Este no es el destino correcto");
            return;
        }

        inventario.RemoverItem(item);
        inventario.LimpiarSeleccion();
        receptorActual.Recibir(item);
    }

    private void OnTriggerEnter(Collider other)
    {
        receptorActual = other.GetComponent<IReceptorEntrega>();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<IReceptorEntrega>() == receptorActual)
            receptorActual = null;
    }
}
