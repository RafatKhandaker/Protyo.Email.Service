

namespace Protyo.Utilities.Services
{
    using Protyo.Utilities.Helper;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;

    public class Cache<X,Y>
    {
        public Dictionary<X, Y> CacheStorage { get; set; }
        private readonly Func<Dictionary<X, Y>> _dataRetriever;
        private readonly TimeSpan _refreshInterval;
        private readonly Timer _refreshTimer;

        private ObjectExtensionHelper Helper;

        public Cache(Func<Dictionary<X, Y>> dataRetriever, TimeSpan refreshInterval)
        {
            CacheStorage = dataRetriever();
            Helper = new ObjectExtensionHelper();

            _dataRetriever = dataRetriever;
            _refreshInterval = refreshInterval;

            _refreshTimer = new Timer(RefreshCache, null, TimeSpan.Zero, _refreshInterval);
        }

        public Y Get(X key)
        {
            lock (CacheStorage)
            {
                if (CacheStorage.ContainsKey(key))
                    return CacheStorage[key];
                
                return default(Y);
            }
        }

        public List<Y> GetAll()
        {
            lock (CacheStorage)
                return CacheStorage.Select(s=> s.Value).ToList();
        }

        private void RefreshCache(object state)
        {
            lock (CacheStorage) {
                var merger = _dataRetriever().Where(w => !CacheStorage.Keys.Contains(w.Key)).ToDictionary(x => x.Key, y => y.Value);
                CacheStorage = Helper.MergeDictionaries<X, Y>(CacheStorage, merger);
            }
        }

    }
}

