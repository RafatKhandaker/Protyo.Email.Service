using Protyo.Utilities.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Protyo.Utilities.Services
{
    public class ListCache<X>
    {
        public List<X> CacheStorage { get; set; }
        private Func<List<X>> _dataRetriever;
        private TimeSpan _refreshInterval;
        private Timer _refreshTimer;

        private ObjectExtensionHelper Helper;

        public ListCache<X> SetInstance(Func<List<X>> dataRetriever, TimeSpan refreshInterval)
        {
            if (CacheStorage != null) return this; 

            CacheStorage = dataRetriever();
            Helper = new ObjectExtensionHelper();

            _dataRetriever = dataRetriever;
            _refreshInterval = refreshInterval;

            _refreshTimer = new Timer(RefreshCache, null, TimeSpan.Zero, _refreshInterval);

            return this;
        }
      
        public X Get(int key)
        {
            lock (CacheStorage)
                return CacheStorage.Where(s => (int)s.GetType().GetField("_id").GetValue(s) == key).FirstOrDefault();
        }
        public List<X> GetAll()
        {
            lock (CacheStorage)
                return CacheStorage;
        }

        public List<X> GetAll(int page, int size)
        {
            lock (CacheStorage)
                return CacheStorage.Skip((page - 1) * size).Take(size).ToList();
        }

        private void RefreshCache(object state)
        {
            lock (CacheStorage)
                CacheStorage = _dataRetriever();
        }
    }
}
