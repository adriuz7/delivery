using UnityEngine;
using TMPro;

public class TooltipUI : MonoBehaviour
{
    public static TooltipUI Instance;
    public GameObject panel;
    public TextMeshProUGUI text;

    private void Awake()
    {
        Instance = this;
        panel.SetActive(false);
    }

    public void Show(ItemData item)
    {
        panel.SetActive(true);
        text.text = $"<b>{item.itemNombre}</b>\n{item.Destino}</b>\n{item.descripcion}";
    }

    public void Hide()
    {
        panel.SetActive(false);
    }
}
