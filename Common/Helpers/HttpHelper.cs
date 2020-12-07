using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Authentication.Helpers
{
    public static class HttpHelper
    {
        public static string conStrSql;
        private static IHttpContextAccessor _accessor;
        private static IConfiguration _config;
        public static void Configure(IHttpContextAccessor httpContextAccessor, IConfiguration config)
        {
            _accessor = httpContextAccessor;
            _config = config;
        }

        public static HttpContext HttpContext => _accessor.HttpContext;


        public static T GetService<T>()
        {
            return (T)_accessor.HttpContext.RequestServices.GetService(typeof(T));
        }
        public static T GetConfig<T>(string key)
        {

            var appSetting = _config[key.Trim()];
            if (string.IsNullOrWhiteSpace(appSetting)) throw new Exception(key);

            var converter = TypeDescriptor.GetConverter(typeof(T));
            return (T)(converter.ConvertFromInvariantString(appSetting));
        }
    }
}
