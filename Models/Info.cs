using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;

namespace rol_api.Models
{
  public class Info : ModelBase
  {
    private static Dictionary<string, string> cache = new Dictionary<string, string>();

    private static DateTime lastUpdated = DateTime.MinValue;

    private static int expirationMinutes = 30;

    private readonly string baseAddress;

    private readonly string proxy;

    private string route;

    private string id;

    public Info(string id, AppSettings config)
    {
      this.id = id;
      route = $"{id}?metadata=1";
      baseAddress = config.MixCloudBaseAddress;
      proxy = config.ApiProxy;
    }

    public string Get()
    {
      string payload2 = base.Get(id, cache, lastUpdated, expirationMinutes);
      if (!string.IsNullOrEmpty(payload2))
      {
        return payload2;
      }
      payload2 = base.Get(baseAddress, route, proxy);
      lastUpdated = base.Set(id, payload2, cache);
      return payload2;
    }
  }
}