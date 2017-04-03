using Sitecore.Mvc.Extensions;
using Sitecore.Mvc.Presentation;
using System.Xml.Linq;

namespace Sitecore.Support.Mvc.Presentation
{
    public class XmlBasedRenderingParser : Sitecore.Mvc.Presentation.XmlBasedRenderingParser
    {
        public override Rendering Parse(XElement node, bool parseChildNodes)
        {
            Rendering rendering = base.Parse(node, parseChildNodes);
            rendering["ClearOnIndexUpdate"] = node.GetAttributeValueOrNull("ciu");
            return rendering;
        }
    }
}
