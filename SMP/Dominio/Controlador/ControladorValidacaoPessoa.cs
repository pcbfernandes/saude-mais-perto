using SMP.Dominio.Model;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace SMP.Dominio.Controlador
{
	public class ControladorValidacaoPessoa : ControladorBase
	{
		public void SalvarValidacao(string cpf, List<ValidacaoPessoaModel> validacoes)
		{
			List<ValidacaoPessoaModel> existente = _context.DbValidacaoPessoa.Find(v => v.CPF == cpf && !v.DataVisualizacao.HasValue && !v.DataExclusao.HasValue).ToList();

			List<ValidacaoPessoaModel> excluir = existente.Where(e => !validacoes.Any(v => v.Id == e.Id)).ToList();
			List<ValidacaoPessoaModel> adicionar = validacoes.Where(v => !existente.Any(e => e.Id == v.Id)).ToList();

			if (excluir?.Any() == true)
			{
				foreach (var item in excluir)
				{
					item.DataExclusao = DateTime.Now;
					//_context.DbValidacaoPessoa.Delete(item.Id);
				}

				_context.DbValidacaoPessoa.Update(excluir);
			}

			foreach (var item in adicionar)
			{
				item.DataCriacao = DateTime.Now;
			}

			_context.DbValidacaoPessoa.InsertBulk(adicionar);
		}

		public void AtualizarValidacaoPessoa(string cpf)
		{
			var itensPendentes = _context.DbValidacaoPessoa.Find(v => v.CPF == cpf && !v.PessoaValidada && !v.DataVisualizacao.HasValue && !v.DataExclusao.HasValue);

			if (itensPendentes?.Any() == true)
			{
				foreach (var item in itensPendentes)
				{
					item.DataVisualizacao = DateTime.Now;
					_context.DbValidacaoPessoa.Update(item);
				}
			}
		}
		private List<ValidacaoPessoaModel> ObterListaValidacao(string cpf)
		{
			List<ValidacaoPessoaModel> retorno = _context.DbValidacaoPessoa.Find(v => v.CPF == cpf).OrderByDescending(v => v.DataCriacao).ToList();
			return retorno;
		}

		public List<ValidacaoPessoaModel> ObterListaUltimaValidacao()
		{
			List<ValidacaoPessoaModel> retorno = new List<ValidacaoPessoaModel>();
			List<ValidacaoPessoaModel> listaCompleta = _context.DbValidacaoPessoa.FindAll().OrderByDescending(v => v.DataCriacao).ToList();
			foreach (var item in listaCompleta)
			{
				if (!retorno.Any(v => v.CPF == item.CPF))
				{
					retorno.Add(item);
				}
			}

			return retorno;
		}

		public List<ResultadoValidacaoPessoaModel> ObterResultadoValidacaoGeral()
		{
			List<ResultadoValidacaoPessoaModel> retorno = new List<ResultadoValidacaoPessoaModel>();
			List<ValidacaoPessoaModel> listaUltimaValidacao = new List<ValidacaoPessoaModel>();
			List<ValidacaoPessoaModel> listaCompleta = _context.DbValidacaoPessoa.FindAll().OrderByDescending(v => v.DataCriacao).ToList();

			foreach (var item in listaCompleta)
			{
				if (!listaUltimaValidacao.Any(v => v.CPF == item.CPF))
				{
					listaUltimaValidacao.Add(item);
				}
			}

			foreach (var item in listaUltimaValidacao)
			{
				var resultado = new ResultadoValidacaoPessoaModel
				{
					ListaValidacaoPessoa = new List<ValidacaoPessoaModel>() { item },
					EstaValido = null,
					ResultadoValidacao = "O cadastro foi alterado e ainda não foi validado"
				};
				retorno.Add(ProcessarResultado(resultado));
			}

			return retorno;
		}

		private static ResultadoValidacaoPessoaModel ProcessarResultado(ResultadoValidacaoPessoaModel resultado)
		{
			if (resultado.ListaValidacaoPessoa?.Any() == true)
			{
				var itemMaisRecente = resultado.ListaValidacaoPessoa.First();

				if (itemMaisRecente.PessoaValidada)
				{
					resultado.EstaValido = true;
					resultado.ResultadoValidacao = "Cadastro validado!";
				}
				else
				{
					if (!itemMaisRecente.DataVisualizacao.HasValue)
					{
						resultado.EstaValido = false;
						resultado.ResultadoValidacao = "O cadastro foi invalidado. Atualize os dados do cadastro.";
					}
				}
			}

			return resultado;
		}

		public ResultadoValidacaoPessoaModel ObterResultadoValidacaoPessoa(string cpf, bool considerarExcluidos)
		{
			ResultadoValidacaoPessoaModel retorno = new ResultadoValidacaoPessoaModel
			{
				ListaValidacaoPessoa = ObterListaValidacao(cpf),
				EstaValido = null,
				ResultadoValidacao = "O cadastro foi alterado e ainda não foi validado"
			};

			retorno = ProcessarResultado(retorno);

			if (retorno.ListaValidacaoPessoa?.Any() == true)
			{
				if (!considerarExcluidos)
				{
					retorno.ListaValidacaoPessoa.RemoveAll(v => v.DataVisualizacao.HasValue || v.DataExclusao.HasValue);
				}
			}

			return retorno;
		}

		public Dictionary<string, string> ObterOpcoesAtributos()
		{
			Dictionary<string, string> retorno = new Dictionary<string, string>();

			List<PropertyInfo> properties = typeof(DadosCadastraisModel).GetProperties().Where(prop => prop.IsDefined(typeof(ESUSAttribute), false)).ToList();
			properties.AddRange(typeof(DadosSocioDemograficosModel).GetProperties().Where(prop => prop.IsDefined(typeof(ESUSAttribute), false)).ToList());
			properties.AddRange(typeof(DadosCondicoesSaudeModel).GetProperties().Where(prop => prop.IsDefined(typeof(ESUSAttribute), false)).ToList());
			properties.AddRange(typeof(DadosFamiliaModel).GetProperties().Where(prop => prop.IsDefined(typeof(ESUSAttribute), false)).ToList());

			foreach (var item in properties)
			{
				if (!retorno.ContainsKey(item.Name))
				{
					if (item.GetCustomAttributes(typeof(DisplayAttribute), true).Any())
					{
						retorno[item.Name] = ((DisplayAttribute)(item
						.GetCustomAttributes(typeof(DisplayAttribute), true)[0])).Name;
					}
				}
			}

			return retorno;
		}
	}
}
