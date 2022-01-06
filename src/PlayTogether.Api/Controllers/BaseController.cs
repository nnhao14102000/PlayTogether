using Microsoft.AspNetCore.Mvc;
using PlayTogether.Api.Helpers;

namespace PlayTogether.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/" + ApiConstants.ServiceName + "/v{api-version:apiVersion}/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        
    }
}
