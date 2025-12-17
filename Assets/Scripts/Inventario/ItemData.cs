using UnityEngine;

public enum ItemType
{
    Juguete,
    Electrodomestico,
    Cristaleria,
    Valioso,
    Otro
}

[CreateAssetMenu(menuName = "Inventory/Item")]
public class ItemData : ScriptableObject
{
    public string itemNombre;
    public float Peso;
    public int Precio;
    public string Destino;
    public float Estado = 100f;
    public ItemType Tipo;
    [TextArea]
    public string descripcion;
    public Sprite icono;
    public GameObject worldPrefab;
}
