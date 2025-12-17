public interface IReceptorEntrega
{
    string NombreDestino { get; }
    bool PuedeRecibir(ItemWorld item);
    void Recibir(ItemWorld item);
}
