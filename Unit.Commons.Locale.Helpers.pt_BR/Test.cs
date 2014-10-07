using System;
using System.Globalization;
using Commons.Locale.Helpers.pt_BR;
using NUnit.Framework;

namespace Unit.Commons.Locale.Helpers.pt_BR
{
	[TestFixture]
	public class Test
	{
		[Test]
		public void TesteDoBilhão ()
		{
			var valor = decimal.Parse("1234567890.12", CultureInfo.InvariantCulture);
			PorExtenso teste = new PorExtenso(valor);
			Assert.AreEqual(valor, teste.ValorNumérico);
			Assert.AreEqual("um bilhão duzentos e trinta e quatro milhões quinhentos e sessenta e sete mil e oitocentos e noventa reais e doze centavos", teste.ToString());
			Assert.AreEqual("um bilhão duzentos e trinta e quatro milhões quinhentos e sessenta e sete mil e oitocentos e noventa reais e doze centavos", PorExtenso.EmReais(valor));
			Assert.AreEqual("um bilhão duzentos e trinta e quatro milhões quinhentos e sessenta e sete mil e oitocentos e noventa dólares e doze centavos de dólar", PorExtenso.EmDólares(valor));
			Assert.AreEqual(new string[] {
				"um bilhão duzentos e trinta e",
				"quatro milhões quinhentos e", 
				"sessenta e sete mil e oitocentos e noventa reais e doze centavos"
			}, teste.QuebradoEmLinhas(30, 30, 100));
		}

		[Test]
		public void TesteDeCotaçãoDoDólar ()
		{
			Assert.AreEqual("dois reais e seiscentos e doze milésimos", PorExtenso.CotaçãoDoDólar(2.612m));
		}

		[Test]
		public void TesteDeMultiValores ()
		{

			for (decimal i = 0m; i < 1002m; i += (i < 2m) ? 0.01m : 1m) {
				var teste = new PorExtenso(i);
				var testeItens = new PorExtenso(i, "item", "itens", nãoEncurtarUmMilParaMil: true, useAParteFracionária : false);
				var testeCoisas = new PorExtenso(i, "coisa", "coisas", noFeminino: true, useAParteFracionária : false);
				if (i == 0m) {
					Assert.AreEqual("zero", teste.ToString());
					Assert.AreEqual(teste.ToString(), testeItens.ToString());
					Assert.AreEqual(teste.ToString(), testeCoisas.ToString());
					Assert.AreEqual(testeItens.ToString(), testeCoisas.ToString());
				} else if (i < 1) {
					Assert.AreNotEqual(teste.ToString(), testeItens.ToString());
					Assert.IsEmpty(testeCoisas.ToString());
					Assert.IsEmpty(testeCoisas.ToString());
				} else {
					Assert.AreNotEqual(teste.ToString(), testeItens.ToString());
					Assert.AreNotEqual(teste.ToString(), testeCoisas.ToString());
					Assert.AreNotEqual(testeItens.ToString(), testeCoisas.ToString());
				}
			}
		}
	}
}

