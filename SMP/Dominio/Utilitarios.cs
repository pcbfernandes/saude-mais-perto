using SMP.Dominio.Controlador;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Reflection;
using System.Text;

namespace SMP.Dominio
{
	public static class Utilitarios
	{
		public static string ObterPastaTemporaria()
		{
			string appPath = AppDomain.CurrentDomain.BaseDirectory;
			string tempPath = Path.Combine(appPath, "Temp");

			if (!(Directory.Exists(tempPath)))
			{
				Directory.CreateDirectory(tempPath);
			}

			return tempPath;
		}
		public static Tipo? ConverterPara<Tipo>(object objeto) where Tipo : class
		{
			if (objeto == null)
				return null;

			object novoObjeto = Activator.CreateInstance(typeof(Tipo));

			foreach (PropertyInfo itemNovoObjeto in novoObjeto.GetType().GetProperties())
			{
				foreach (PropertyInfo itemObjeto in objeto.GetType().GetProperties())
				{
					if (itemObjeto.Name.ToUpper() == itemNovoObjeto.Name.ToUpper())
						itemNovoObjeto.SetValue(novoObjeto, itemObjeto.GetValue(objeto, null), null);
				}
			}

			return novoObjeto as Tipo;
		}

		public static void CompletarDados<Tipo>(this Tipo objeto, object dados) where Tipo : class
		{
			if (objeto == null || dados == null)
			{
				return;
			}

			foreach (PropertyInfo infoDados in dados.GetType().GetProperties())
			{
				foreach (PropertyInfo infoObjeto in objeto.GetType().GetProperties())
				{
					if (infoDados.Name.ToUpper() == infoObjeto.Name.ToUpper())
						infoObjeto.SetValue(objeto, infoDados.GetValue(dados, null), null);
				}
			}
		}

		public static string GetErrorMessageFrom(this object instance, string propertyName)
		{
			var property = instance.GetType().GetProperty(propertyName);
			var mensagem = property.GetCustomAttributes(true).OfType<RequiredAttribute>().FirstOrDefault()?.ErrorMessage;

			if (string.IsNullOrWhiteSpace(mensagem))
			{
				var attributes = (ConditionalAttribute[])property.GetCustomAttributes(typeof(ConditionalAttribute), false);
				foreach (var attribute in attributes)
				{
					mensagem = attribute.Message;
				}
			}

			return mensagem;
		}
		public static string GetDisplayAttributeFrom(this object instance, string propertyName)
		{
			return ((DisplayAttribute)
				(instance
				.GetType()
				.GetProperty(propertyName)
				.GetCustomAttributes(typeof(DisplayAttribute), true)[0])).Name;
		}
		public static bool GetIsRequired(this object instance, string propertyName)
		{

			var retorno = instance.GetType().GetRuntimeProperties()
				.Where(pi => pi.Name == propertyName
				&& pi.GetCustomAttributes<RequiredAttribute>(true).Any()).Any();

			return retorno;
		}

		public static void SetAttributesToUpper(this object objeto)
		{
			if (objeto != null)
			{
				PropertyInfo[] properties = objeto.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

				foreach (PropertyInfo p in properties)
				{
					if (p.PropertyType == typeof(string) && p.CanWrite && p.CanRead)
					{
						var value = p.GetValue(objeto, null);

						if (value != null)
						{
							string valueString = value.ToString().ToUpper();
							p.SetValue(objeto, valueString, null);
						}
					}
				}
			}
		}


		public static bool ValidarCPF(object valor)
		{
			string cpf = string.Empty;

			if (valor == null)
			{
				return false;
			}
			cpf = valor.ToString();

			bool todosValoresIguais = true;

			char comparar = cpf.ToString()[0];
			foreach (var n in cpf.ToString())
			{
				if (comparar != n)
				{
					todosValoresIguais = false;
				}
			}

			if (todosValoresIguais)
			{
				return false;
			}

			int[] multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
			int[] multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

			string tempCpf;
			string digito;
			int soma;
			int resto;

			cpf = cpf.Trim();
			cpf = cpf.Replace(".", "").Replace("-", "");

			if (cpf.Length != 11)
				return false;

			tempCpf = cpf.Substring(0, 9);
			soma = 0;
			for (int i = 0; i < 9; i++)
				soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];

			resto = soma % 11;

			if (resto < 2)
				resto = 0;
			else
				resto = 11 - resto;

			digito = resto.ToString();
			tempCpf = tempCpf + digito;

			soma = 0;

			for (int i = 0; i < 10; i++)
				soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];

