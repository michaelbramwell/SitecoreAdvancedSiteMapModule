using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sitecore.AdvancedSiteMap.Component;
using Sitecore.AdvancedSiteMap.Constants;
using Sitecore.Configuration;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Links;

namespace Sitecore.AdvancedSiteMap
{
    public class HTMLSiteMapBuilder
    {
        static SiteMapConfig siteMapConfig = new SiteMapConfig();
        StringBuilder sbSiteMapHTML = new StringBuilder();

        private Database GetTargetDatabase()
        {
            return Factory.GetDatabase(siteMapConfig.targetDatabaseName);
        }

        public string BuildSitemapHTML()
        {
            Item _currentItem = Sitecore.Context.Item;

            if (_currentItem == null)
                return string.Empty;

            Item _root = _currentItem.Parent;

            if (_root == null)
                return string.Empty;


            string displayText = string.Empty;
            if (_root.Fields[SiteMapFields.HTMLSiteMapTitle] != null)
                displayText = _root.Fields[SiteMapFields.HTMLSiteMapTitle].Value;

            if (displayText == string.Empty && _root.Name != null)
                displayText = _root.Name;

            var options = global::Sitecore.Links.LinkManager.GetDefaultUrlOptions();
            options.AlwaysIncludeServerUrl = true;
            options.LanguageEmbedding = LanguageEmbedding.Always;
            options.Language = Sitecore.Context.Language;
            options.EmbedLanguage(Sitecore.Context.Language);

            string itemURL = Sitecore.Links.LinkManager.GetItemUrl(_root, options);
            bool ShowInHTMLSiteMap = false;
            CheckboxField _ShowInHTMLSiteMap = _root.Fields[SiteMapFields.ShowInHTMLSiteMap];
            if (_ShowInHTMLSiteMap != null)
                ShowInHTMLSiteMap = _ShowInHTMLSiteMap.Checked;


            if (ShowInHTMLSiteMap)
            {
                sbSiteMapHTML.Append("<ul>");

                sbSiteMapHTML.Append("<li>");
                sbSiteMapHTML.Append("<a href='" + itemURL + "'>");
                sbSiteMapHTML.Append(displayText);
                sbSiteMapHTML.Append("</a>");
                sbSiteMapHTML.Append("</li>");
                RecursiveChildBuilder(_root);

                sbSiteMapHTML.Append("</ul>");
            }
            else
            {
                RecursiveChildBuilder(_root);
            }

            return sbSiteMapHTML.ToString();
        }

        private void RecursiveChildBuilder(Item Root)
        {
            if (Root == null)
                return;

            Item _currentLoopItem = Root;
            if (_currentLoopItem == null)
                return;


            var options = global::Sitecore.Links.LinkManager.GetDefaultUrlOptions();
            options.AlwaysIncludeServerUrl = true;
            options.LanguageEmbedding = LanguageEmbedding.Always;
            options.Language = Sitecore.Context.Language;
            options.EmbedLanguage(Sitecore.Context.Language);

            List<Item> children = _currentLoopItem.Children.Where(x => x.Name != "*").ToList();

            if (children != null && children.Any())
            {
                sbSiteMapHTML.Append("<ul>");

                foreach (var child in children)
                {
                    string displayText = string.Empty;
                    if (child.Fields[SiteMapFields.HTMLSiteMapTitle] != null)
                        displayText = child.Fields[SiteMapFields.HTMLSiteMapTitle].Value;

                    if (displayText == string.Empty && child.Name != null)
                        displayText = child.Name;

                    string itemURL = Sitecore.Links.LinkManager.GetItemUrl(child, options);

                    bool ShowInHTMLSiteMap = false;
                    CheckboxField _ShowInHTMLSiteMap = child.Fields[SiteMapFields.ShowInHTMLSiteMap];
                    if (_ShowInHTMLSiteMap != null)
                        ShowInHTMLSiteMap = _ShowInHTMLSiteMap.Checked;

                    if (ShowInHTMLSiteMap)
                    {
                        sbSiteMapHTML.Append("<li>");
                        sbSiteMapHTML.Append("<a href='" + itemURL + "'>");
                        sbSiteMapHTML.Append(displayText);
                        sbSiteMapHTML.Append("</a>");
                        sbSiteMapHTML.Append("</li>");
                    }

                    if (child.HasChildren)
                        RecursiveChildBuilder(child);
                }

                sbSiteMapHTML.Append("</ul>");
            }
        }
    }
}
