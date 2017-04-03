using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Globalization;
using Sitecore.Mvc.Extensions;
using Sitecore.Mvc.Pipelines.Response.RenderRendering;
using Sitecore.Mvc.Presentation;
using Sitecore.SecurityModel;

namespace Sitecore.Support.Mvc.Pipelines.Response.RenderRendering
{
    public class GenerateCacheKey : Sitecore.Mvc.Pipelines.Response.RenderRendering.GenerateCacheKey
    {
        protected override string GenerateKey(Rendering rendering, RenderRenderingArgs args)
        {
            string text = rendering.Caching.CacheKey.OrIfEmpty(args.Rendering.Renderer.ValueOrDefault((Renderer renderer) => renderer.CacheKey));
            string result;
            if (text.IsEmptyOrNull())
            {
                result = null;
            }
            else
            {
                string text2 = text + "_#lang:" + Language.Current.Name.ToUpper();
                RenderingCachingDefinition caching = rendering.Caching;
                Item contentDatasource = null;
                Item renderingItem = null;
                Database currentDb = null;
                using (new SecurityDisabler())
                {
                    contentDatasource = rendering.Item;
                }
                if (contentDatasource != null)
                    currentDb = contentDatasource.Database;
                else
                {
                    currentDb = PageContext.Current.Database;
                }
                
                if (currentDb != null)
                {
                    renderingItem = currentDb.GetItem(rendering.RenderingItemPath);
                }
                if (rendering["ClearOnIndexUpdate"] == "1" || (renderingItem!=null&& renderingItem.Fields["F3E7E552-D7C8-469B-A150-69E4E14AB35C"].Value == "1"))
                    {
                        text2 += "_#index";
                    }
                if (caching.VaryByData)
                {
                    text2 += this.GetDataPart(rendering);
                }
                if (caching.VaryByDevice)
                {
                    text2 += this.GetDevicePart(rendering);
                }
                if (caching.VaryByLogin)
                {
                    text2 += this.GetLoginPart(rendering);
                }
                if (caching.VaryByUser)
                {
                    text2 += this.GetUserPart(rendering);
                }
                if (caching.VaryByParameters)
                {
                    text2 += this.GetParametersPart(rendering);
                }
                if (caching.VaryByQueryString)
                {
                    text2 += this.GetQueryStringPart(rendering);
                }
                result = text2;
            }
            return result;
        }
    }
}
