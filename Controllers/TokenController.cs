using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AuthApiDemo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TokenController : ControllerBase
    {

        private readonly ILogger<TokenController> _logger;
        private readonly ITokenProvider TokenProvider;
        public TokenController(ILogger<TokenController> logger , ITokenProvider tokenProvider)
        {
            _logger = logger;
            this.TokenProvider = tokenProvider;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Running");
        }

        [HttpPost]
        public async Task<ActionResult<TokenResponse>> Post([FromForm] TokenRequest request)
        {
            var response = await TokenProvider.GetToken(request);

            if (!string.IsNullOrEmpty(response.Error))
            {
                return new BadRequestObjectResult(response);
            }

            return response;
        }

        [HttpGet("Test")]
        [Authorize]
        public IActionResult Get1()
        {
            return Ok("Running Authorize");
        }

        [HttpGet("1")]
        public async Task<IActionResult> Get2()
        {
            // discover endpoints from metadata
            var client = new HttpClient();
            var disco = await client.GetDiscoveryDocumentAsync("https://localhost:5001");
            if (disco.IsError)
            {
                return BadRequest();
            }

            var tokenResponse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = disco.TokenEndpoint,

                ClientId = "client_credential_flow",
                ClientSecret = "client_credential_flow_secret",
                Scope = "api"
            });

            if (tokenResponse.IsError)
            {
                return BadRequest(tokenResponse.Error);
            }

            return Ok(tokenResponse.Json);
        }
    }
}
