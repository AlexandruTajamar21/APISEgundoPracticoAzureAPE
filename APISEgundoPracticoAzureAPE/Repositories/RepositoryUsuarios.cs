using APISEgundoPracticoAzureAPE.Data;
using APISEgundoPracticoAzureAPE.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APISEgundoPracticoAzureAPE.Repositories
{
    public class RepositoryUsuarios
    {
        private AzureContext context;

        public RepositoryUsuarios(AzureContext context)
        {
            this.context = context;
        }

        public Usuario AutentificaUsuario(string username, string password)
        {
            var consulta = from datos in this.context.Usuarios
                           where datos.UserName == username
                           && datos.Password == password
                           select datos;
            if (consulta.Count() == 0)
            {
                return null;
            }
            else
            {
                return consulta.First();
            }
        }

        public void InsertUsuario(string nombre, string apellidos, string email, string userName, string password)
        {
            int id = this.GetMaxId();
            Usuario usuario = new Usuario()
            {
                IdUsuario = id,
                Nombre = nombre,
                Apellidos = apellidos,
                Email = email,
                UserName = userName,
                Password = password
            };
            this.context.Add(usuario);
            this.context.SaveChanges();
        }
        public int GetMaxId()
        {
            int id = 1;
            if (this.context.Usuarios.Count() > 0)
            {
                int max = this.context.Usuarios.Max(x => x.IdUsuario);
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
