﻿using BinaryQuest.Framework.ModularCore.Interface;
using BinaryQuest.Framework.ModularCore.Model;
using CacheManager.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryQuest.Framework.ModularCore.Implementation
{
    internal sealed class ApplicationService : IApplicationService, ICacheManager
    {
        readonly object lockObject = new();
        private readonly AppConfigOptions configOptions;
        private readonly ICacheManager<object> cacheManager;                
        private readonly Type dbContextType;

        public ApplicationService(AppConfigOptions configOptions, Type dbContextType)
        {
            this.Bootdata = new Bootdata(configOptions);
            this.configOptions = configOptions;            
            this.dbContextType = dbContextType;
            var cacheConfig = ConfigurationBuilder.BuildConfiguration(settings =>
            {
                settings.WithMicrosoftMemoryCacheHandle("inprocess");
            });

            cacheManager = CacheFactory.FromConfiguration<object>("systemCache", cacheConfig);            
        }

        public Bootdata Bootdata { get; }
        public AppConfigOptions ConfigOptions => configOptions;        

        public Type DbContextType => dbContextType;

        public void ClearCache()
        {
            if (cacheManager == null)
                return;
            lock (lockObject)
            {
                cacheManager.Clear();
            }
        }

        public void DeleteObject(string key)
        {
            key = "cache:" + "/" + key;
            if (cacheManager != null)
            {
                lock (lockObject)
                {
                    cacheManager.Remove(key);
                }
            }
        }        

        public T? GetObject<T>(string key)
        {
            key = "cache:" + "/" + key;
            if (cacheManager != null)
            {
                lock (lockObject)
                {
                    return (T)(cacheManager[key]);
                }
            }
            return default;
        }

        public bool ObjectExists(string key)
        {
            key = "cache:" + "/" + key;
            if (cacheManager != null)
            {
                lock (lockObject)
                {
                    return (cacheManager[key] != null);
                }
            }
            return false;
        }

        public void SetObject<T>(string key, T? obj)
        {
            key = "cache:" + "/" + key;
            if (obj != null)
            {
                if (cacheManager != null)
                {
                    lock (lockObject)
                    {
                        cacheManager.AddOrUpdate(key, obj, (x) => { return obj; });
                    }
                }
            }
        }

        public T? TryToGetObject<T>(string key, Func<T> newObjectProvider)
        {
            key = "cache:" + "/" + key;
            if (cacheManager != null)
            {
                lock (lockObject)
                {
#pragma warning disable CS8603 // Possible null reference return.
                    return (T)cacheManager.GetOrAdd(key, (f) => { return newObjectProvider(); });
#pragma warning restore CS8603 // Possible null reference return.
                }
            }
            return default;
        }


    }
}
