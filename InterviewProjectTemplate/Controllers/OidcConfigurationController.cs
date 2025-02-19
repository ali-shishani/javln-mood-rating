using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InterviewProjectTemplate.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OidcConfigurationController : Controller
{
    private readonly ILogger<OidcConfigurationController> _logger;

    public OidcConfigurationController(
        IClientRequestParametersProvider clientRequestParametersProvider,
        ILogger<OidcConfigurationController> logger)
    {
        ClientRequestParametersProvider = clientRequestParametersProvider;
        _logger = logger;
    }

    public IClientRequestParametersProvider ClientRequestParametersProvider { get; }

    [AllowAnonymous]
    [HttpGet("_configuration/{clientId}")]
    public IActionResult GetClientRequestParameters([FromRoute]string clientId)
    {
        var parameters = ClientRequestParametersProvider.GetClientParameters(HttpContext, clientId);
        return Ok(parameters);
    }
}
