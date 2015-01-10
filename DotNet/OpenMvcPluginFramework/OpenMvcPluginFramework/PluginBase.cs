using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Routing;
using OpenMvcPluginFramework.Interfaces;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace OpenMvcPluginFramework
{
    public class PluginBase : IPlugin
    {
        private const string ViewLocationFormat = "/Views.{1}.{0}.cshtml";
        private const string RessourceUrlFormat = "~/{0}/{1}.{2}";

        protected IList<PluginElement> _elements;
        protected IList<IPluginResource> _cssResources;
        protected IList<IPluginResource> _scriptResources; 

        public PluginBase()
        {
            _elements = new List<PluginElement>();
            _cssResources = new List<IPluginResource>();
            _scriptResources = new List<IPluginResource>();
        }

        public IEnumerable<PluginElement> Elements { get { return _elements; } }
        public IEnumerable<IPluginResource> CssResources { get { return _cssResources; } }
        public IEnumerable<IPluginResource> ScriptResources { get { return _scriptResources; } }

        public void AddElement(PluginElement element)
        {
            _elements.Add(element);
        }

        public void AddCssResource(IPluginResource resource)
        {
            _cssResources.Add(resource);
        }

        public void AddScriptResource(IPluginResource resource)
        {
            _scriptResources.Add(resource);
        }

        public string ViewLocation { get { return GetViewLocation(); } }

        public void Render(System.Web.Mvc.HtmlHelper html)
        {   
            foreach (var element in _elements)
            {
                html.RenderAction(element.Action, element.Controller, null);
            }
        }

        private string GetViewLocation()
        {
            string assemblyName = this.GetType().Assembly.GetName().Name;
            string newName = "~/Plugins/" + assemblyName;
            return newName + ViewLocationFormat;
        }

        protected string GetResourceLocation(string resourcename, string filename)
        {
            string assemblyName = this.GetType().Assembly.GetName().Name;
            return String.Format(RessourceUrlFormat,resourcename, assemblyName, filename);
        }
    }
}
