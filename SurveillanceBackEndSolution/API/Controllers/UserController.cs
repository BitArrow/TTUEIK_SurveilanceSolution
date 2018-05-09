using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Core.DTO;
using Core.DTO.Constants;
using Core.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Newtonsoft.Json.Linq;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace API.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [Authorize]
    public class UserController : Controller
    {
        private IConfiguration _configuration;
        private IMapper _mapper;
        private readonly IUserService _userService;

        public UserController(IConfiguration configuration, IMapper mapper, IUserService userService)
        {
            _configuration = configuration;
            _mapper = mapper;
            _userService = userService;
        }

        [SwaggerResponse(statusCode: (int)HttpStatusCode.OK, type: typeof(UserDto), description: "Validate client's login")]
        [SwaggerResponse((int)ErrorsEnum.UserB2CMissing, type: typeof(ErrorDto), description: "User B2CId is missing")]
        [HttpGet]
        [Route("validate")]
        public async Task<IActionResult> Login()
        {
            var result = await _userService.ValidateUser();

            if (!result.Succeeded)
                return BadRequest(result.ErrorResult);
            
            return Ok(result.Content as UserDto);
        }

        [SwaggerResponse(statusCode: (int)HttpStatusCode.OK, type: typeof(UserDto), description: "Get my profile")]
        [SwaggerResponse((int)ErrorsEnum.UserB2CMissing, type: typeof(ErrorDto), description: "User B2CId is missing")]
        [HttpGet]
        [Route("me")]
        public async Task<IActionResult> Me()
        {
            var result = await _userService.GetUser();

            return Ok(_mapper.Map<UserDto>(result));
        }
    }
}