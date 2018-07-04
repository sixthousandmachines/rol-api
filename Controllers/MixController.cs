using System.Net;
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
  public class MixController : Controller
  {
    private readonly AppSettings config;

    public MixController(IOptions<AppSettings> appSettings)
    {
      config = appSettings.Value;
    }

    [HttpGet("{id}")]
    public JsonResult Get(string id)
    {
      var model = new Mix(id, config);
			return Json(JObject.Parse(model.Get()));
    }
  }
}