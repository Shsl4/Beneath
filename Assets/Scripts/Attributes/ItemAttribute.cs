using System;

namespace Attributes
{
    [Serializable]
    public abstract class ItemAttribute
    {
    
        public readonly String AttributeName;

        protected ItemAttribute(String name)
        {
            AttributeName = name;
        }

        public abstract string Format();

    }
}
