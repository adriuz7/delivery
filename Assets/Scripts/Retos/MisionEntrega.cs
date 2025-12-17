using UnityEngine;

[System.Serializable]
public class MisionEntrega
{
    public ItemData itemData;
    public Transform destino;
    public float tiempoRestante;
    public bool completada;
    public bool fallida;

    public MisionEntrega(ItemData item, Transform dest, float tiempo)
    {
        itemData = item;
        destino = dest;
        tiempoRestante = tiempo;
        completada = false;
        fallida = false;
    }
}
