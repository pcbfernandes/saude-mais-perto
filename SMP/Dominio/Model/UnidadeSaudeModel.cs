namespace SMP.Dominio.Model
{
	public class UnidadeSaudeModel
	{
		public string CNES { get; set; }
		public string Descricao { get; set; }
		public string CodigoIBGE { get; set; }
		public List<string> ListaCepCobertura { get; set; }
	}
}
