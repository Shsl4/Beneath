namespace Assets.Scripts.UI.Inventory
{
    public abstract class InventoryInterface : UserInterface
    {
        public InventoryManager Manager => MasterView.GetComponent<InventoryManager>();
    }
}
