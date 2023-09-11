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
        void SetObject<T>(string key, T obj);
        void DeleteObject(string key);
        void ClearCache();        
    }
}
