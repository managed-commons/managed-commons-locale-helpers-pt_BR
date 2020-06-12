// Class Commons.Locale.Helpers.pt_BR.PorExtenso
//
// Copyright ©2010 Rafael 'Monoman' Teixeira
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Commons;

[assembly: Commons.Author("Rafael 'Monoman' Teixeira")]
[assembly: License(LicenseType.MIT)]
[assembly: ReportBugsTo("https://github.com/managed-commons/managed-commons-locale-helpers-pt_BR/issues")]
[assembly: IsPartOfPackage("Managed Commons")]

namespace Commons.Locale.Helpers.pt_BR
{
    public enum Gênero
    {
        Masculino,
        Feminino
    }

    ///<summary>
    /// Formats a number to it's Brazilian Portuguese written form, allowing for unit and gender variations
    ///</summary>
    public class PorExtenso
    {
        public readonly decimal ValorNumérico;

        public PorExtenso(decimal valor) : this(valor, "real", "reais", 100m, "centavo", "centavos", false, Gênero.Masculino) {
        }

        public PorExtenso(decimal valor, string unidadeNoSingular, string unidadeNoPlural, bool useAParteFracionária = true, bool nãoEncurtarUmMilParaMil = false, bool noFeminino = false) :
            this(valor, unidadeNoSingular, unidadeNoPlural, 100m, "centavo", "centavos", nãoEncurtarUmMilParaMil, noFeminino ? Gênero.Feminino : Gênero.Masculino)
            => _useAParteFracionária = useAParteFracionária;

        public PorExtenso(
            decimal valor,
            string unidadeNoSingular,
            string unidadeNoPlural,
            decimal escalaDaParteFracionária,
            string unidadeFracionáriaNoSingular,
            string unidadeFracionáriaNoPlural,
            bool nãoEncurtarUmMilParaMil,
            Gênero gênero) {
            ValorNumérico = Math.Abs(valor);
            _unidadeNoSingular = unidadeNoSingular;
            _unidadeNoPlural = unidadeNoPlural;
            _useAParteFracionária = (escalaDaParteFracionária > 0m);
            _nãoEncurtarUmMilParaMil = nãoEncurtarUmMilParaMil;
            _gênero = gênero;
            _escalaDaParteFracionária = escalaDaParteFracionária;
            _unidadeFracionáriaNoSingular = unidadeFracionáriaNoSingular;
            _unidadeFracionáriaNoPlural = unidadeFracionáriaNoPlural;
        }

        public string[] Partes {
            get {
                if (ValorNumérico == 0m)
                    return _pequenosNúmeros.Take(1).ToArray();
                var partes = new List<string>();
                var valor = ValorNumérico;
                int fragmento;
                foreach (var qualificador in _qualificadores.Where(qualificador => valor >= qualificador.Fator)) {
                    fragmento = (int)Math.Truncate(valor / qualificador.Fator);
                    ProcessarUmFragmento(fragmento, partes, false, _gênero);
                    if (fragmento > 1 || partes.Count > 1)
                        partes.Add(qualificador.Plural);
                    else
                        partes.Add(qualificador.Singular);
                    valor %= qualificador.Fator;
                }

                if (valor >= 1000m) {
                    fragmento = (int)(valor / 1000m);
                    if (fragmento != 1 || partes.Count > 0 || _nãoEncurtarUmMilParaMil) {
                        ProcessarUmFragmento(fragmento, partes, false, _gênero);
                    }
                    partes.Add("mil");
                    valor %= 1000m;
                }
                if (valor >= 1m) {
                    fragmento = (int)valor;
                    if (fragmento == 1 && partes.Count == 0) {
                        if (_gênero == Gênero.Feminino)
                            partes.Add("uma");
                        else
                            partes.Add("um");
                        partes.Add(_unidadeNoSingular);
                    } else {
                        ProcessarUmFragmento(fragmento, partes, true, _gênero);
                        partes.Add(_unidadeNoPlural);
                    }
                } else {
                    if (ValorNumérico >= 1000000m)
                        partes.Add("de");
                    if (ValorNumérico >= 2m)
                        partes.Add(_unidadeNoPlural);
                }
                if (_useAParteFracionária) {
                    fragmento = (int)((valor % 1m) * _escalaDaParteFracionária);
                    if (fragmento >= 1) {
                        ProcessarUmFragmento(fragmento, partes, true, _gênero);
                        if (fragmento > 1)
                            partes.Add(_unidadeFracionáriaNoPlural);
                        else
                            partes.Add(_unidadeFracionáriaNoSingular);
                    }
                }
                return partes.ToArray();
            }
        }

        public static string CotaçãoDoDólar(decimal valor) => new PorExtenso(valor,
                                                                             "real",
                                                                             "reais",
                                                                             1000m,
                                                                             "milésimo",
                                                                             "milésimos",
                                                                             false,
                                                                             Gênero.Masculino).ToString();

        public static string EmDólares(decimal valor) => new PorExtenso(valor,
                                                                        "dólar",
                                                                        "dólares",
                                                                        100m,
                                                                        "centavo de dólar",
                                                                        "centavos de dólar",
                                                                        false,
                                                                        Gênero.Masculino).ToString();

