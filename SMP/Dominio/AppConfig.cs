using Newtonsoft.Json;

namespace SMP.Dominio
{
	public class AppConfig
	{
		private static string _credenciaisApi;
		private static bool _desabilitarBuscaCep;
		private static bool _desabilitarBuscaDadosCpf;
		private static bool _desabilitarProcessamentoArquivo;
		private static Usuario _administrador;
		public static string CredenciaisApi { get { return _credenciaisApi; } }
		public static bool DesabilitarBuscaCep { get { return _desabilitarBuscaCep; } }
		public static bool DesabilitarBuscaDadosCpf { get { return _desabilitarBuscaDadosCpf; } }
		public static bool DesabilitarProcessamentoArquivo { get { return _desabilitarProcessamentoArquivo; } }
		public static Usuario Administrador { get { return _administrador; } }

		private static AppConfig _instance;
		private static Object _mutex = new Object();
		public AppConfig(string credenciaisApi, bool desabilitarBuscaCep, bool desabilitarBuscaDadosCpf, bool desabilitarProcessamentoArquivo, Usuario administrador)
		{
			_credenciaisApi = credenciaisApi;
			_desabilitarBuscaCep = desabilitarBuscaCep;
			_desabilitarBuscaDadosCpf = desabilitarBuscaDadosCpf;
			_desabilitarProcessamentoArquivo = desabilitarProcessamentoArquivo;
			_administrador = administrador;
		}

		public static AppConfig GetInstance(AppConfigModel model)
		{
			if (_instance == null)
			{
				lock (_mutex)
				{
					if (_instance == null)
					{
						_instance = new AppConfig(model.CredenciaisApi, model.DesabilitarBuscaCep, model.DesabilitarBuscaDadosCpf, model.DesabilitarProcessamentoArquivo, model.Administrador);
					}
				}
			}

			return _instance;
		}
	}

	public class AppConfigModel
	{
		public string CredenciaisApi { get; set; }
		public bool DesabilitarBuscaCep { get; set; }
		public bool DesabilitarBuscaDadosCpf { get; set; }
		public bool DesabilitarProcessamentoArquivo { get; set; }
		public Usuario? Administrador { get; set; }
	}

	public class Usuario
	{
		public string Nome { get; set; }
		public string Login { get; set; }
		public string Senha { get; set; }
	}
}
