using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;

namespace OpenMvcPluginFramework.Interfaces
{
    public interface IPluginMetaData
    {
        string Name { get; }
        string Description { get; }
    }

    public class PluginMetaData : IPluginMetaData
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }

    [MetadataAttribute]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class PluginMetaDataAttribute : ExportAttribute
    {
        public string Name { get; private set; }
        public string Description { get; private set; }

        public PluginMetaDataAttribute(string name, string description) : base(typeof(IPluginMetaData))
        {
            if (String.IsNullOrEmpty(name)) throw new ArgumentException("name");
            if (String.IsNullOrEmpty(description)) throw new ArgumentException("description");

            Name = name;
            Description = description;
        }

        public override bool Equals(object obj)
        {
            var metaData = (PluginMetaDataAttribute)obj;

            return String.Compare(Name, metaData.Name, System.StringComparison.OrdinalIgnoreCase) == 0;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }


}
