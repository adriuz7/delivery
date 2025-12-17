using UnityEngine;

public class DineroJugador : MonoBehaviour
{
    public static DineroJugador Instance;
    public int dineroActual;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void SumarDinero(int monto)
    {
        dineroActual += monto;
        Debug.Log("Dinero actual: " + dineroActual);
    }
}
