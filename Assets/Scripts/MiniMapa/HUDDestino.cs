using UnityEngine;
using TMPro;

public class HUDDestino : MonoBehaviour
{
    public Inventory inventory;
    public TextMeshProUGUI textoDestino;

    private void OnEnable()
    {
        inventory.OnInventoryChanged += ActualizarHUD;
    }

    private void OnDisable()
    {
        inventory.OnInventoryChanged -= ActualizarHUD;
    }

    private void ActualizarHUD()
    {
        ItemWorld item = inventory.ObtenerItemSeleccionado();

        if (item == null)
        {
            textoDestino.text = "Destino: —";
            return;
        }

        textoDestino.text = $"Destino: {item.data.Destino}";
    }
}
