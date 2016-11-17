using Sitecore.AdvancedSiteMap.Component;
using Sitecore.AdvancedSiteMap.Constants;
using Sitecore.Configuration;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Links;
using Sitecore.Sites;
using Sitecore.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sitecore.AdvancedSiteMap
{
    public static class HTMLSiteMapBuilder
    {
        private static Func<Database> GetTargetDatabase = () => Factory.GetDatabase(siteMapConfig.targetDatabaseName);
        private static Func<Item, bool> HasContentChildren = (child) => child.Children.Any(p => p.TemplateName == "Base Web Page");

        private static Func<string, string, string> NodeWithMarkup = (displayText, itemURL) =>
            string.Format("<li><p><a href='{0}'>{1}</a></p></li>", itemURL, displayText);

        private static Func<CheckboxField, bool> ShowNode = (showInHTMLSiteMap) => (showInHTMLSiteMap != null) ? showInHTMLSiteMap.Checked : false;
        private static SiteMapConfig siteMapConfig = new SiteMapConfig();

        public static string BuildSitemapHTML()
        {
            Item currentItem = Sitecore.Context.Item;
            SiteInfo currentSiteRoot = SiteContextFactory.Sites
                .Where(s => s.RootPath != "" && currentItem.Paths.Path.ToLower().StartsWith(s.RootPath.ToLower()))
                .OrderByDescending(s => s.RootPath.Length)
                .FirstOrDefault();

            if (currentSiteRoot == null)
            {
                return string.Empty;
            }

            Item root = SiteMapBuilder.GetTargetDatabase().GetItem(currentSiteRoot.RootPath);
            if (root == null)
            {
                return string.Empty;
            }

            Item home = root.Children.FirstOrDefault(p => p.Name == "Home");
            if (home == null)
            {
                return string.Empty;
            }

            UrlOptions options = GetOptions();
            StringBuilder sb = new StringBuilder("");

            if (ShowNode(home.Fields[SiteMapFields.ShowInHTMLSiteMap]))
            {
                sb.AppendFormat("<ul class=\"site-map\">{0}</ul>", NodeWithMarkup(GetTitle(home), LinkManager.GetItemUrl(home, options)));
                sb.Append(GetSiteMapTree(home));
            }

            return sb.ToString();
        }

        private static UrlOptions GetOptions()
        {
            var options = global::Sitecore.Links.LinkManager.GetDefaultUrlOptions();
            options.AlwaysIncludeServerUrl = true;
            options.LanguageEmbedding = LanguageEmbedding.Always;
            options.Language = Sitecore.Context.Language;
            options.EmbedLanguage(Sitecore.Context.Language);

            return options;
        }

        private static string GetSiteMapTree(Item node)
        {
            if (node == null)
            {
                return "";
            }

            UrlOptions options = GetOptions();
            IEnumerable<Item> children = node.Children.Where(x => !string.IsNullOrEmpty(x.Name));
            StringBuilder sb = new StringBuilder("");

            if (children == null || !children.Any())
            {
                return "";
            }

            sb.Append("<ul>");

            foreach (var child in children)
            {
                if (ShowNode(child.Fields[SiteMapFields.ShowInHTMLSiteMap]))
                {
                    sb.Append(NodeWithMarkup(GetTitle(child), Sitecore.Links.LinkManager.GetItemUrl(child, options)));
                }

                if (HasContentChildren(child))
                {
                    sb.Append(GetSiteMapTree(child));
                }
            }

            sb.Append("</ul>");

            return sb.ToString();
        }

        private static string GetTitle(Item node)
        {
            string displayText = string.Empty;

            if (node.Fields[SiteMapFields.HTMLSiteMapTitle] != null)
            {
                displayText = node.Fields[SiteMapFields.HTMLSiteMapTitle].Value;
            }

            if (string.IsNullOrEmpty(displayText) && node.Name != null)
            {
                displayText = (!string.IsNullOrEmpty(node.DisplayName)) ? node.DisplayName : node.Name;
            }

            return displayText;
        }
    }
}