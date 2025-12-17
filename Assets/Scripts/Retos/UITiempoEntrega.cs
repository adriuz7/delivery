using UnityEngine;
using TMPro;

public class UITiempoEntrega : MonoBehaviour
{
    public TextMeshProUGUI textoTiempo;

    private void Update()
    {
        var mision = GestorMisionesEntrega.Instance.ObtenerMisionActiva();

        if (mision == null)
        {
            textoTiempo.text = "";
            return;
        }

        float t = Mathf.Max(0, mision.tiempoRestante);
        textoTiempo.text = $"Tiempo restante: {t:F0}s";
    }
}
