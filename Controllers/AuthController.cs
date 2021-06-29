using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Data;
using Api.Dtos;
using Api.Helpers;
using Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route(template:"api")] //Routing of api
    [ApiController] //Differentiating api from mvc
    public class AuthController : ControllerBase //Adding repository and jwtservice
    {
        private readonly IUserRepository _repository;
        private readonly JwtService _jwtService;

        public AuthController(IUserRepository repository, JwtService jwtService)
        {
            _repository = repository;
            _jwtService = jwtService;
        }

        [HttpPost(template: "register")]
        public IActionResult Register(RegisterDto dto) //Creating a user using user repository
        {
            var user = new AppUser
            {
                Name = dto.Name,
                Email = dto.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(dto.Password) //Encrypting password
            };

            return Created("success", _repository.Create(user)); //Success message by returning created user
        }

        [HttpPost(template:"login")]
        public IActionResult Login(LoginDto dto) //Verifying and logging in user
        {
            var user = _repository.GetByEmail(dto.Email); //Getting user email

            if (user == null) return BadRequest(new { message = "Invalid Credentials" }); //Checking if user is null

            if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.Password))//Checking if the encrypted password matches the hash in the database
            {
                return BadRequest(new { message = "Invalid Credentials" }); //Error message if not matched
            }

            var jwt = _jwtService.Generate(user.Id); //Generating jwt token

            Response.Cookies.Append("jwt", jwt, new CookieOptions //Adding the jwt token to the cookies
            {
                HttpOnly = true,

            });

            return Ok(new //Success message
            { 
                message = "success"
            });
        }

        [HttpGet(template:"user")] //Finding user
        public IActionResult User()
        {
            try
            {
                var jwt = Request.Cookies["jwt"]; //Request cookies for jwt
                var token = _jwtService.Verify(jwt); //Verify if the token matches

                int userId = int.Parse(token.Issuer); //Getting the id from issuer

                var user = _repository.GetById(userId); //Getting user's id

                return Ok(user);
            }
            catch(Exception e)
            {
                return Unauthorized();
            }
            
        }

        [HttpPost(template:"logout")] //Removing the token from the cookie
        public IActionResult Logout()
        {
            Response.Cookies.Delete("jwt");

            return Ok(new //Success message
            {
                message = "success"
            });
        }
    }
}
