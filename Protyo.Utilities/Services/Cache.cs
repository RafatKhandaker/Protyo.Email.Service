

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
        private Func<Dictionary<X, Y>> _dataRetriever;
        private TimeSpan _refreshInterval;
        private Timer _refreshTimer;

        private ObjectExtensionHelper Helper;

      
        public Cache<X, Y> SetInstance(Func<Dictionary<X, Y>> dataRetriever, TimeSpan refreshInterval)
        {
            if (CacheStorage != null) return this;

            CacheStorage = dataRetriever();
            Helper = new ObjectExtensionHelper();

            _dataRetriever = dataRetriever;
            _refreshInterval = refreshInterval;

            _refreshTimer = new Timer(RefreshCache, null, TimeSpan.Zero, _refreshInterval);

            return this;
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

        public IEnumerable<Y> GetAll(int page, int size)
        {
            lock (CacheStorage)
                return CacheStorage.Values.ToList().Skip((page - 1) * size).Take(size);
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

