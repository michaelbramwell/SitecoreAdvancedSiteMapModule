# SitecoreAdvancedSiteMapModule
Sitecore Advanced SiteMap Module Source Code 

Fork of this module to fix issue with hostname doubling up in xml sitemap.

to resolve issues with multisite link resolution here, set the rootPath="#" on the publisher site in web.config and also add scheme="http" to the Site Definition for your site

Setting the ServerURL setting under SiteMap Sites will now override the hostname in your Site Definition. 
