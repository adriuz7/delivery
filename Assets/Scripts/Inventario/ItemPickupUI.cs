using UnityEngine;
using TMPro;

public class ItemPickupUI : MonoBehaviour
{
    public static ItemPickupUI Instance;

    public GameObject panel;
    public TextMeshProUGUI infoText;

    private void Awake()
    {
        Instance = this;
        panel.SetActive(false);
    }
    
    public void Show(ItemData data)
    {
        panel.SetActive(true);
        infoText.text =
            $"Nombre: {data.itemNombre}\n" +
            $"Peso: {data.Peso}\n" +
            $"Precio: {data.Precio}\n" +
            $"Destino: {data.Destino}\n" +
            $"Estado: {data.Estado}\n" +
            $"Tipo: {data.Tipo}\n\n" +
            $"{data.descripcion}";
    }

    public void Hide()
    {
        panel.SetActive(false);
    }
}
