using UnityEngine;

public class SistemaReputacion : MonoBehaviour
{
    public static SistemaReputacion Instance;

    [Header("Configuración")]
    public int maxMalaReputacion = 5;

    [Header("Estado")]
    [SerializeField] private int malaReputacionActual = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void RegistrarMalaReputacion()
    {
        malaReputacionActual++;

        Debug.Log($"Mala reputación: {malaReputacionActual}/{maxMalaReputacion}");

        if (malaReputacionActual >= maxMalaReputacion)
        {
            FinDelJuego();
        }
    }

    private void FinDelJuego()
    {
        Debug.Log("FIN DEL JUEGO - Mala Reputación");
        Time.timeScale = 0f;
    }

    public int ObtenerMalaReputacion()
    {
        return malaReputacionActual;
    }
}
