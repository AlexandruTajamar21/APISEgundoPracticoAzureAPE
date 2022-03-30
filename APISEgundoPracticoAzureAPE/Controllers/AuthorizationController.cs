using APISEgundoPracticoAzureAPE.Helpers;
using APISEgundoPracticoAzureAPE.Model;
using APISEgundoPracticoAzureAPE.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace APISEgundoPracticoAzureAPE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorizationController : ControllerBase
    {
        private RepositoryUsuarios repo;
        private HelperOAuthToken helper;

        public AuthorizationController(RepositoryUsuarios repo
            , HelperOAuthToken helper)
        {
            this.repo = repo;
            this.helper = helper;
        }

        [HttpPost]
        [Route("[action]")]
        public ActionResult ValidarUsuario(LoginModel model)
        {

            Usuario usuario = this.repo.AutentificaUsuario(model.UserName, model.Password);

            if (usuario == null)
            {
                return Unauthorized();
            }
            else
            {
                SigningCredentials credentials =
                    new SigningCredentials(this.helper.GetKeyToken(), SecurityAlgorithms.HmacSha256);

                string jsonUsuario = JsonConvert.SerializeObject(usuario);
                Claim[] claims = new[]
                {
                    new Claim("UserData", jsonUsuario)
                };
                JwtSecurityToken token =
                    new JwtSecurityToken(
                        claims: claims,
                        issuer: this.helper.Issuer,
                        audience: this.helper.Audience,
                        signingCredentials: credentials,
                        expires: DateTime.UtcNow.AddMinutes(30),
                        notBefore: DateTime.UtcNow
                        );
                return Ok(
                    new
                    {
                        response =
                        new JwtSecurityTokenHandler().WriteToken(token)
                    });
            }
        }

        [HttpPost]
        [Route("[action]")]
        public ActionResult<Boolean> InsertUsuario(Usuario user)
        {
            this.repo.InsertUsuario(user.Nombre, user.Apellidos, user.Email, user.UserName,user.Password);
            return true;
        }
    }
}
