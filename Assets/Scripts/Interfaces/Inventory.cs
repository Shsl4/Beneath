namespace Interfaces
{
    public interface IInventory
    {

        Inventory GetInventory();
        bool PickupItem(ItemData itemData);

    }
}
