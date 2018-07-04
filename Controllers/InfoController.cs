using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using rol_api.Models;

namespace rol_api.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class InfoController : Controller
  {
    private readonly AppSettings config;

    public InfoController(IOptions<AppSettings> appSettings)
    {
      config = appSettings.Value;
    }

    [HttpGet]
    public IEnumerable<string> Get()
    {
      return config.DJs.Split(new char[1]
      {
      ','
      }, StringSplitOptions.RemoveEmptyEntries);
    }

    [HttpGet("{id}")]
    public JsonResult Get(string id)
    {
			var model = new Info(id, config);
			return Json(JObject.Parse(model.Get()));
    }
  }
}
