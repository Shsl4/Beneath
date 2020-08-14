using UnityEngine;

namespace Assets.Scripts.UI.Inventory.BottomView.Selection
{
    public class SelectionComponent : MonoBehaviour
    {

        public UseComponent Use => GetComponentInChildren<UseComponent>();
        public AttributesComponent Attributes => GetComponentInChildren<AttributesComponent>();
        public DiscardComponent Discard => GetComponentInChildren<DiscardComponent>();
        public DescriptionComponent Description => GetComponentInChildren<DescriptionComponent>();

    }
}
