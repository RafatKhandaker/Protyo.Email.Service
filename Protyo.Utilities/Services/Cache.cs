

namespace Protyo.Utilities.Services
{
    using Protyo.Utilities.Helper;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;

    public class Cache<T>
    {
        public Dictionary<int, T> CacheStorage { get; set; }
        private readonly Func<Dictionary<int, T>> _dataRetriever;
        private readonly TimeSpan _refreshInterval;
        private readonly Timer _refreshTimer;

        private ObjectExtensionHelper Helper;

        public Cache(Func<Dictionary<int,T>> dataRetriever, TimeSpan refreshInterval)
        {
            CacheStorage = dataRetriever();
            Helper = new ObjectExtensionHelper();

            _dataRetriever = dataRetriever;
            _refreshInterval = refreshInterval;

            _refreshTimer = new Timer(RefreshCache, null, TimeSpan.Zero, _refreshInterval);
        }

        public T Get(int key)
        {
            lock (CacheStorage)
            {
                if (CacheStorage.ContainsKey(key))
                    return CacheStorage[key];
                
                return default(T);
            }
        }

        public List<T> GetAll()
        {
            lock (CacheStorage)
                return CacheStorage.Select(s=> s.Value).ToList();
        }

        private void RefreshCache(object state)
        {
            lock (CacheStorage)
                CacheStorage = Helper.MergeDictionaries<int, T>(CacheStorage, _dataRetriever());
        }

    }
}

