using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SlotUI : MonoBehaviour,
    IBeginDragHandler,
    IDragHandler,
    IEndDragHandler,
    IPointerEnterHandler,
    IPointerExitHandler,
    IPointerClickHandler
{
    public Image icon;
    public Image fondoSeleccion;

    private Inventory inventory;
    private int index;
    private ItemData currentItem;

    private Transform originalParent;
    private Canvas canvas;

    public void Setup(int slotIndex, Inventory inv)
    {
        index = slotIndex;
        inventory = inv;
        canvas = GetComponentInParent<Canvas>();
    }

    public void SetItem(ItemData item)
    {
        currentItem = item;
        icon.enabled = item != null;
        icon.sprite = item != null ? item.icono : null;
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (currentItem == null) return;

        originalParent = icon.transform.parent;
        icon.transform.SetParent(canvas.transform);
        icon.raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (currentItem == null) return;

        icon.transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (inventory.SlotSeleccionado != index) return;
        if (currentItem == null) return;

        icon.raycastTarget = true;

        if (!RectTransformUtility.RectangleContainsScreenPoint(
            originalParent.GetComponent<RectTransform>(),
            eventData.position,
            canvas.worldCamera))
        {
            inventory.DropItem(index);
        }

        icon.transform.SetParent(originalParent);
        icon.transform.localPosition = Vector3.zero;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (currentItem != null)
            TooltipUI.Instance.Show(currentItem);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        TooltipUI.Instance.Hide();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (currentItem == null) return;

        inventory.SeleccionarSlot(index);
    }

    public void ActualizarSeleccion()
    {
        bool seleccionado = inventory.SlotSeleccionado == index;
        fondoSeleccion.enabled = seleccionado;
    }


}
