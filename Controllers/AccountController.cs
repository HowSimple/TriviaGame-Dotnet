using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace TriviaApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<AccountController> _logger;

        public AccountController(
            IConfiguration configuration,
            IHttpClientFactory httpClientFactory,
            ILogger<AccountController> logger)
        {
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        /// <summary>
        /// Test endpoint to verify Auth0 configuration
        /// </summary>
        [HttpGet("test-config")]
        public IActionResult TestConfig()
        {
            var domain = _configuration["Auth0:Domain"];
            var clientId = _configuration["Auth0:ClientId"];
            var dbConnection = _configuration["Auth0:DatabaseConnection"];
            var audience = _configuration["Auth0:Audience"];

            return Ok(new
            {
                domain = domain,
                clientId = clientId,
                connection = dbConnection,
                audience = audience,
                hasClientSecret = !string.IsNullOrEmpty(_configuration["Auth0:ClientSecret"])
            });
        }

        /// <summary>
        /// Register a new user with Auth0
        /// </summary>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            try
            {
                var domain = _configuration["Auth0:Domain"];
                var clientId = _configuration["Auth0:ClientId"];
                var dbConnection = _configuration["Auth0:DatabaseConnection"];

                // Log configuration (remove in production)
                _logger.LogInformation($"Attempting registration with domain: {domain}, clientId: {clientId}, connection: {dbConnection}");

                if (string.IsNullOrEmpty(domain) || string.IsNullOrEmpty(clientId))
                {
                    return BadRequest(new { message = "Auth0 configuration is missing. Check Domain and ClientId in appsettings.json" });
                }

                // If no connection specified, try to discover it
                if (string.IsNullOrEmpty(dbConnection))
                {
                    dbConnection = "Username-Password-Authentication";
                    _logger.LogWarning($"No DatabaseConnection configured, using default: {dbConnection}");
                }

                var client = _httpClientFactory.CreateClient();

                var signupData = new
                {
                    client_id = clientId,
                    email = request.Email,
                    password = request.Password,
                    connection = dbConnection,
                    given_name = request.FirstName,
                    family_name = request.LastName,
                    name = !string.IsNullOrEmpty(request.FirstName) || !string.IsNullOrEmpty(request.LastName)
                        ? $"{request.FirstName} {request.LastName}".Trim()
                        : null,
                    user_metadata = request.UserMetadata
                };

                var jsonContent = JsonSerializer.Serialize(signupData, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                });

                _logger.LogInformation($"Sending signup request to: https://{domain}/dbconnections/signup");

                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(
                    $"https://{domain}/dbconnections/signup",
                    content);

                var responseContent = await response.Content.ReadAsStringAsync();

                _logger.LogInformation($"Auth0 Response Status: {response.StatusCode}");
                _logger.LogInformation($"Auth0 Response: {responseContent}");

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError($"Auth0 registration failed with status {response.StatusCode}: {responseContent}");

                    try
                    {
                        var errorObj = JsonSerializer.Deserialize<JsonElement>(responseContent);

                        // Handle different error formats
                        string errorMessage = "Registration failed";

                        if (errorObj.TryGetProperty("message", out var msgElement))
                        {
                            errorMessage = msgElement.GetString() ?? errorMessage;
                        }
                        else if (errorObj.TryGetProperty("error", out var errElement))
                        {
                            errorMessage = errElement.GetString() ?? errorMessage;
                        }
                        else if (errorObj.TryGetProperty("description", out var descElement))
                        {
                            errorMessage = descElement.GetString() ?? errorMessage;
                        }

                        if (errorObj.TryGetProperty("statusCode", out var statusElement))
                        {
                            var statusCode = statusElement.GetInt32();
                            if (statusCode == 404)
                            {
                                errorMessage = $"Database connection '{dbConnection}' not found. Please check your Auth0 Dashboard under Authentication > Database for the exact connection name.";
                            }
                        }

                        return BadRequest(new
                        {
                            message = errorMessage,
                            debug = new
                            {
                                domain = domain,
                                connection = dbConnection,
                                statusCode = response.StatusCode,
                                rawResponse = responseContent
                            }
                        });
                    }
                    catch (Exception parseEx)
                    {
                        _logger.LogError(parseEx, "Failed to parse error response");
                    }

                    return BadRequest(new
                    {
                        message = "Registration failed. Check logs for details.",
                        statusCode = response.StatusCode,
                        details = responseContent
                    });
                }

                var result = JsonSerializer.Deserialize<Auth0UserResponse>(responseContent);
                return Ok(new
                {
                    message = "User registered successfully",
                    userId = result?.UserId,
                    email = result?.Email
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during user registration");
                return StatusCode(500, new { message = "An error occurred during registration", error = ex.Message });
            }
        }



        /// <summary>
        /// Login user and get access token
        /// </summary>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                var domain = _configuration["Auth0:Domain"];
                var clientId = _configuration["Auth0:ClientId"];
                var clientSecret = _configuration["Auth0:ClientSecret"];
                var audience = _configuration["Auth0:Audience"];
                var dbConnection = _configuration["Auth0:DatabaseConnection"] ?? "TriviaUsers";

                _logger.LogInformation($"Attempting login for user: {request.Email}");

                var client = _httpClientFactory.CreateClient();

                // Use http://auth0.com/oauth/grant-type/password-realm for database connections
                var loginData = new
                {
                    grant_type = "http://auth0.com/oauth/grant-type/password-realm",
                    client_id = clientId,
                    client_secret = clientSecret,
                    username = request.Email,
                    password = request.Password,
                    audience = !string.IsNullOrEmpty(audience) ? audience : $"https://{domain}/api/v2/",
                    scope = "openid profile email offline_access",
                    realm = dbConnection
                };

                var jsonContent = JsonSerializer.Serialize(loginData, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                });

                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                _logger.LogInformation($"Sending login request to: https://{domain}/oauth/token");

                var response = await client.PostAsync(
                    $"https://{domain}/oauth/token",
                    content);

                var responseContent = await response.Content.ReadAsStringAsync();

                _logger.LogInformation($"Auth0 Login Response Status: {response.StatusCode}");

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError($"Auth0 login failed: {responseContent}");

                    string errorMessage = "Login failed";
                    string errorDescription = "";

                    try
                    {
                        var errorObj = JsonSerializer.Deserialize<JsonElement>(responseContent);

                        if (errorObj.TryGetProperty("error", out var errElement))
                        {
                            errorMessage = errElement.GetString() ?? errorMessage;
                        }

                        if (errorObj.TryGetProperty("error_description", out var descElement))
                        {
                            errorDescription = descElement.GetString() ?? "";
                        }
                    }
                    catch (Exception parseEx)
                    {
                        _logger.LogError(parseEx, "Failed to parse error response");
                    }

                    return Unauthorized(new
                    {
                        message = errorMessage,
                        description = errorDescription,
                        debug = new
                        {
                            domain = domain,
                            connection = dbConnection,
                            hasAudience = !string.IsNullOrEmpty(audience),
                            statusCode = response.StatusCode,
                            rawResponse = responseContent
                        }
                    });
                }

                var result = JsonSerializer.Deserialize<TokenResponse>(responseContent);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during user login");
                return StatusCode(500, new { message = "An error occurred during login", error = ex.Message });
            }
        }

        /// <summary>
        /// Logout user (client-side token removal)
        /// </summary>
        [HttpPost("logout")]
        [Authorize]
        public IActionResult Logout()
        {
            // Auth0 logout is typically handled client-side by removing tokens
            // This endpoint can be used for server-side logging or cleanup
            var userId = User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
            _logger.LogInformation($"User {userId} logged out");

            var domain = _configuration["Auth0:Domain"];
            var clientId = _configuration["Auth0:ClientId"];
            var returnTo = _configuration["Auth0:LogoutReturnUrl"] ?? Request.Headers["Origin"].ToString();

            var logoutUrl = $"https://{domain}/v2/logout?client_id={clientId}&returnTo={Uri.EscapeDataString(returnTo)}";

            return Ok(new { message = "Logout successful", logoutUrl = logoutUrl });
        }

        /// <summary>
        /// Get current user profile
        /// </summary>
        [HttpGet("profile")]
        [Authorize]
        public async Task<IActionResult> GetProfile()
        {
            try
            {
                var domain = _configuration["Auth0:Domain"];
                var accessToken = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var response = await client.GetAsync($"https://{domain}/userinfo");

                if (!response.IsSuccessStatusCode)
                {
                    return Unauthorized(new { message = "Invalid token" });
                }

                var userInfo = await response.Content.ReadAsStringAsync();
                return Ok(JsonSerializer.Deserialize<object>(userInfo));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving user profile");
                return StatusCode(500, new { message = "An error occurred retrieving profile" });
            }
        }

        /// <summary>
        /// Refresh access token
        /// </summary>
        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            try
            {
                var domain = _configuration["Auth0:Domain"];
                var clientId = _configuration["Auth0:ClientId"];
                var clientSecret = _configuration["Auth0:ClientSecret"];

                var client = _httpClientFactory.CreateClient();
                var refreshData = new
                {
                    grant_type = "refresh_token",
                    client_id = clientId,
                    client_secret = clientSecret,
                    refresh_token = request.RefreshToken
                };

                var content = new StringContent(
                    JsonSerializer.Serialize(refreshData),
                    Encoding.UTF8,
                    "application/json");

                var response = await client.PostAsync(
                    $"https://{domain}/oauth/token",
                    content);

                var responseContent = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return Unauthorized(new { message = "Invalid refresh token" });
                }

                var result = JsonSerializer.Deserialize<TokenResponse>(responseContent);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error refreshing token");
                return StatusCode(500, new { message = "An error occurred refreshing token" });
            }
        }
    }

    // Request/Response Models
    public class RegisterRequest
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public Dictionary<string, object>? UserMetadata { get; set; }
    }

    public class LoginRequest
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class RefreshTokenRequest
    {
        public string RefreshToken { get; set; } = string.Empty;
    }

    public class TokenResponse
    {
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; } = string.Empty;

        [JsonPropertyName("refresh_token")]
        public string? RefreshToken { get; set; }

        [JsonPropertyName("id_token")]
        public string? IdToken { get; set; }

        [JsonPropertyName("token_type")]
        public string TokenType { get; set; } = string.Empty;

        [JsonPropertyName("expires_in")]
        public int ExpiresIn { get; set; }
    }

    public class Auth0UserResponse
    {
        [JsonPropertyName("_id")]
        public string UserId { get; set; } = string.Empty;

        [JsonPropertyName("email")]
        public string Email { get; set; } = string.Empty;

        [JsonPropertyName("email_verified")]
        public bool EmailVerified { get; set; }
    }

    public class Auth0ManagementUserResponse
    {
        [JsonPropertyName("user_id")]
        public string UserId { get; set; } = string.Empty;

        [JsonPropertyName("email")]
        public string Email { get; set; } = string.Empty;

        [JsonPropertyName("email_verified")]
        public bool EmailVerified { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
    }
}