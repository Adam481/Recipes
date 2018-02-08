using System;
using System.Configuration;

namespace Recipes.Common
{
    // TODO: add description
    internal class MySection : ConfigurationSection
    {
        private MySection() { }

        public static MySection GetConfig() 
            => (MySection)ConfigurationManager.GetSection("mySection");


        [ConfigurationProperty("items")]
        [ConfigurationCollection(typeof(Items), AddItemName = "worker")]
        public Items Workers => this["items"] as Items;
    }


    internal class Items : ConfigurationElementCollection
    {
        public ItemConfiguration this[int index]
        {
            get => BaseGet(index) as ItemConfiguration;
            set
            {
                if (BaseGet(index) != null)
                {
                    BaseRemoveAt(index);
                }
                BaseAdd(index, value);
            }
        }

        protected new ItemConfiguration this[string responseString]
        {
            get => (ItemConfiguration)BaseGet(responseString);
            set
            {
                if (BaseGet(responseString) != null)
                {
                    BaseRemoveAt(BaseIndexOf(BaseGet(responseString)));
                }
                BaseAdd(value);
            }
        }


        protected override ConfigurationElement CreateNewElement()
            => new ItemConfiguration();


        protected override object GetElementKey(ConfigurationElement element)
            => ((ItemConfiguration)element).Name;
    }


    internal class ItemConfiguration : ConfigurationElement
    {
        [ConfigurationProperty("name", IsRequired = true)]
        public string Name => this["name"].ToString();

        [ConfigurationProperty("subitem", IsRequired = true)]
        public Subitem Subitem => this["subitem"] as Subitem;

        [ConfigurationProperty("othersub", IsRequired = true)]
        public OtherSubitem OtherSub => this["othersub"] as OtherSubitem;
    }


    internal class Subitem : ConfigurationElement
    {

        [ConfigurationProperty("firstproperty", IsRequired = true)]
        public int FirstProperty => Int32.Parse(this["firstproperty"].ToString());


        [ConfigurationProperty("secondproperty", IsRequired = true)]
        public int SecondProperty => Int32.Parse(this["secondproperty"].ToString());
    }


    internal class OtherSubitem : ConfigurationElement
    {
        [ConfigurationProperty("firstproperty", IsRequired = true)]
        public int FirstProeprty => Int32.Parse(this["firstpropertys"].ToString());
        
    }
}
