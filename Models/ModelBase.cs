using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

public abstract class ModelBase
{
	protected virtual string Get(string baseAddress, string route, string proxy = null)
	{
		var cookies = new CookieContainer();
		var handler = new HttpClientHandler
		{
			CookieContainer = cookies,
			UseCookies = true,
			UseDefaultCredentials = false
		};

		if (!string.IsNullOrEmpty(proxy))
		{
			handler.Proxy = new WebProxy(proxy, false, new string[0]);
			handler.UseProxy = true;
		}

		using (var client = new HttpClient(handler))
		{
			client.BaseAddress = new Uri(baseAddress);
			client.DefaultRequestHeaders.Accept.Clear();
			client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
			HttpResponseMessage clientResponse = client.GetAsync(route).Result;
			if (clientResponse.IsSuccessStatusCode)
			{
				return clientResponse.Content.ReadAsStringAsync().Result;
			}
			return null;
		}
	}

	protected virtual string Get(string id, Dictionary<string, string> cache, DateTime lastUpdated, int expirationMinutes)
	{
		string value = null;
		double totalMinutes = DateTime.Now.Subtract(lastUpdated).TotalMinutes;
		if (totalMinutes < (double)expirationMinutes)
		{
			cache.TryGetValue(id, out value);
		}
		return value;
	}

	protected virtual DateTime Set(string id, string value, Dictionary<string, string> cache)
	{
		lock (cache)
		{
			if (cache.Keys.Contains(id))
			{
				cache[id] = value;
			}
			else
			{
				cache.Add(id, value);
			}
		}
		return DateTime.Now;
	}
}
