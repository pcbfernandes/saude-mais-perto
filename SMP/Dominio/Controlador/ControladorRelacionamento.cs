using SMP.Dominio.Model;

namespace SMP.Dominio.Controlador
{
	public class ControladorRelacionamento : ControladorBase
	{
		public List<RelacionamentoPessoaModel> ObterListaRelacionamento(string cpfResponsavel)
		{
			List<RelacionamentoPessoaModel> lista = new List<RelacionamentoPessoaModel>();

			if (!string.IsNullOrWhiteSpace(cpfResponsavel))
			{
				PessoaModel responsavel = _context.DbPessoas.FindOne(p => p.CPF == cpfResponsavel);

				RelacionamentoPessoaModel relacionamentoResponsavel = new RelacionamentoPessoaModel()
				{
					CpfResponsavel = responsavel?.CPF ?? cpfResponsavel,
					Cpf = responsavel?.CPF ?? cpfResponsavel,
					NomePessoa = string.IsNullOrWhiteSpace(responsavel?.Nome) ? "Ainda não cadastrado" : responsavel.Nome,
					Relacao = responsavel?.ResponsavelFamilia == true ? "Responsável pela família" : "Cadastro não indicado como responsável pela família",
				};

				lista.Add(relacionamentoResponsavel);

				List<PessoaModel> pessoas = _context.DbPessoas.Find(p => p.CpfResponsavel == cpfResponsavel).ToList();
				var parentescos = new ControladorDadosEsus().ObterOpcoesRelacaoParentesco();

				foreach (var pessoa in pessoas)
				{
					RelacionamentoPessoaModel relacionamento = new RelacionamentoPessoaModel()
					{
						CpfResponsavel = pessoa.CpfResponsavel,
						Cpf = pessoa.CPF,
						NomePessoa = pessoa.Nome
					};

					if (pessoa.CodRelacaoParentescoResponsavel.HasValue && parentescos.ContainsValue(pessoa.CodRelacaoParentescoResponsavel.Value))
					{
						relacionamento.Relacao = parentescos.FirstOrDefault(p => p.Value == pessoa.CodRelacaoParentescoResponsavel.Value).Key;
					}

					lista.Add(relacionamento);
				}
			}

			return lista;
		}

		public void ManterPessoaDadosFamilia(string cpf, string cpfResponsavel, long? codRelacaoParentescoResponsavel, bool desejaInformarResponsavel)
		{
			PessoaModel pessoa = _context.DbPessoas.FindOne(p => p.CPF == cpf);

			if (pessoa != null)
			{
				pessoa.CpfResponsavel = cpfResponsavel;
				pessoa.CodRelacaoParentescoResponsavel = codRelacaoParentescoResponsavel;
				pessoa.DesejaInformarResponsavelFamilia = desejaInformarResponsavel;
				_context.DbPessoas.Update(pessoa);
			}
		}
	}
}
