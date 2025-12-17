using System.Collections.Generic;
using UnityEngine;

public class GestorDestinos : MonoBehaviour
{
    public static GestorDestinos Instance;

    private Dictionary<string, Transform> destinos =
        new Dictionary<string, Transform>();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void RegistrarDestino(DestinoEntrega destino)
    {
        if (!destinos.ContainsKey(destino.nombreDestino))
        {
            destinos.Add(destino.nombreDestino, destino.transform);
        }
    }

    public void QuitarDestino(DestinoEntrega destino)
    {
        if (destinos.ContainsKey(destino.nombreDestino))
        {
            destinos.Remove(destino.nombreDestino);
        }
    }

    public Transform ObtenerDestino(string nombre)
    {
        destinos.TryGetValue(nombre, out Transform t);
        return t;
    }
}
