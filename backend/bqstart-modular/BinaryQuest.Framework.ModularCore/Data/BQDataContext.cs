using BinaryQuest.Framework.ModularCore.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace BinaryQuest.Framework.ModularCore.Data
{
    public abstract class BQDataContext : Microsoft.EntityFrameworkCore.DbContext
    {
        

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            var propertyNames = typeof(INotPersistingProperties).GetProperties()
                                .Select(p => p.Name)
                                .ToList();

            var entityTypes = builder.Model.GetEntityTypes()
                .Where(t => typeof(INotPersistingProperties).IsAssignableFrom(t.ClrType));

            foreach (var entityType in entityTypes)
            {
                var entityTypeBuilder = builder.Entity(entityType.ClrType);
                foreach (var propertyName in propertyNames)
                    entityTypeBuilder.Ignore(propertyName);
            }
        }
    }
}
