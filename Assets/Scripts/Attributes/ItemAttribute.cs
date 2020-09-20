using System;

namespace Attributes
{
    [Serializable]
    public abstract class ItemAttribute
    {
    
        public readonly string attributeName;

        protected ItemAttribute(string name)
        {
            attributeName = name;
        }

        public abstract string Format();

    }
}