        public static string EmReais(decimal valor) => new PorExtenso(valor).ToString();

        public string[] QuebradoEmLinhas(params int[] tamanhos) {
            var linhas = new string[tamanhos.Length];
            var partes = Partes;
            var parteEmQuestão = 0;
            var linhaSendoFormada = 0;
            while (linhaSendoFormada < linhas.Length) {
                var sb = new StringBuilder();
                var size = tamanhos[linhaSendoFormada];
                while (parteEmQuestão < partes.Length && partes[parteEmQuestão].Length + sb.Length <= size) {
                    sb.Append(partes[parteEmQuestão++]);
                    if (sb.Length < size)
                        sb.Append(' ');
                }
                linhas[linhaSendoFormada++] = sb.ToString().Trim();
            }
            if (parteEmQuestão < partes.Length)
                throw new ArgumentException("Tamanhos das linhas são insuficientes para acomodar o texto formatado");
            return linhas;
        }

        public override string ToString() => string.Join(" ", Partes);

        private static readonly string[] _centenas = {"cento", "duzentos", "trezentos", "quatrocentos", "quinhentos", "seiscentos",
            "setecentos", "oitocentos", "novecentos"
        };

        private static readonly string[] _centenasNoFeminino = {"cento", "duzentas", "trezentas", "quatrocentas", "quinhentas", "seiscentas",
            "setecentas", "oitocentas", "novecentas"
        };

        private static readonly string[] _dezenas = { "vinte", "trinta", "quarenta", "cinqüenta", "sessenta", "setenta", "oitenta", "noventa" };

        private static readonly string[] _pequenosNúmeros = {"zero", "um", "dois", "três", "quatro", "cinco", "seis", "sete", "oito", "nove", "dez",
            "onze", "doze", "treze", "quatorze", "quinze", "dezesseis", "dezessete", "dezoito", "dezenove"
        };

        private static readonly string[] _pequenosNúmerosNoFeminino = {"zero", "uma", "duas", "três", "quatro", "cinco", "seis", "sete", "oito", "nove", "dez",
            "onze", "doze", "treze", "quatorze", "quinze", "dezesseis", "dezessete", "dezoito", "dezenove"
        };

        private static readonly Qualificador[] _qualificadores = {
            new Qualificador(1000000000000000000000000m, "septilhão", "septilhões"),
            new Qualificador(1000000000000000000000m, "sextilhão", "sextilhões"),
            new Qualificador(1000000000000000000m, "quintilhão", "quintilhões"),
            new Qualificador(1000000000000000m, "quatrilhão", "quatrilhões"),
            new Qualificador(1000000000000m, "trilhão", "trilhões"),
            new Qualificador(1000000000m, "bilhão", "bilhões"),
            new Qualificador(1000000m, "milhão", "milhões")
        };

        private readonly decimal _escalaDaParteFracionária;

        private readonly Gênero _gênero;

        private readonly bool _nãoEncurtarUmMilParaMil;

        private readonly string _unidadeFracionáriaNoPlural;

        private readonly string _unidadeFracionáriaNoSingular;

        private readonly string _unidadeNoPlural;

        private readonly string _unidadeNoSingular;

        private readonly bool _useAParteFracionária;

        private static void ProcessarUmFragmento(int valor, List<string> partes, bool precisaDoConector, Gênero gênero) {
            if (valor > 0 && valor < 1000) {
                if (precisaDoConector && partes.Count > 0)
                    partes.Add("e");
                if (valor > 100) {
                    if (gênero == Gênero.Feminino)
                        partes.Add(_centenasNoFeminino[(valor / 100) - 1]);
                    else
                        partes.Add(_centenas[(valor / 100) - 1]);
                    precisaDoConector = true;
                } else {
                    if (valor == 100) {
                        partes.Add("cem");
                        return;
                    }
                    precisaDoConector = false;
                }
                var valorDasDezenas = valor % 100;
                if (valorDasDezenas > 0) {
                    if (precisaDoConector)
                        partes.Add("e");
                    if (valorDasDezenas < 20) {
                        if (gênero == Gênero.Feminino)
                            partes.Add(_pequenosNúmerosNoFeminino[valorDasDezenas]);
                        else
                            partes.Add(_pequenosNúmeros[valorDasDezenas]);
                    } else {
                        partes.Add(_dezenas[(valorDasDezenas / 10) - 2]);
                        if (valorDasDezenas % 10 > 0) {
                            partes.Add("e");
                            if (gênero == Gênero.Feminino)
                                partes.Add(_pequenosNúmerosNoFeminino[valorDasDezenas % 10]);
                            else
                                partes.Add(_pequenosNúmeros[valorDasDezenas % 10]);
                        }
                    }
                }
            }
        }
    }

    internal struct Qualificador
    {
        public readonly decimal Fator;
        public readonly string Plural;
        public readonly string Singular;

        public Qualificador(decimal fator, string singular, string plural) {
            Fator = fator;
            Singular = singular;
            Plural = plural;
        }
    }
}
