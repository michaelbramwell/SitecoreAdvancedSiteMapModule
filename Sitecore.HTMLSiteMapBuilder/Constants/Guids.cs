using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sitecore.Data;

namespace Sitecore.AdvancedSiteMap.Constants
{
    public class Guids
    {
        public static class Items
        {
            public static ID AdvancedSiteMapConfigItemId = new ID("{0E91DDD2-9BA2-47E5-9C7F-484C0E60484F}");
            public static ID LangaugesRootItem = new ID("{64C4F646-A3FA-4205-B98E-4DE2C609B60F}");
        }

        public static class Templates
        {
            public static ID SiteMapSearchEngineConfigTemplateId = new ID("{D4939687-C5DE-4165-AE0D-206829104E99}");
            public static ID SiteMapSiteTemplateId = new ID("{AACEFA89-BF04-4DF1-B6C1-09975D1F59FC}");
        }
    }
}
