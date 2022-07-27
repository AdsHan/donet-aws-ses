using AmazonSimpleEmail.API.Application.InputModels;
using AmazonSimpleEmail.API.Application.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace AmazonSimpleEmail.API.Controllers;

[Produces("application/json")]
[Route("api/auth")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _auth;
    private readonly IUserService _user;

    public AuthController(IAuthService auth, IUserService user)
    {
        _auth = auth;
        _user = user;
    }

    // POST api/auth/login
    /// <summary>
    /// Efetua o login do usuário
    /// </summary>   
    /// <remarks>
    /// Exemplo request:
    ///
    ///     USU: adshan@gmail.com SEN: 123456 (Este registro é criado automaticamente toda vez que a Api sobe)
    ///     
    /// 
    ///     POST Login
    ///     
    ///     {
    ///         "userName": "adshan",
    ///         "password": "123456" 
    ///     }
    ///     
    ///     POST Refresh Token
    ///
    ///     {
    ///         "userName": "adshan",
    ///         "refreshToken": "9949ad3f35f444fbb7a8fc28334edf79"
    ///     }        
    ///
    /// </remarks>                
    /// <response code="200">Foi realizado o login corretamente</response>                
    /// <response code="400">Falha na requisição</response>         
    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SignInAsync([FromBody] SignInAuthInputModel input)
    {

        var result = await _auth.SignInAsync(input);

        if (!result.IsValid())
        {
            ModelState.AddModelError("Messages", result.GetErrorsMessages());
            return BadRequest(ModelState);
        }

        return Ok(result.Response);
    }

    // POST api/auth/create-user
    /// <summary>
    /// Criação de um novo usuário
    /// </summary>   
    /// <remarks>
    /// Exemplo request:
    ///    
    ///     POST Create User
    ///     
    ///     {
    ///         "userName": "adshan",
    ///         "email": "adshan@gmail.com",
    ///         "password": "123456" 
    ///     }     
    ///
    /// </remarks>               
    /// <response code="204">Usuário criado corretamente</response>                
    /// <response code="400">Falha na requisição</response>         
    [HttpPost("create-user")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateUserAsync([FromBody] CreateUserInputModel input)
    {
        var result = await _user.CreateUserAsync(input);

        if (!result.IsValid())
        {
            ModelState.AddModelError("Messages", result.GetErrorsMessages());
            return BadRequest(ModelState);
        }

        return Ok(new { message = $"Úsuário criado com sucesso, verifique seu e-mail para obter instruções de verificação! Token: {result.Response}" });
    }

    // POST api/auth/verify-email
    /// <summary>
    /// Confirmação de e-mail
    /// </summary>   
    /// <remarks>
    /// Exemplo request:
    ///    
    ///     POST Verify Email
    ///     
    ///     {
    ///         "token": "5d375c41ea7d4f44892945f7b7650465"
    ///     }     
    ///
    /// </remarks>                      
    /// <response code="204">Endereço de e-mail confirmado</response>                
    /// <response code="400">Falha na requisição</response>         
    [HttpPost("verify-email")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> VerifyEmailAsync([FromBody] VerifyEmailInputModel input)
    {
        var result = await _user.VerifyEmailAsync(input);

        if (!result.IsValid())
        {
            ModelState.AddModelError("Messages", result.GetErrorsMessages());
            return BadRequest(ModelState);
        }

        return Ok(new { message = "Verificação bem-sucedida, você já pode realizar o login!" });
    }

    // POST api/auth/forgot-password
    /// <summary>
    /// Recuperação de senha
    /// </summary>   
    /// <remarks>
    /// Exemplo request:
    ///    
    ///     POST Forgot Password
    ///     
    ///     {
    ///         "userName": "adshan"
    ///     }     
    ///
    /// </remarks>                      
    /// <response code="204">Enviado o e-mail de recuperação de senha</response>                
    /// <response code="400">Falha na requisição</response>         
    [HttpPost("forgot-password")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ForgotPasswordAsync([FromBody] ForgotPasswordInputModel input)
    {
        var result = await _user.ForgotPasswordAsync(input);

        if (!result.IsValid())
        {
            ModelState.AddModelError("Messages", result.GetErrorsMessages());
            return BadRequest(ModelState);
        }

        return Ok(new { message = $"Verifique seu e-mail para obter instruções de redefinição de senha! Token: {result.Response}" });
    }

    // POST api/auth/validate-email-token
    /// <summary>
    /// Validação do token de e-mail
    /// </summary>   
    /// <remarks>
    /// Exemplo request:
    ///    
    ///     POST Validate E-mail Token
    ///     
    ///     {
    ///         "token": "5d375c41ea7d4f44892945f7b7650465"
    ///     }     
    ///
    /// </remarks>                      
    /// <response code="204">O token de e-mail informado é válido</response>                
    /// <response code="400">Falha na requisição</response>         
    [HttpPost("validate-email-token")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ValidateResetTokenAsync([FromBody] ValidateResetTokenInputModel input)
    {
        var result = _user.ValidateResetTokenAsync(input);

        if (!result.IsValid())
        {
            ModelState.AddModelError("Messages", result.GetErrorsMessages());
            return BadRequest(ModelState);
        }

        return Ok(new { message = "O Token informado é válido!" });
    }

    // POST api/auth/reset-password
    /// <summary>
    /// Redefinição da senha
    /// </summary>   
    /// <remarks>
    /// Exemplo request:
    ///    
    ///     POST ResetPassword
    ///     
    ///     {
    ///         "token": "5d375c41ea7d4f44892945f7b7650465"
    ///         "password": "123456"
    ///         "confirmPassword": "123456"
    ///     }     
    ///
    /// </remarks>                      
    /// <response code="204">A senha foi redefinida com sucesso</response>                
    /// <response code="400">Falha na requisição</response>         
    [HttpPost("reset-password")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ResetPasswordAsync([FromBody] ResetPasswordInputModel input)
    {
        var result = await _user.ResetPasswordAsync(input);

        if (!result.IsValid())
        {
            ModelState.AddModelError("Messages", result.GetErrorsMessages());
            return BadRequest(ModelState);
        }

        return Ok(new { message = "Senha redefinida, você já pode realizar o login!" });
    }

}

