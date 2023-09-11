﻿using BinaryQuest.Framework.ModularCore.Data;
using BinaryQuest.Framework.ModularCore.Exceptions;
using BinaryQuest.Framework.ModularCore.Interface;
using BinaryQuest.Framework.ModularCore.Model;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OData;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


namespace BinaryQuest.Framework.ModularCore.Implementation
{
    internal class BQBuilder<TContext> : IBQBuilder where TContext : DbContext
    {        
        private Action<IEndpointRouteBuilder>? customEndpoints;
        
        public BQBuilder(IApplicationBuilder builder)
        {
            Builder = builder;            
        }

        public IApplicationBuilder Builder { get; }        

        public IBQBuilder Build()
        {
            Builder.UseMiddleware<StatusCodeExceptionHandler>();

            Builder.UseRouting();
            
            Builder.UseAuthentication();            
            Builder.UseAuthorization();            

            Builder.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");

                customEndpoints?.Invoke(endpoints);                

                endpoints.MapControllers();
            });

            return this;
        }

        public IBQBuilder UseCustomEndpoints(Action<IEndpointRouteBuilder> endpoints)
        {
            this.customEndpoints = endpoints;
            return this;
        }
    }
}
