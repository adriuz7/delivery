using UnityEngine;

public class RadarDestino : MonoBehaviour
{
    [Header("Referencias")]
    public Inventory inventory;
    public Transform jugador;

    public RectTransform radarArea;
    public RectTransform puntoDestino;

    [Header("Configuración")]
    public float radioMundo = 50f;
    public float radioRadar = 80f;
    private void Update()
    {
        ItemWorld item = inventory.ObtenerItemSeleccionado();

        if (item == null)
        {
            puntoDestino.gameObject.SetActive(false);
            return;
        }

        Transform destino = GestorDestinos.Instance
            .ObtenerDestino(item.data.Destino);

        if (destino == null)
        {
            puntoDestino.gameObject.SetActive(false);
            return;
        }

        puntoDestino.gameObject.SetActive(true);
        ActualizarRadar(destino.position);
    }

    private void ActualizarRadar(Vector3 destinoPos)
    {
        Vector3 delta = destinoPos - jugador.position;

        Vector2 plano = new Vector2(delta.x, delta.z);

        float distancia = plano.magnitude;
        Vector2 direccion = plano.normalized;

        float factor = Mathf.Clamp01(distancia / radioMundo);

        Vector2 posicionRadar = direccion * factor * radioRadar;

        puntoDestino.anchoredPosition = posicionRadar;
    }
}
