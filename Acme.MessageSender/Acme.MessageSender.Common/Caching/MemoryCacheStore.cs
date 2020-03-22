using Microsoft.Extensions.Caching.Memory;
using System;

namespace Acme.MessageSender.Common.Caching
{
	public class MemoryCacheStore : ICacheStore
	{
		private IMemoryCache _memoryCache;

		public MemoryCacheStore(IMemoryCache memoryCache)
		{
			_memoryCache = memoryCache;
		}

		public void Add(string cacheKey, object item, TimeSpan cacheLifetime)
		{
			_memoryCache.Set(cacheKey, item, cacheLifetime);
		}

		public TItem Get<TItem>(string cacheKey) where TItem : class
		{
			_memoryCache.TryGetValue(cacheKey, out TItem value);
			return value;
		}
	}
}
