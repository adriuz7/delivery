using UnityEngine;

public class ReceptorEntrega : MonoBehaviour, IReceptorEntrega
{
    [Header("Destino")]
    [SerializeField] private string nombreDestino;

    public string NombreDestino => nombreDestino;

    public bool PuedeRecibir(ItemWorld item)
    {
        return item.data.Destino == nombreDestino;
    }

    public void Recibir(ItemWorld item)
    {
        int pagoFinal = CalcularPago(item);

        DineroJugador.Instance.SumarDinero(pagoFinal);

        Debug.Log($"Entrega completada: {item.data.itemNombre} | Pago: {pagoFinal}");

        Destroy(item.gameObject);
    }

    private int CalcularPago(ItemWorld item)
    {
        float estado = item.data.Estado;
        float factor = Mathf.Clamp01(estado / 100f);

        return Mathf.RoundToInt(item.data.Precio * factor);
    }
}
