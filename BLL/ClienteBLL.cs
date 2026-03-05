using DAL;
using DTL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL
{
    public class ClienteBLL
    {
        private readonly ClienteDAL _dal;

        public ClienteBLL(ClienteDAL dal)
        {
            _dal = dal;
        }

        public Task<bool> MttoCliente(Cliente cliente, char accion) => _dal.MttoCliente(cliente, accion);
        public Task<List<Cliente>> ObtenerClientes() => _dal.ObtenerClientes();
        public Task<bool> Login(string correo, string password) => _dal.Login(correo, password);
        public Task<Cliente?> ObtenerClientePorCorreo(string correo) => _dal.ObtenerClientePorCorreo(correo);
    }
}
