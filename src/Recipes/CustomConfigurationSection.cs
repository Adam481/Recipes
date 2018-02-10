using System;
using System.Configuration;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Recipes.Tests")]

namespace Recipes
{
    // TODO: add description
    // It does not work in .net core
    public class MySection : ConfigurationSection
    {
        private MySection() { }

        public static MySection GetConfig() 
            => (MySection)ConfigurationManager.GetSection("mySection");


        [ConfigurationProperty("items")]
        [ConfigurationCollection(typeof(Items), AddItemName = "item")]
        public Items Items => this["items"] as Items;
    }


    public class Items : ConfigurationElementCollection
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


    public class ItemConfiguration : ConfigurationElement
    {
        [ConfigurationProperty("name", IsRequired = true)]
        public string Name => this["name"].ToString();

        [ConfigurationProperty("subitem", IsRequired = true)]
        public Subitem Subitem => this["subitem"] as Subitem;

        [ConfigurationProperty("othersub", IsRequired = true)]
        public OtherSubitem OtherSub => this["othersub"] as OtherSubitem;
    }


    public class Subitem : ConfigurationElement
    {

        [ConfigurationProperty("firstproperty", IsRequired = true)]
        public int FirstProperty => Int32.Parse(this["firstproperty"].ToString());


        [ConfigurationProperty("secondproperty", IsRequired = true)]
        public string SecondProperty => this["secondproperty"].ToString();
    }


    public class OtherSubitem : ConfigurationElement
    {
        [ConfigurationProperty("firstproperty", IsRequired = true)]
        public int FirstProeprty => Int32.Parse(this["firstpropertys"].ToString());
        
    }
}
