using APISEgundoPracticoAzureAPE.Model;
using APISEgundoPracticoAzureAPE.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace APISEgundoPracticoAzureAPE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmpresaController : ControllerBase
    {

        private RepositoryTickets repotickets;

        public EmpresaController(RepositoryTickets repotickets)
        {
            this.repotickets = repotickets;
        }

        [HttpGet]
        [Authorize]
        [Route("[action]")]
        public ActionResult<List<Ticket>> TicketsUsuario()
        {
            List<Claim> claims = HttpContext.User.Claims.ToList();
            string jsonUsuario = claims.SingleOrDefault(z => z.Type == "UserData").Value;
            Usuario usuario = JsonConvert.DeserializeObject<Usuario>(jsonUsuario);
            return this.repotickets.GetTicketsUsuario(usuario.IdUsuario);
        }

        [HttpGet]
        [Authorize]
        [Route("[action]/{id}")]
        public ActionResult<Ticket> FindTicket(int id)
        {
            return this.repotickets.GetTicketId(id);
        }

        [HttpPost]
        [Authorize]
        [Route("[action]")]
        public async Task<ActionResult<Boolean>> CreateTicketAsync(Ticket ticket)
        {
            List<Claim> claims = HttpContext.User.Claims.ToList();
            string jsonUsuario = claims.SingleOrDefault(z => z.Type == "UserData").Value;
            Usuario usuario = JsonConvert.DeserializeObject<Usuario>(jsonUsuario);
            if (await this.repotickets.InsertTicketAsync(usuario.IdUsuario, ticket.Fecha, ticket.Importe, ticket.Producto, ticket.FileName, ticket.StoragePath))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        [HttpPost]
        [Authorize]
        [Route("[action]")]
        public ActionResult<Boolean> ProcessTicket(Ticket ticket)
        {
            return false;
        }
    }
}
