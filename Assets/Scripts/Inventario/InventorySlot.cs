[System.Serializable]
public class InventorySlot
{
    public ItemWorld itemWorld;

    public bool IsEmpty()
    {
        return itemWorld == null;
    }
}
