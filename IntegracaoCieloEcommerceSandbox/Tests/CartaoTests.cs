using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.ComponentModel.DataAnnotations;
using IntegracaoCieloEcommerceSandbox.Models;
using System.Collections.Generic;

namespace IntegracaoCieloEcommerceSandbox.Tests
{
    [TestClass]
    public class CartaoTests
    {
        [TestMethod]
        [DataRow("4111111111111", true)]  // Cartão Visa válido com 13 dígitos
        [DataRow("4111111111111111", true)]  // Cartão Visa válido com 16 dígitos
        [DataRow("5111111111111111", true)]  // Cartão Mastercard válido com 16 dígitos
        [DataRow("6111111111111111", false)]  // Cartão de número inicial inválido
        [DataRow("411111111111111", false)]  // Cartão Visa inválido com 15 dígitos
        [DataRow("511111111111111", false)]  // Cartão Mastercard inválido com 15 dígitos
        public void TestarValidacaoDoNumeroDoCartao(string numeroDoCartao, bool resultadoEsperado)
        {
            var cartao = new Cartao
            {
                NumeroDoCartao = numeroDoCartao,
                Cvv = "123",
                Mes = 12,
                Ano = 2025,
                NomeNoCartao = "Roberto Valorant Pedroso"
            };

            var resultados = new List<ValidationResult>();

            // Prepara um contexto (lista informações) para o método seguinte
            var contexto = new ValidationContext(cartao);

            // Valida usando as Data Annotations do Model e guarda os erros na List resultados
            var isValid = Validator.TryValidateObject(cartao, contexto, resultados, true);

            Assert.AreEqual(resultadoEsperado, isValid);
        }

        [TestMethod]
        [DataRow("Fernanda GTA VI da Silva", true)]  // Nome válido
        [DataRow("João da Silva", false)]  // Nome inválido
        [DataRow("Joao123", false)]  // Nome inválido
        [DataRow("", false)]  // Nome vazio
        public void TestarValidacaoDoNomeNoCartao(string nomeNoCartao, bool resultadoEsperado)
        {
            var cartao = new Cartao
            {
                NumeroDoCartao = "4111111111111111",
                Cvv = "123",
                Mes = 12,
                Ano = 2025,
                NomeNoCartao = nomeNoCartao
            };

            var contexto = new ValidationContext(cartao);
            var resultados = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(cartao, contexto, resultados, true);

            Assert.AreEqual(resultadoEsperado, isValid);
        }
    }
}
