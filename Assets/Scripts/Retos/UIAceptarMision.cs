using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class UIAceptarMision : MonoBehaviour
{
    [Header("Referencias")]
    public Inventory inventory;
    public Transform jugador;

    [Header("UI")]
    public GameObject panelTablet;
    public Button botonAceptar;

    [Header("Input System")]
    public InputActionReference abrirTabletAction;

    [Header("Condiciones")]
    public float distanciaMinima = 1f;

    private bool tabletAbierta;

    private void OnEnable()
    {
        abrirTabletAction.action.Enable();
        abrirTabletAction.action.performed += ToggleTablet;
    }

    private void OnDisable()
    {
        abrirTabletAction.action.performed -= ToggleTablet;
        abrirTabletAction.action.Disable();
    }

    private void Start()
    {
        panelTablet.SetActive(false);
        botonAceptar.interactable = false;
    }

    private void Update()
    {
        if (!tabletAbierta)
            return;

        EvaluarBotonAceptar();
    }

    private void ToggleTablet(InputAction.CallbackContext ctx)
    {
        tabletAbierta = !tabletAbierta;
        panelTablet.SetActive(tabletAbierta);
    }

    private void EvaluarBotonAceptar()
    {
        if (GestorMisionesEntrega.Instance.HayMisionActiva())
        {
            botonAceptar.interactable = false;
            return;
        }

        ItemWorld item = inventory.ObtenerItemSeleccionado();
        if (item == null)
        {
            botonAceptar.interactable = false;
            return;
        }

        Transform destino =
            GestorDestinos.Instance.ObtenerDestino(item.data.Destino);

        if (destino == null)
        {
            botonAceptar.interactable = false;
            return;
        }

        float distancia = Vector3.Distance(jugador.position, destino.position);

        botonAceptar.interactable = distancia > distanciaMinima;
    }

    public void AceptarMision()
    {
        ItemWorld item = inventory.ObtenerItemSeleccionado();
        Debug.Log("item al aceptar mision: " + item);
        if (item == null)
        {
            Debug.Log("item es nulo" + item);
            return;
        }

        Transform destino =
            GestorDestinos.Instance.ObtenerDestino(item.data.Destino);
            Debug.Log("aceptarmision" + destino);

        if (destino == null)
        {
            Debug.Log("destino es nulo: " + destino);
            return;
        } 

        GestorMisionesEntrega.Instance
            .AceptarMision(item, destino, jugador);

        botonAceptar.interactable = false;
    }
}
