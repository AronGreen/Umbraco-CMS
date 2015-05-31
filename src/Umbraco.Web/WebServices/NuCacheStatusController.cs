﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Umbraco.Web.Cache;
using Umbraco.Web.PublishedCache;
using Umbraco.Web.PublishedCache.NuCache;
using Umbraco.Web.WebApi;

namespace Umbraco.Web.WebServices
{
    public class NuCacheStatusController : UmbracoAuthorizedApiController
    {
        private static FacadeService FacadeService
        {
            get
            {
                var svc = PublishedCachesServiceResolver.Current.Service as FacadeService;
                if (svc == null)
                    throw new NotSupportedException("Not running NuCache.");
                return svc;
            }
        }

        [HttpPost]
        public string RebuildDbCache()
        {
            var service = FacadeService;
            service.RebuildContentDbCache();
            service.RebuildMediaDbCache();
            service.RebuildMemberDbCache();
            return service.GetStatus();
        }

        [HttpGet]
        public string GetStatus()
        {
            var service = FacadeService;
            return service.GetStatus();
        }

        [HttpGet]
        public string Collect()
        {
            var service = FacadeService;
            GC.Collect();
            service.Collect();
            return service.GetStatus();
        }

        [HttpPost]
        public void ReloadCache()
        {
            DistributedCache.Instance.RefreshAllPublishedContentCache();
        }
    }
}