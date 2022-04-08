using SMP.Dominio.Model;

namespace SMP.Dominio.Controlador
{
	public class ControladorSigtap : ControladorBase
	{
		public List<OcupacaoModel>? ObterOpcoesOcupacao()
		{
			IEnumerable<OcupacaoModel> ocupacoes = _context.DbOcupacaoSIGTAP.FindAll();

			return ocupacoes.OrderBy(o => o.Descricao).ToList();
		}
	}
}
