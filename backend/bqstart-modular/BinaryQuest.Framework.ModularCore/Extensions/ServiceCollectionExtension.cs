using BinaryQuest.Framework.ModularCore.Data;
using BinaryQuest.Framework.ModularCore.Implementation;
using BinaryQuest.Framework.ModularCore.Interface;
using BinaryQuest.Framework.ModularCore.Model;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.OData;
using System.Text.Json;
using System.Text.Json.Serialization;
using Newtonsoft.Json.Serialization;
using BinaryQuest.Framework.ModularCore.OData;
using Newtonsoft.Json.Converters;
using System.Globalization;
using Microsoft.EntityFrameworkCore;

namespace BinaryQuest.Framework.ModularCore
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddBqAdminServices<TDb>(this IServiceCollection services, Action<AppConfigOptions> options)
            where TDb : DbContext        
        {
            var opt = AppConfigOptions.Default;
            options?.Invoke(opt);
            var appSvc = new ApplicationService(opt, typeof(TDb));
            services.AddSingleton<IApplicationService>(appSvc);
            services.AddSingleton<ICacheManager>(appSvc);
            services.AddSingleton(opt.SecurityRulesProvider);

            services.AddControllers()
                .AddNewtonsoftJson(json =>
                {
                    json.SerializerSettings.DateTimeZoneHandling = opt.TimeZoneHandling;
                    json.SerializerSettings.MaxDepth = 32;
                    //if (opt.JsonCamelCase)
                    //{
                    //    json.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    //}
                    //else
                    //{
                    //    json.SerializerSettings.ContractResolver = null;
                    //}
                    json.SerializerSettings.PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.None;
                    json.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                })
                //.AddJsonOptions(options =>
                //{
                //    //options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                //    options.JsonSerializerOptions.MaxDepth = 32;
                //    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                //})
                .AddOData(oData =>
                {
                    oData.TimeZone = opt.TimeZoneHandling == Newtonsoft.Json.DateTimeZoneHandling.Utc ? TimeZoneInfo.Utc : TimeZoneInfo.Local;
                    oData.AddRouteComponents("odata", opt.GetEdmModel<TDb>(services, appSvc));
                    oData.EnableAttributeRouting = true;
                    oData.OrderBy().Filter().Select().Expand().Count().SetMaxTop(null);
                    oData.Conventions.Add(new BQEntityRoutingConvention());
                });


            return services;
        }


        //private static Type FindGenericBaseType(Type currentType, Type genericBaseType)
        //{
        //    var type = currentType;
        //    while (type != null)
        //    {
        //        var genericType = type.IsGenericType ? type.GetGenericTypeDefinition() : null;
        //        if (genericType != null && genericType == genericBaseType)
        //        {
        //            return type;
        //        }
        //        type = type.BaseType;
        //    }
        //    return null;
        //}
    }
}
