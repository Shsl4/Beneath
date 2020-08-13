using System;

namespace Assets.Scripts.Attributes
{
    public abstract class ItemAttribute
    {
    
        public readonly String AttributeName;

        protected ItemAttribute(String name)
        {
            AttributeName = name;
        }

    }
}
