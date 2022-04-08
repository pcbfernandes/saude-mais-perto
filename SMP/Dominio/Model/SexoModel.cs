namespace SMP.Dominio.Model
{
    public class SexoModel
    {
        public string Descricao { get; }
        public string Valor { get; }
        public SexoModel(string descricao, string valor)
        {
            Descricao = descricao;
            Valor = valor;
        }
    }
}
