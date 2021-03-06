using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VUTTR.Domain.ViewModels;
using VUTTR.Service.Interfaces.Interfaces;

namespace VUTTR.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService service)
        {
            _userService = service;
        }

        [HttpGet("{UserId}")]
        [Authorize("Bearer")]
        public async Task<ActionResult<UserViewModel>> GetById([FromRoute] int UserId)
        {
            try
            {
                return Ok(await _userService.GetById(UserId, false));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("login")]
        [AllowAnonymousAttribute]
        public async Task<ActionResult<TokenViewModel>> Login([FromBody] UserViewModel user)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest("Modelo passado não é válido!");

                var token = await _userService.Login(user);

                if (token == null) return Unauthorized("Acesso negado!");
                return Ok(token);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("register")]
        [AllowAnonymousAttribute]
        public async Task<ActionResult<UserViewModel>> Register([FromBody] UserViewModel user)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest("Modelo passado não é válido!");

                return Ok(await _userService.Register(user));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut]
        [Authorize("Bearer")]
        public async Task<ActionResult<UserViewModel>> Update([FromBody] UserViewModel user)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest("Modelo passado não é válido!");

                return Ok(await _userService.Update(user));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

    }
}