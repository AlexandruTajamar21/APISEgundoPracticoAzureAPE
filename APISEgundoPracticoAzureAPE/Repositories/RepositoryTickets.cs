using APISEgundoPracticoAzureAPE.Data;
using APISEgundoPracticoAzureAPE.Model;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace APISEgundoPracticoAzureAPE.Repositories
{
    public class RepositoryTickets
    {
        private AzureContext context;
        private MediaTypeWithQualityHeaderValue Header;

        public RepositoryTickets(AzureContext context)
        {
            this.context = context;
            this.Header = new MediaTypeWithQualityHeaderValue("application/json");
        }

        public List<Ticket> GetTicketsUsuario(int idUsuario)
        {
            return this.context.Tickets.Where(x => x.IdUsuario == idUsuario).ToList();
        }

        public Ticket GetTicketId(int id)
        {
            return this.context.Tickets.Where(x => x.IdTicket == id).SingleOrDefault();
        }

        public async Task<bool> InsertTicketAsync(int idUsuario, DateTime fecha, string importe, string producto, string fileName, string storagePath)
        {
            int id = this.GetTicketMaxId();
            string urlFlowInsert = "https://prod-60.westeurope.logic.azure.com:443/workflows/44f0a11e13b345478ce57e7d17809b21/triggers/manual/paths/invoke?api-version=2016-06-01&sp=%2Ftriggers%2Fmanual%2Frun&sv=1.0&sig=wRf0xQFaa71mWYZ3sep6u_LbJ2ryERadJ23JFy9DkFg";
            using (HttpClient client = new HttpClient())
            {
                Ticket ticket = new Ticket()
                {
                    IdTicket = id,
                    IdUsuario = idUsuario,
                    Fecha = fecha,
                    Importe = importe,
                    Producto = producto,
                    FileName = fileName,
                    StoragePath = storagePath
                };
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);
                string json = JsonConvert.SerializeObject(ticket);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(urlFlowInsert, content);
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public int GetTicketMaxId()
        {
            int id = 1;
            if (this.context.Tickets.Count() > 0)
            {
                int max = this.context.Tickets.Max(x => x.IdTicket);
                id = max + 1;
                return id;
            }
            else
            {
                return 1;
            }
        }
    }
}
