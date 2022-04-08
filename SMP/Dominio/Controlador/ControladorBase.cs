using LiteDB;
using SMP.Dominio.Mapeamento;
using SMP.Dominio.Model;

namespace SMP.Dominio.Controlador
{
    public partial class ControladorBase
    {
        public DataBaseContext _context;
        public ControladorBase()
        {
            _context = DataBaseContext.Instancia;
        }
    }
}
