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
        MisionEntrega mision = GestorMisionesEntrega.Instance.ObtenerMisionActiva();

        if (mision == null || mision.itemData != item.data)
        {
            int pagoPenal = PenalizaciónPorMalaEntrega(item);
            DineroJugador.Instance.SumarDinero(pagoPenal);
            Destroy(item.gameObject);
            Debug.Log($"Entrega incorrecta: {item.data.itemNombre} | Penalización: {pagoPenal}");
        }

        int pagoFinal = CalcularPago(item);

        DineroJugador.Instance.SumarDinero(pagoFinal);

        GestorMisionesEntrega.Instance.CompletarMision();

        Debug.Log($"Entrega completada: {item.data.itemNombre} | Pago: {pagoFinal}");

        Destroy(item.gameObject);
    }


    private int CalcularPago(ItemWorld item)
    {
        float estado = item.data.Estado;
        float factor = Mathf.Clamp01(estado / 100f);

        return Mathf.RoundToInt(item.data.Precio * factor);
    }
    private int PenalizaciónPorMalaEntrega(ItemWorld item)
    {
        SistemaReputacion.Instance.RegistrarMalaReputacion();
        float penalizacion = item.data.Precio * 0.5f;
        return -Mathf.RoundToInt(penalizacion);
    }
}
