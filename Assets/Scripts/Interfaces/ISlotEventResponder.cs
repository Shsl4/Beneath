using UI.Inventory;

namespace Interfaces
{
    public interface ISlotEventResponder
    {
        void JumpToSelection();
        void RefreshInfoFromSlot(SlotComponent slot);
        void DropItemFromActiveSlot(bool discard);

    }
}