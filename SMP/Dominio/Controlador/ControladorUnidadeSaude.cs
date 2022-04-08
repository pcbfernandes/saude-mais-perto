using SMP.Dominio.Model;

namespace SMP.Dominio.Controlador
{
	public class ControladorUnidadeSaude : ControladorBase
	{
		public List<UnidadeSaudeModel> ObterListaUnidadeSaude()
		{
			IEnumerable<UnidadeSaudeModel> unidades = _context.DbUnidadeSaude.FindAll();
			return unidades.ToList();
		}

		public UnidadeSaudeModel ObterUnidadeSaude(string cnes)
		{
			return _context.DbUnidadeSaude.FindOne(u => u.CNES == cnes);
		}
		public string ObterDescricoUnidadeSaude(string cnes)
		{
			return _context.DbUnidadeSaude.FindOne(u => u.CNES == cnes)?.Descricao;
		}

		public async Task<UnidadeSaudeModel?> DescobrirUnidadeSaude(string cep)
		{
			UnidadeSaudeModel unidade = _context.DbUnidadeSaude.FindOne(u => u.ListaCepCobertura.Contains(cep));

			if (unidade == null)
			{
				await Task.Run(() =>
				{
					List<UnidadeSaudeModel> lista = ObterListaUnidadeSaude().Where(u => u.ListaCepCobertura.Any(c => c.Contains("*"))).ToList();

					foreach (var us in lista)
					{
						foreach (var item in us.ListaCepCobertura.Where(c => c.Contains("*")))
						{
							if (cep.Contains(item.Replace("*", "")))
							{
								unidade = us;
								break;
							}
						}

						if (unidade != null)
						{
							break;
						}
					}
				});
			}

			return unidade;
		}
	}
}
