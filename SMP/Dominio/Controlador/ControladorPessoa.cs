using LiteDB;
using Newtonsoft.Json;
using SMP.Dominio.Model;
using System.Diagnostics;

namespace SMP.Dominio.Controlador
{
	public class ControladorPessoa : ControladorBase
	{
		public async Task<PessoaModel> ObterPessoaPeloCPF(string? cpf)
		{
			if (Debugger.IsAttached)
			{
				await Task.Delay(3000);
			}

			PessoaModel pessoa = _context.DbPessoas.FindOne(p => p.CPF == cpf);

			return pessoa;
		}

		public void ManterPessoa(PessoaModel pessoaModel)
		{
			ValidarDadosPessoa(pessoaModel);
			pessoaModel.SetAttributesToUpper();
			PessoaModel pessoa = _context.DbPessoas.FindOne(p => p.CPF == pessoaModel.CPF);

			if (pessoa == null)
			{
				pessoaModel.GuidPessoa = Guid.NewGuid().ToString();
				pessoaModel.DataCriacao = DateTime.Now;
				_context.DbPessoas.Insert(pessoaModel);
			}
			else
			{
				pessoa = pessoaModel;
				pessoa.DataUltimaAtualizaco = DateTime.Now;
				_context.DbPessoas.Update(pessoa);
			}
		}

		public Dictionary<string, List<string>> ValidarDadosPessoa(PessoaModel pessoaModel)
		{
			Dictionary<string, List<string>> erros = new Dictionary<string, List<string>>();

			if (pessoaModel != null)
			{
				if (!string.IsNullOrWhiteSpace(pessoaModel.Nome))
				{
					try
					{
						pessoaModel.Nome = Utilitarios.ValidarNome(pessoaModel.Nome);
					}
					catch (Exception erro)
					{
						erros.Add(nameof(pessoaModel.Nome), new() { erro.Message });
					}
				}

				if (!string.IsNullOrWhiteSpace(pessoaModel.CpfResponsavel))
				{
					if (Utilitarios.ValidarCPF(pessoaModel.CpfResponsavel))
					{
						erros.Add(nameof(pessoaModel.CpfResponsavel), new() { "O CPF informado não é válido." });
					}
				}

				if (!string.IsNullOrWhiteSpace(pessoaModel.NomeMae))
				{
					try
					{
						pessoaModel.NomeMae = Utilitarios.ValidarNome(pessoaModel.NomeMae);
					}
					catch (Exception erro)
					{
						erros.Add(nameof(pessoaModel.NomeMae), new() { erro.Message });
					}
				}

				if (!string.IsNullOrWhiteSpace(pessoaModel.NomePai))
				{
					try
					{
						pessoaModel.NomePai = Utilitarios.ValidarNome(pessoaModel.NomePai);
					}
					catch (Exception erro)
					{
						erros.Add(nameof(pessoaModel.NomePai), new() { erro.Message });
					}
				}
			}

			return erros;
		}

		public List<PessoaModel> ObterListaPessoas()
		{
			List<PessoaModel> pessoas = _context.DbPessoas.FindAll().ToList();

			if (pessoas.Any())
			{
				List<ArquivoModel> arquivos = _context.DbArquivo.Find(a => pessoas.Select(p => p.CPF).Contains(a.PessoaCPF) && !a.DataExclusao.HasValue).ToList();
				List<ResultadoValidacaoPessoaModel> validacoes = new ControladorValidacaoPessoa().ObterResultadoValidacaoGeral();

				foreach (var pessoa in pessoas)
				{
					pessoa.ModelArquivo = arquivos.FirstOrDefault(a => a.PessoaCPF == pessoa.CPF);
					pessoa.ModelResultadoValidacaoPessoa = validacoes.FirstOrDefault(v=>v.ListaValidacaoPessoa.Any(p=>p.CPF == pessoa.CPF));
				}				
			}

			return pessoas.ToList();
		}

	}
}
