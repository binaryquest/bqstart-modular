using BinaryQuest.Framework.ModularCore.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryQuest.Framework.ModularCore.Interface
{
    public interface ICacheManager
    {
        bool ObjectExists(string key);
        T? GetObject<T>(string key);
        T? TryToGetObject<T>(string key, Func<T> newObjectProvider);
        T? TryToGetObject<T>(string key, Func<T> newObjectProvider, int expiry, bool isSliding);
        void SetObject<T>(string key, T obj);
        void SetObject<T>(string key, T obj, int expiry, bool isSliding);
        void DeleteObject(string key);
        void ClearCache();        
    }
}
