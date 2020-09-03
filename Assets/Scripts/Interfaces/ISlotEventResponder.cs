using UI.Inventory;

namespace Interfaces
{
    public interface ISlotEventResponder
    {
        void JumpToSelection();
        void SetActiveSlot(SlotComponent slot);
        SlotComponent GetActiveSlot();
        void RefreshInfoFromSlot(SlotComponent slot);
        void DropItemFromActiveSlot(bool discard);

    }
}