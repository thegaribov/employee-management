using EmployeeManagement.Service.Business.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Service.Business.Implementations
{
    public class CacheService : ICacheService
    {
        private ObjectCache _memoryCache = MemoryCache.Default;

        public T GetData<T>(string key)
        {
            try
            {
                T item = (T)_memoryCache.Get(key);
                return item;
            }
            catch (Exception e)
            {
                throw;
            }
        }
        
        public bool SetData<T>(string key, T value, DateTimeOffset expirationTime)
        {
            bool res = true;
            try
            {
                if (!string.IsNullOrEmpty(key))
                {
                    _memoryCache.Set(key, value, expirationTime);
                }
            }
            catch (Exception e)
            {
                throw;
            }

            return res;
        }
        
        public object RemoveData(string key)
        {
            try
            {
                if (!string.IsNullOrEmpty(key))
                {
                    return _memoryCache.Remove(key);
                }
            }
            catch (Exception e)
            {
                throw;
            }

            return false;
        }

    }
}
