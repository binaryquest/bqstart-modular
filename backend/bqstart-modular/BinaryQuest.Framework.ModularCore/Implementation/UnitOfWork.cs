﻿using BinaryQuest.Framework.ModularCore.Data;
using BinaryQuest.Framework.ModularCore.Exceptions;
using BinaryQuest.Framework.ModularCore.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryQuest.Framework.ModularCore.Implementation
{
    public class UnitOfWork : IDisposable, IUnitOfWork
    {
        private readonly DbContext context;

        private readonly Dictionary<Type, object> _repos;

        public UnitOfWork(DbContext context)
        {
            _repos = new Dictionary<Type, object>();
            this.context = context;
            
            //TODO: CHECK lazy loading later
            //context.Configuration.LazyLoadingEnabled = false;
            
        }

        public IGenericRepository<TEntity> GenericRepository<TEntity>() where TEntity : class
        {
            if (_repos.ContainsKey(typeof(TEntity)))
            {
                return (IGenericRepository<TEntity>)_repos[typeof(TEntity)];
            }
            else
            {
                var repo = _repos[typeof(TEntity)] = new GenericRepository<TEntity>(context);
                return (IGenericRepository<TEntity>)repo;
            }
        }



        public async Task SaveAsync()
        {
            var validationErrors = context.ChangeTracker.Entries<IValidatableObject>().SelectMany(e => e.Entity.Validate(null!)).Where(r => r != ValidationResult.Success);

            if (validationErrors.Any())
            {
                throw new DbEntityValidationException(validationErrors);
            }

            await context.SaveChangesAsync();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
