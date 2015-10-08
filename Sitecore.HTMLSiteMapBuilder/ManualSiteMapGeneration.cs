using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sitecore.Shell.Framework.Commands;
using Sitecore.Web.UI.Sheer;

namespace Sitecore.AdvancedSiteMap
{
    [Serializable]
    public class ManualSiteMapGeneration : Command
    {
        public override void Execute(CommandContext context)
        {
            Sitecore.Context.ClientPage.Start(this, "Run", context.Parameters);
        }

        protected static void Run(ClientPipelineArgs args)
        {
            var SyunObj = new SiteMapBuilder();
            SyunObj.BuildSiteMap();
        }
    }
}
