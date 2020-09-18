using System.Collections.Generic;
using Attributes;

public static partial class Beneath
{
    public static class Items
    {
        
        public static readonly ItemData NullKey;

        static Items()
        {
            NullKey = new ItemData(0,"NullKey", "You shouldn't have this.", AssetReferences.NullKeySprite, ItemTypes.Weapon, 0, new ItemAttribute[]{new DamageAttribute(999)});
        }

        public static ItemData GetItemWithID(int id)
        {
            switch (id)
            {
                case -1: return null;
                case 0: return NullKey;
            }

            return null;
        }

        public static bool IsItemValid(ItemData data)
        {
            return GetItemWithID(data.id) == data;
        }
        
        public static bool IsItemIDValid(int id)
        {
            return GetItemWithID(id) != null;
        }

        public static int GetDefinedItemsCount() { return 1; }


    }
        
}