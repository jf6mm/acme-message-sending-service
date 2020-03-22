using System;

namespace Acme.MessageSender.Common.Caching
{
	public interface ICacheStore
	{
		void Add(string cacheKey, object item, TimeSpan cacheLifetime);

		TItem Get<TItem>(string cacheKey) where TItem : class;
	}
}
