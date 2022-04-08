namespace SMP.Dominio.Model
{
    public class DeficienciaModel
    {
        public string Descricao { get; internal set; }
        public long Codigo { get; internal set; }
        public bool IsSelecionado { get; set; }
        public DeficienciaModel(string descricao, long codigo)
        {
            Descricao = descricao;
            Codigo = codigo;
        }
    }
}
