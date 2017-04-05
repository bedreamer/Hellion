using Hellion.Core.Data.Headers;
using System.Collections.Generic;

namespace Hellion.World.Structures
{
    public class Attributes
    {
        private Dictionary<uint, int> attributes;
        private Dictionary<uint, int> baseAttributes;
        private Dictionary<uint, int> bonusAttributes;

        /// <summary>
        /// Gets or sets the value of an attribute.
        /// </summary>
        /// <param name="attr">Attribute to set</param>
        /// <returns></returns>
        public int this[DefineAttributes attr]
        {
            get { return this.Get((uint)attr); }
            set { this.Set((uint)attr, value); }
        }

        /// <summary>
        /// Creates a new Attributes instance.
        /// </summary>
        public Attributes()
        {
            this.attributes = new Dictionary<uint, int>();
            this.baseAttributes = new Dictionary<uint, int>();
            this.bonusAttributes = new Dictionary<uint, int>();
        }

        /// <summary>
        /// Get the value of an attribute by his key.
        /// </summary>
        /// <param name="key">Attribute key</param>
        /// <returns></returns>
        private int Get(uint key)
        {
            return this.attributes.ContainsKey(key) ? this.attributes[key] : 0;
        }

        /// <summary>
        /// Set or create the attribute with key and value.
        /// </summary>
        /// <param name="key">Attribute key</param>
        /// <param name="value">Attribute value</param>
        private void Set(uint key, int value)
        {
            if (this.attributes.ContainsKey(key))
                this.attributes[key] = value;
            else
                this.attributes.Add(key, value);
        }
    }
}