			resto = soma % 11;

			if (resto < 2)
				resto = 0;
			else
				resto = 11 - resto;

			digito = digito + resto.ToString();

			return cpf.EndsWith(digito);

		}

		public static string RemoveCaracterEspecial(string str)
		{

			/** Troca os caracteres acentuados por não acentuados **/
			//string[] acentos = new string[] { "ç", "Ç", "á", "é", "í", "ó", "ú", "ý", "Á", "É", "Í", "Ó", "Ú", "Ý", "à", "è", "ì", "ò", "ù", "À", "È", "Ì", "Ò", "Ù", "ã", "õ", "ñ", "ä", "ë", "ï", "ö", "ü", "ÿ", "Ä", "Ë", "Ï", "Ö", "Ü", "Ã", "Õ", "Ñ", "â", "ê", "î", "ô", "û", "Â", "Ê", "Î", "Ô", "Û" };
			//string[] semAcento = new string[] { "c", "C", "a", "e", "i", "o", "u", "y", "A", "E", "I", "O", "U", "Y", "a", "e", "i", "o", "u", "A", "E", "I", "O", "U", "a", "o", "n", "a", "e", "i", "o", "u", "y", "A", "E", "I", "O", "U", "A", "O", "N", "a", "e", "i", "o", "u", "A", "E", "I", "O", "U" };

			//for (int i = 0; i < acentos.Length; i++)
			//{
			//    str = str.Replace(acentos[i], semAcento[i]);
			//}

			/** Troca os caracteres especiais da string por "" **/
			string[] caracteresEspeciais = { "\\.", "`", "´", ".", "'", ",", "-", ":", "\\(", "\\)", "ª", "\\|", "\\\\", "°" };

			for (int i = 0; i < caracteresEspeciais.Length; i++)
			{
				str = str.Replace(caracteresEspeciais[i], "");
			}

			/** Troca os espaços no início por "" **/
			str = str.Replace("^\\s+", "");
			/** Troca os espaços no início por "" **/
			str = str.Replace("\\s+$", "");
			/** Troca os espaços duplicados, tabulações e etc por  " " **/
			str = str.Replace("\\s+", " ");

			return str.ToUpper();

		}

		public static string ValidarNome(string nome)
		{
			nome = nome.Replace("  ", " ").Trim();
			nome = RemoveCaracterEspecial(nome);
			nome = nome.Length > 69 ? nome.Substring(0, 69) : nome;

			if (nome.Length < 3)
				throw new Exception(string.Format("O nome '{0}' é inválido. O nome deve conter pelo menos três caracteres", nome));

			if (nome.IndexOf(" ") <= 0)
				throw new Exception(string.Format("O nome '{0}' é inválido. O nome não pode conter apenas um único termo", nome));

			string primeiroTermo = nome.Substring(0, nome.IndexOf(" "));
			string segundoTermo = nome.Replace(primeiroTermo, "").Trim();

			if (segundoTermo.IndexOf(" ") > 0)
			{
				segundoTermo = segundoTermo.Substring(0, segundoTermo.IndexOf(" "));
			}

			if (primeiroTermo.Length == 1 && segundoTermo.Length == 1)
				throw new Exception(string.Format("O nome '{0}' é inválido. O nome não pode conter o primeiro e segundo termos com apenas um caractere em cada um deles", nome));

			bool possuiMaisTermos = nome.Replace(primeiroTermo, "").Replace(segundoTermo, "").Trim().Length > 0;

			if (primeiroTermo.Length == 2 && segundoTermo.Length == 2 && !possuiMaisTermos)
				throw new Exception(string.Format("O nome '{0}' é inválido. O nome não pode conter apenas dois termos, ambos com apenas dois caracteres", nome));

			return nome;
		}

		public static long ConverterDataEpoch(DateTime data)
		{

			DateTime dt = data.Date;

			Int64 retval = 0;

			var st = new DateTime(1970, 1, 1);

			TimeSpan t = (dt.ToUniversalTime() - st);

			retval = (long)(t.TotalMilliseconds + 0.5);

			return retval;
		}

		public static long ConverterDataHoraEpoch(DateTime data)
		{
			DateTimeOffset dt = new DateTimeOffset(data);
			return dt.ToUnixTimeMilliseconds();
		}

		public static List<T> ConvertToList<T>(string valor)
		{
			List<T> retorno = null;

			if (!string.IsNullOrWhiteSpace(valor))
			{
				retorno = new List<T>();
				foreach (var item in valor.Split("|", StringSplitOptions.RemoveEmptyEntries))
				{
					retorno.Add((T)Convert.ChangeType(item, typeof(T)));
				}
			}

			return retorno;
		}

		public static string ConvertFromList<T>(List<T>? valor)
		{
			string retorno;

			if (valor?.Any() == true)
			{
				retorno = string.Join("|", valor);
			}
			else
			{
				retorno = null;
			}

			return retorno;
		}

		private static string ObterValorConvertido(string converterMethod, object value)
		{
			string retorno = string.Empty;

			List<object> controllers = new List<object>()
			{
				 new ControladorDadosEsus(),
				 new ControladorEndereco(),
				 new ControladorSigtap(),
				 new ControladorUnidadeSaude(),
			};

			foreach (var controller in controllers)
			{
				string result = ObterValorConvertido(controller, converterMethod, value);
				if (!string.IsNullOrWhiteSpace(result))
				{
					return result;
				}
			}

			return string.Empty;
		}
		private static string ObterValorConvertido(object controllerClass, string converterMethod, object value)
		{
			string retorno = string.Empty;
			MethodInfo methodInfo = controllerClass.GetType().GetMethod(converterMethod);

			if (methodInfo != null)
			{
				object results = null;
				ParameterInfo[] parameters = methodInfo.GetParameters();

				if (parameters.Length == 0)
				{
					results = methodInfo.Invoke(controllerClass, null);
				}
				else
				{
					object[] parametersArray = new object[] { value };
					results = methodInfo.Invoke(controllerClass, parametersArray);
				}

				if (results != null)
				{
					if (results.GetType() == typeof(Dictionary<string, long>))
					{
						var dic = (Dictionary<string, long>)results;
						if (dic != null && dic.Any(r => r.Value == (long)value))
						{
							retorno = dic.FirstOrDefault(r => r.Value == (long)value).Key;
						}
					}

					if (results.GetType() == typeof(Dictionary<string, string>))
					{
						var dic = (Dictionary<string, string>)results;
						if (dic != null && dic.Any(r => r.Value == (string)value))
						{
							retorno = dic.FirstOrDefault(r => r.Value == (string)value).Key;
						}
					}

					if (results.GetType() == typeof(string))
					{
						retorno = (string)results;
					}
				}
			}

			return retorno;
		}

		public static void ObterResumo(this Dictionary<string, string> retorno, object objeto, List<string>? exibirApenas = null)
		{
			if (objeto != null)
			{
				PropertyInfo[] properties = objeto.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

				foreach (PropertyInfo p in properties)
				{
					if (exibirApenas == null || exibirApenas.Contains(p.Name))
					{
						string valorConvertido = string.Empty;
						var value = p.GetValue(objeto, null);

						if (value != null)
						{
							foreach (ConditionalAttribute cond in p.GetCustomAttributes(typeof(ConditionalAttribute), true))
							{
								if (!string.IsNullOrWhiteSpace(cond.Converter))
								{
									value = ObterValorConvertido(cond.Converter, value);
								}
							}

							if (value.GetType() == typeof(DateTime))
							{
								valorConvertido = Convert.ToDateTime(value).ToString("dd/MM/yyyy");
							}
							else if (value.GetType() == typeof(bool))
							{
								valorConvertido = (bool)value ? "Sim" : "Não";
							}
							else
							{
								valorConvertido = value.ToString();
							}
						}

						string nomeExibicao = string.Empty;

						foreach (DisplayAttribute customAtt in p.GetCustomAttributes(typeof(DisplayAttribute), true))
						{
							nomeExibicao = customAtt.Name;
						}


						if (!string.IsNullOrWhiteSpace(nomeExibicao))
						{
							retorno[nomeExibicao] = valorConvertido;
						}
					}
				}
			}
		}

		public static void TrySetValue<T>(this T instance, string propName, object value)
		{
			PropertyInfo? property = null;
			if (instance != null)
				property = instance.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(p => p.Name == propName).FirstOrDefault();

			if (property != null)
			{
				if (value != null)
				{
					try
					{
						Type type = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
						property.SetValue(instance, Convert.ChangeType(value, type, null));
					}

					catch { }
				}
			}
		}

		public static string RemoveDiacritics(this String s)
		{
			String normalizedString = s.Normalize(NormalizationForm.FormD);
			StringBuilder stringBuilder = new StringBuilder();

			for (int i = 0; i < normalizedString.Length; i++)
			{
				Char c = normalizedString[i];
				if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
					stringBuilder.Append(c);
			}

			return stringBuilder.ToString();
		}
	}
}
