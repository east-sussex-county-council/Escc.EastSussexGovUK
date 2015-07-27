using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web;
using EsccWebTeam.Cms;
using EsccWebTeam.Data.Web;
using Microsoft.ApplicationBlocks.ExceptionManagement;
using Microsoft.ContentManagement.Publishing;

namespace EsccWebTeam.EastSussexGovUK.MasterPages.Controls
{
    /// <summary>
    /// Get breadcrumb trail data from Microsoft CMS
    /// </summary>
    public class MicrosoftCmsBreadcrumbProvider : IBreadcrumbProvider
    {
        /// <summary>
        /// Gets the data for a breadcrumb trail from the CMS channel, indexed by the display text with the URL to link to as the value
        /// </summary>
        /// <returns></returns>
        public IDictionary<string, string> BuildTrail()
        {
            var breadcrumbTrail = new Dictionary<string, string>();

            // get the CMS channel
            Channel channel = CmsUtilities.GetCurrentChannel();

            if (channel != null)
            {
                var cms = CmsHttpContext.Current;

                // add protocol & host when exiting SSL
                string linkPrefix = (HttpContext.Current.Request.Url.Scheme == Uri.UriSchemeHttps) ? Uri.UriSchemeHttp + "://" + HttpContext.Current.Request.Url.Host : "";

                // start with supplied (current) channel, and work backwards
                var levels = new Dictionary<string, string>();
                while (channel != null && channel.Name.ToLower() != "channels")
                {
                    if (!channel.IsHiddenModePublished || cms.Mode != PublishingMode.Published)
                    {
                        // channel.DefaultPostingName is empty, and postings not in sorted order by default,
                        // so sort a copy of the postings as a way to find the channel home page
                        string channelUrl = String.Empty;
                        PostingCollection postings = channel.Postings;
                        var defaultPosting = CmsUtilities.DefaultPostingInChannel(channel);

                        if (defaultPosting != null)
                        {
                            // If the default posting in this channel is a redirect and we're not in published mode, get where it's going
                            if (defaultPosting.Template.Guid == "{DD3D9CBC-93C4-4F72-8572-D3E96B8A9EAE}" && CmsHttpContext.Current.Mode == PublishingMode.Published)
                            {
                                CustomProperty urlProp = CmsUtilities.GetCustomProperty(defaultPosting.CustomProperties, "Url");
                                if (urlProp != null && !String.IsNullOrEmpty(urlProp.Value))
                                {
                                    // If it's an absolute URI, use as-is
                                    Uri targetUri;
                                    if (Uri.TryCreate(urlProp.Value, UriKind.RelativeOrAbsolute, out targetUri) && targetUri.IsAbsoluteUri)
                                    {
                                        channelUrl = urlProp.Value;
                                    }
                                    else
                                    {
                                        // Need a base URL to resolve the relative URL. That base URL cannot have a querystring so get the AbsolutePath.
                                        var cmsUrl = CmsUtilities.CorrectPublishedUrl(defaultPosting.Url);
                                        cmsUrl = Iri.MakeAbsolute(new Uri(cmsUrl, UriKind.RelativeOrAbsolute)).AbsolutePath;
                                        try
                                        {
                                            channelUrl = VirtualPathUtility.Combine(cmsUrl, urlProp.Value);
                                        }
                                        catch (HttpException ex)
                                        {
                                            // "This is not a valid virtual path" error. Hopefully solved now, but catch and fail gracefully if it happens again.
                                            ex.Data.Add("Base path", cmsUrl);
                                            ex.Data.Add("Relative path", urlProp.Value);
                                            ExceptionManager.Publish(ex);
                                        }
                                    }
                                }
                            }

                            if (String.IsNullOrEmpty(channelUrl))
                            {
                                // Otherwise get the URL of the default posting in this channel
                                channelUrl = CmsUtilities.CorrectPublishedUrl(defaultPosting.Url);
                            }

                            // If the link is to the current page we're at the channel home page, so don't link it.
                            // This is aimed at non-CMS pages found from the redirect template above, but code is here so that it doesn't
                            // trigger the String.IsNullOrEmpty test and link to the redirect page instead.
                            // Make sure we only do it for the final level of the trail; we always want to link higher up
                            if (channelUrl == HttpContext.Current.Request.Url.AbsolutePath && String.IsNullOrEmpty(HttpContext.Current.Request.QueryString.ToString())) channelUrl = String.Empty;
                        }
                        else
                        {
                            // If there are no postings it's probably a new channel, so just use the channel URL for now
                            channelUrl = CmsUtilities.CorrectPublishedUrl(channel.Url);
                        }

                        // if this is the current channel
                        if (channel == cms.Channel)
                        {
                            // if this is the channel home page, do not link to self
                            if (cms.Posting != null && (postings.Count == 0 || cms.Posting.Guid == defaultPosting.Guid))
                            {
                                levels[channel.DisplayName] = String.Empty;
                            }
                            else
                            {
                                // otherwise link to channel home page
                                levels[channel.DisplayName] = linkPrefix + channelUrl;

                                // update the link to use SSL if required
                                if (CmsUtilities.IsSecureChannel(channel)) levels[channel.DisplayName] = Uri.UriSchemeHttps + "://" + HttpContext.Current.Request.Url.Host + channelUrl;
                            }
                        }
                        else
                        {
                            // all other levels should be links, but check whether it's already an absolute link before building one up
                            Uri targetUri;
                            if (Uri.TryCreate(channelUrl, UriKind.Absolute, out targetUri))
                            {
                                // it's absolute, so use as-is
                                levels[channel.DisplayName] = channelUrl;
                            }
                            else
                            {
                                // it's relative, so combine with prefix
                                levels[channel.DisplayName] = linkPrefix + channelUrl;

                                // update the link to use SSL if required
                                if (CmsUtilities.IsSecureChannel(channel)) levels[channel.DisplayName] = Uri.UriSchemeHttps + "://" + HttpContext.Current.Request.Url.Host + channelUrl;
                            }
                        }
                    }

                    // change the condition
                    channel = channel.Parent;
                }

                // Add the home page as the final level
                levels["Home"] = linkPrefix + "/default.htm";

                // Convert list to a dictionary in reverse order
                var keys = new List<string>(levels.Keys);
                keys.Reverse();
                foreach (string key in keys) breadcrumbTrail.Add(key, levels[key]);

            }
            return breadcrumbTrail;
        }
    }
}