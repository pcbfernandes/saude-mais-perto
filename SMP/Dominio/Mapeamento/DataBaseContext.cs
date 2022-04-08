using LiteDB;
using Newtonsoft.Json;
using SMP.Dominio.Model;

namespace SMP.Dominio.Mapeamento
{
	public sealed class DataBaseContext
	{
		static DataBaseContext _instancia;
		public static DataBaseContext Instancia
		{
			get { return _instancia ?? (_instancia = new DataBaseContext()); }
		}
		private DataBaseContext()
		{
			string dataBasePath = Utilitarios.ObterPastaTemporaria();
			string dataBaseFullPath = Path.Combine(dataBasePath, "MyData.db");
			LiteDatabase dataBase = new LiteDatabase(dataBaseFullPath);

			DbPessoas = dataBase.GetCollection<PessoaModel>("pessoas");
			DbMunicipios = dataBase.GetCollection<MunicipioModel>("municipiosESUS");
			DbOcupacaoSIGTAP = dataBase.GetCollection<OcupacaoModel>("ocupacoesSIGTAP");
			DbArquivo = dataBase.GetCollection<ArquivoModel>("arquivo");
			DbArquivoDados = dataBase.GetCollection<ArquivoDadosModel>("arquivoDados");
			DbUnidadeSaude = dataBase.GetCollection<UnidadeSaudeModel>("unidadeSaude");
			DbValidacaoPessoa = dataBase.GetCollection<ValidacaoPessoaModel>("validacaoPessoa");

			if (DbMunicipios.Count() == 0)
			{
				var assembly = System.Reflection.Assembly.GetExecutingAssembly();
				var resourceName = "SMP._resources.MunicipiosESUS.txt";

				using (Stream? stream = assembly.GetManifestResourceStream(resourceName))
				{
					using (StreamReader reader = new StreamReader(stream))
					{
						string json = reader.ReadToEnd();
						var lista = JsonConvert.DeserializeObject<List<MunicipioModel>>(json);
						DbMunicipios.InsertBulk(lista);
					}
				}
			}

			if (DbOcupacaoSIGTAP.Count() == 0)
			{
				var assembly = System.Reflection.Assembly.GetExecutingAssembly();
				var resourceName = "SMP._resources.OcupacaoSIGTAP.txt";

				using (Stream? stream = assembly.GetManifestResourceStream(resourceName))
				{
					using (StreamReader reader = new StreamReader(stream))
					{
						string json = reader.ReadToEnd();

						var lista = JsonConvert.DeserializeObject<List<OcupacaoModel>>(json);
						DbOcupacaoSIGTAP.InsertBulk(lista);
					}
				}
			}

			if (DbUnidadeSaude.Count() == 0)
			{
				var assembly = System.Reflection.Assembly.GetExecutingAssembly();
				var resourceName = "SMP._resources.UnidadeSaude.txt";

				using (Stream? stream = assembly.GetManifestResourceStream(resourceName))
				{
					using (StreamReader reader = new StreamReader(stream))
					{
						string json = reader.ReadToEnd();

						var lista = JsonConvert.DeserializeObject<List<UnidadeSaudeModel>>(json);
						DbUnidadeSaude.InsertBulk(lista);
					}
				}
			}

		}
		public ILiteCollection<PessoaModel> DbPessoas { get; set; }
		public ILiteCollection<MunicipioModel> DbMunicipios { get; set; }
		public ILiteCollection<OcupacaoModel> DbOcupacaoSIGTAP { get; set; }
		public ILiteCollection<ArquivoModel> DbArquivo { get; set; }
		public ILiteCollection<ArquivoDadosModel> DbArquivoDados { get; set; }
		public ILiteCollection<UnidadeSaudeModel> DbUnidadeSaude { get; set; }
		public ILiteCollection<ValidacaoPessoaModel> DbValidacaoPessoa { get; set; }
	}
}
