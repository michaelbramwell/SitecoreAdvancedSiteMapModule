# SitecoreAdvancedSiteMapModule
Sitecore Advanced SiteMap Module Source Code 

## Current Form Notes
Fixed HTML sitemap issues; to the extent that it actually works now and outputs a tree of nodes starting from the root of the current site. 
Still could do with a couple of less hardcoded values. Everything else is pretty much as the previous fork.

## Previous Fork Notes
Fork of this module to fix issue with hostname doubling up in xml sitemap.

to resolve issues with multisite link resolution here, set the rootPath="#" on the publisher site in web.config and also add scheme="http" to the Site Definition for your site

Setting the ServerURL setting under SiteMap Sites will now override the hostname in your Site Definition. 

See: http://blog.ryanbailey.co.nz/2016/04/sitecore-advanced-sitemap-module-updated.html for update Sitecore package for this fork.
