using UnityEngine;

public class GestorMisionesEntrega : MonoBehaviour
{
    public static GestorMisionesEntrega Instance;

    private MisionEntrega misionActiva;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            misionActiva = null;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (misionActiva == null || misionActiva.completada || misionActiva.fallida)
            return;

        misionActiva.tiempoRestante -= Time.deltaTime;

        if (misionActiva.tiempoRestante <= 0f)
        {
            FallarMision();
        }
    }

    public void AceptarMision(ItemWorld itemWorld, Transform destino, Transform jugador)
    {
        if (misionActiva != null)
        {
            Debug.LogWarning("Ya hay una misión activa.");
            return;
        }

        ItemData item = itemWorld.data;

        float tiempoCalculado = CalcularTiempoEntrega(jugador.position, destino.position, item);

        misionActiva = new MisionEntrega(item, destino, tiempoCalculado);

        Debug.Log($"MISIÓN INICIADA: {item.itemNombre} | Tiempo: {tiempoCalculado:F1}s");
    }

    private float CalcularTiempoEntrega(Vector3 inicio, Vector3 fin, ItemData item)
    {
        float distancia = Vector3.Distance(inicio, fin);

        float tiempoBase = distancia / 6f;

        tiempoBase += item.Peso * 0.3f;

        return Mathf.Clamp(tiempoBase, 10f, 600f);
    }

    public bool HayMisionActiva()
    {
        return misionActiva != null;
    }

    public MisionEntrega ObtenerMisionActiva()
    {
        return misionActiva;
    }

    public void CompletarMision()
    {
        if (misionActiva == null) return;

        misionActiva.completada = true;
        misionActiva = null;
    }

    private void FallarMision()
    {
        Debug.Log("MISIÓN FALLIDA");

        SistemaReputacion.Instance.RegistrarMalaReputacion();
        misionActiva = null;
    }
}
