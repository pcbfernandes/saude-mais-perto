#nullable disable
using Newtonsoft.Json;
using SMP.Dominio.Model;

namespace SMP.Dominio.Controlador
{
    public class ControladorBuscaDadosPessoa
    {
        public async Task<PessoaModel> ObterPessoaBuscaDadosCpf(string cpf)
        {
            if (System.Diagnostics.Debugger.IsAttached)
            {
                await Task.Delay(3000);
            }

            PessoaModel pessoa = null;
            ConsultaCpfModel resultadoConsulta = await ConsultarDadosCNS(cpf);

            if (resultadoConsulta != null && resultadoConsulta.Sucesso && resultadoConsulta.Dados != null)
            {
                pessoa = new();

                pessoa.TrySetValue("CPF", resultadoConsulta.Dados.CPF.numeroCPF);
                pessoa.TrySetValue("Nome", resultadoConsulta.Dados.NomeCompleto.Nome);
                pessoa.TrySetValue("NomeMae", resultadoConsulta.Dados.Mae.Nome);
                pessoa.TrySetValue("NomePai", resultadoConsulta.Dados.Pai.Nome);
                pessoa.TrySetValue("NomeSocial", resultadoConsulta.Dados.NomeSocial);
                pessoa.TrySetValue("DataNascimento", resultadoConsulta.Dados.dataNascimento);
                pessoa.TrySetValue("PortariaNaturalizacao", resultadoConsulta.Dados.DadosNacionalidade.numeroPortariaNaturalizacao);

                ControladorDadosEsus controladorDadosEsus = new ControladorDadosEsus();
                ControladorEndereco controladorEndereco = new ControladorEndereco();

                var estados = controladorEndereco.ObterEstados();
                var paises = controladorDadosEsus.ObterOpcoesPais();
                if (paises.ContainsKey(resultadoConsulta.Dados.DadosNacionalidade?.PaisNascimento?.nomePais))
                {
                    var codPaisNascimento = paises[resultadoConsulta.Dados.DadosNacionalidade?.PaisNascimento?.nomePais];
                    pessoa.CodPaisNascimento = codPaisNascimento;

                    if (codPaisNascimento == ControladorDadosEsus.COD_BRASIL)
                    {
                        pessoa.CodNacionalidade = ControladorDadosEsus.COD_NACIONALIDADE_BRASILEIRA;
                    }
                }

                var sexo = controladorDadosEsus.ObterOpcoesSexo().FirstOrDefault(s => s.Key.ToUpper() == resultadoConsulta.Dados.Sexo?.descricaoSexo);
                pessoa.TrySetValue("Sexo", sexo.Value);

                var endereco = resultadoConsulta.Dados.Enderecos?.Endereco;
                if (endereco != null)
                {
                    pessoa.CEP = endereco.CEP?.numeroCEP;
                    string codigoMunicipio = endereco.Municipio.codigoMunicipio;

                    string siglaUF = endereco.Municipio?.UF?.siglaUF;
                    var estado = estados.FirstOrDefault(e => e.Sigla == siglaUF);
                    pessoa.CodEstado = estado?.CodEstado;

                    var municipios = controladorEndereco.ObterMunicipios(siglaUF);
                    

                    MunicipioModel municipio = municipios.FirstOrDefault(m => m.CodigoIBGE.Equals(codigoMunicipio) 
                    || m.CodigoIBGE.IndexOf(codigoMunicipio) >= 0);

                    pessoa.CodMunicipioIbge = municipio?.CodigoIBGE;

                    pessoa.TrySetValue("Bairro", endereco.Bairro?.descricaoBairro);
                    pessoa.Logradouro = $"{endereco.TipoLogradouro?.descricaoTipoLogradouro} {endereco.nomeLogradouro}, {endereco.numero}";
                    pessoa.TrySetValue("Complemento", endereco.complemento);
                }

                var municipioNascimento = resultadoConsulta.Dados.MunicipioNascimento;
                if (municipioNascimento != null)
                {
                    string siglaUF = municipioNascimento.UF?.siglaUF;
                    var estado = estados.FirstOrDefault(e => e.Sigla == siglaUF);
                    pessoa.CodEstadoNascimento = estado?.CodEstado;

                    var municipios = controladorEndereco.ObterMunicipios(siglaUF);

                    MunicipioModel municipio =
                        municipios.FirstOrDefault(m => m.CodigoIBGE == municipioNascimento.codigoMunicipio)
                        ?? municipios.FirstOrDefault(m => m.Nome == municipioNascimento.nomeMunicipio);

                    pessoa.CodIbgeMunicipioNascimento = municipio?.CodigoIBGE;
                }

                var telefone = resultadoConsulta.Dados.Telefones?[0];
                if (telefone != null)
                {
                    pessoa.TelefoneContato = $"{telefone.DDD}{telefone.numeroTelefone}";
                }

                var opcoesRacaCor = controladorDadosEsus.ObterOpcoesRacaCor();
                if (opcoesRacaCor.Any(r => r.Value.ToString().Equals(resultadoConsulta.Dados.RacaCor?.codigoRacaCor)))
                {
                    var racaCor = opcoesRacaCor.FirstOrDefault(r => r.Value.ToString().Equals(resultadoConsulta.Dados.RacaCor?.codigoRacaCor));
                    pessoa.TrySetValue("CodRacaCor", racaCor);

                }
                else {
                    pessoa.CodRacaCor = 6; //Sem Informação
                }
            }

            return pessoa;
        }

        private async Task<ConsultaCpfModel> ConsultarDadosCNS(string cpf)
        {
            ConsultaCpfModel retorno = new ConsultaCpfModel();
            if (!AppConfig.DesabilitarBuscaDadosCpf)
            {
                try
                {
                    var clientHandler = new HttpClientHandler { UseCookies = false, };
                    var client = new HttpClient(clientHandler);
                    var request = new HttpRequestMessage
                    {
                        Method = HttpMethod.Get,
                        RequestUri = new Uri(string.Concat("https://labiaps-api.azurewebsites.net/CadSUS/FindByCPF/", cpf)),
                        Headers =
                    {
                        { "Authorization", $"Basic {AppConfig.CredenciaisApi}" },
                    },
                    };

                    using (var response = await client.SendAsync(request))
                    {
                        response.EnsureSuccessStatusCode();
                        var body = await response.Content.ReadAsStringAsync();

                        if (!string.IsNullOrWhiteSpace(body))
                        {
                            retorno = JsonConvert.DeserializeObject<ConsultaCpfModel>(body);
                        }
                    }
                }
                catch (Exception ex)
                {
                    retorno.Error = ex;
                }
            }
            else
            {
                retorno = null;
            }

            return retorno;
        }
    }
}
