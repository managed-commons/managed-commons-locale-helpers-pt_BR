// Class Commons.Locale.Helpers.pt_BR.TestePorExtenso
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

// TODO: Migrate to unit testing using NUnit
using System;
using System.Collections;
using System.Text;

using Commons.Locale.Helpers.pt_BR;

public class TestePorExtenso {

	public static void Main(string[] args) {
		Console.WriteLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
		Console.WriteLine("<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Strict//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd\">");
		Console.WriteLine("<html xmlns=\"http://www.w3.org/1999/xhtml\" xml:lang=\"pt-br\" lang=\"pt-br\"><head><title>Resultados de Testes - PorExtenso</title></head><body>");
		Console.WriteLine("<div style=\"padding:16px;border:solid 4pt navy;background-color:lightblue;\">");
		Console.WriteLine("<h1>Resultados de Testes</h1><h2>Biblioteca Commons.Locale.Helpers.pt_BR</h2><h3>Classe PorExtenso</h3>");
		Console.WriteLine("<!-- Creative Commons License -->");
		Console.WriteLine("<a rel=\"license\" href=\"http://creativecommons.org/licenses/by/2.0/br/\"><img alt=\"Creative Commons License\" border=\"0\" src=\"http://creativecommons.org/images/public/somerights20.pt.gif\" /></a><br />");
		Console.WriteLine("This work is licensed under a <a rel=\"license\" href=\"http://creativecommons.org/licenses/by/2.0/br/\">Creative Commons License</a>.");
		Console.WriteLine("<!-- /Creative Commons License -->");
		Console.WriteLine("<!--");
		Console.WriteLine("<rdf:RDF xmlns=\"http://web.resource.org/cc/\"");
		Console.WriteLine("    xmlns:dc=\"http://purl.org/dc/elements/1.1/\"");
		Console.WriteLine("    xmlns:rdf=\"http://www.w3.org/1999/02/22-rdf-syntax-ns#\">");
		Console.WriteLine("<Work rdf:about=\"\">");
		Console.WriteLine("   <dc:type rdf:resource=\"http://purl.org/dc/dcmitype/Text\" />");
		Console.WriteLine("   <license rdf:resource=\"http://creativecommons.org/licenses/by/2.0/br/\" />");
		Console.WriteLine("</Work>");
		Console.WriteLine("<License rdf:about=\"http://creativecommons.org/licenses/by/2.0/br/\">");
		Console.WriteLine("   <permits rdf:resource=\"http://web.resource.org/cc/Reproduction\" />");
		Console.WriteLine("   <permits rdf:resource=\"http://web.resource.org/cc/Distribution\" />");
		Console.WriteLine("   <requires rdf:resource=\"http://web.resource.org/cc/Notice\" />");
		Console.WriteLine("   <requires rdf:resource=\"http://web.resource.org/cc/Attribution\" />");
		Console.WriteLine("   <permits rdf:resource=\"http://web.resource.org/cc/DerivativeWorks\" />");
		Console.WriteLine("</License>");
		Console.WriteLine("</rdf:RDF>");
		Console.WriteLine("-->");
		Console.WriteLine("</div>");
		foreach(string arg in args) {
			PorExtenso teste = new PorExtenso(decimal.Parse(arg));
			Console.WriteLine("<p style=\"padding:8px;border:solid 2pt black\">Da linha de comando {0} = {1}<br>", teste.ValorNumérico, teste.ToString());
			try {
				Console.WriteLine("Em linhas: <br>");
				foreach(string linha in teste.QuebradoEmLinhas(17,27,27))
					Console.WriteLine("*" + linha + "*<br>");
				Console.WriteLine("Em linhas curtas: <br>");
				foreach(string linha in teste.QuebradoEmLinhas(7,7,7))
					Console.WriteLine("*" + linha + "*<br>");
			} catch (Exception ex) {
				Console.WriteLine(ex.ToString());
			}
			Console.WriteLine("</p>");
		}
		Console.WriteLine("<p style=\"padding:8px;border:solid 2pt black\">EmReais()/EmDólares() {0}  ~= {1}</p>", PorExtenso.EmReais(1.23m), PorExtenso.EmDólares(1.23m / 2.612m));
		Console.WriteLine("<p style=\"padding:8px;border:solid 2pt black\">CotaçãoDoDólar() {0}</p>", PorExtenso.CotaçãoDoDólar(2.612m));
		Console.WriteLine("<div style=\"padding:8px;border:solid 2pt black\">");
		Console.WriteLine("<table style=\"width:100%\" width=\"100%\"><thead style=\"background-color:lightgrey;padding:8px;border:solid 2pt navy\"><td>teste monet&aacute;rio</td>");
		Console.WriteLine("<td>teste inteiro masculino</td><td>teste inteiro feminino</td><thead><tr><td valign=\"top\">");
		for (decimal i = 0m; i < 1002m; i += (i < 2m)?0.01m:1m)
		{
			PorExtenso teste = new PorExtenso(i);
			Console.WriteLine("R$ {0} = {1} <br>", teste.ValorNumérico, teste.ToString());
		}
		Console.WriteLine("</td><td valign=\"top\">");
		for (int i = 0; i < 1002; i++)
		{
			PorExtenso teste = new PorExtenso(i, "item", "itens", false, true, false);
			Console.WriteLine("{0} = {1} <br>", teste.ValorNumérico, teste.ToString());
		}
		Console.WriteLine("</td><td valign=\"top\">");
		for (int i = 0; i < 1002; i++)
		{
			PorExtenso teste = new PorExtenso(i, "coisa", "coisas", false, false, true);
			Console.WriteLine("{0} = {1} <br>", teste.ValorNumérico, teste.ToString());
		}
		Console.WriteLine("</td></tr></table>");
		Console.WriteLine("</div>");
		Console.WriteLine("</body></html>");
	}
	
}
