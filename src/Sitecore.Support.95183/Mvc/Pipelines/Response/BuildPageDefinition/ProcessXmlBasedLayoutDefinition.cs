using Sitecore.Mvc.Configuration;
using Sitecore.Mvc.Extensions;
using Sitecore.Mvc.Pipelines.Response.BuildPageDefinition;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Sitecore.Support.Mvc.Pipelines.Response.BuildPageDefinition
{
    internal class ProcessXmlBasedLayoutDefinition : Sitecore.Mvc.Pipelines.Response.BuildPageDefinition.ProcessXmlBasedLayoutDefinition
    {
        protected virtual Rendering GetRenderingFix(XElement renderingNode, Guid deviceId, Guid layoutId, string renderingType, Sitecore.Support.Mvc.Presentation.XmlBasedRenderingParser parser)
        {
            Rendering rendering = parser.Parse(renderingNode, false);
            rendering.DeviceId = deviceId;
            rendering.LayoutId = layoutId;
            if (renderingType != null)
            {
                rendering.RenderingType = renderingType;
            }
            return rendering;
        }

        protected override IEnumerable<Rendering> GetRenderings(XElement layoutDefinition, BuildPageDefinitionArgs args)
        {
            Sitecore.Support.Mvc.Presentation.XmlBasedRenderingParser registeredObject;
            try
            {
                registeredObject = MvcSettings.GetRegisteredObject<Sitecore.Support.Mvc.Presentation.XmlBasedRenderingParser>();
            }
            catch (InvalidOperationException)
            {
                MvcSettings.RegisterObject<Sitecore.Support.Mvc.Presentation.XmlBasedRenderingParser>(() => new Sitecore.Support.Mvc.Presentation.XmlBasedRenderingParser());
                registeredObject = MvcSettings.GetRegisteredObject<Sitecore.Support.Mvc.Presentation.XmlBasedRenderingParser>();
            }
            foreach (XElement current in layoutDefinition.Elements("d"))
            {
                Guid deviceId = current.GetAttributeValueOrEmpty("id").ToGuid();
                Guid layoutId = current.GetAttributeValueOrEmpty("l").ToGuid();
                yield return this.GetRenderingFix(current, deviceId, layoutId, "Layout", registeredObject);
                foreach (XElement current2 in current.Elements("r"))
                {
                    yield return this.GetRenderingFix(current2, deviceId, layoutId, current2.Name.LocalName, registeredObject);
                }
            }
            yield break;
        }
    }
}
