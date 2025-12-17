using UnityEngine;

public class DestinoEntrega : MonoBehaviour
{
    [Header("Identificador")]
    public string nombreDestino;

    private void OnEnable()
    {
        GestorDestinos.Instance.RegistrarDestino(this);
    }

    private void OnDisable()
    {
        GestorDestinos.Instance.QuitarDestino(this);
    }
}
