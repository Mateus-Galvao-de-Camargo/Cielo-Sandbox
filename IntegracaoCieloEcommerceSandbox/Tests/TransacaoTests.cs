using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using IntegracaoCieloEcommerceSandbox.Data;
using IntegracaoCieloEcommerceSandbox.Models;
using IntegracaoCieloEcommerceSandbox.Services;

namespace IntegracaoCieloEcommerceSandbox.Tests
{
    [TestClass]
    public class TransacaoTests
    {
        private AppDbContext _context = null!;

        [TestInitialize]
        public void Setup()
        {
            // Criando um banco de dados InMemory para os testes
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TesteTransacaoDb")
                .Options;

            _context = new AppDbContext(options);

            // Verifica se o cartão já foi adicionado
            if (!_context.Cartoes.Any(c => c.Id == 1))
            {
                _context.Cartoes.Add(new Cartao
                {
                    Id = 1,
                    NumeroDoCartao = "4111111111111111",
                    Cvv = "123",
                    Mes = 12,
                    Ano = 2025,
                    NomeNoCartao = "Juan Warzone Gomes"
                });
                _context.SaveChanges();
            }
        }

        [TestMethod]
        public void TestarCriacaoDeTransacaoComCartaoExistente()
        {
            var transacao = new Transacao
            {
                CartaoId = 1,  // ID de cartão existente
                Valor = 100,
                EstadoDaTransacao = "Pendente"
            };

            // Verifica se o CartaoId pertence a um Cartão salvo
            var cartaoExiste = _context.Cartoes.Any(c => c.Id == transacao.CartaoId);

            if (cartaoExiste)
            {
                _context.Transacoes.Add(transacao);
                _context.SaveChanges();
            }

            // Verifica se o cartaoExiste é true, se não for mostra a mensagem
            Assert.IsTrue(cartaoExiste, "O cartão deveria existir e a transação deveria ser criada.");
            Assert.IsNotNull(_context.Transacoes.FirstOrDefault(t => t.CartaoId == 1), "A transação com o CartaoId 1 deveria existir.");
        }

        [TestMethod]
        public void TestarCriacaoDeTransacaoComCartaoInexistente()
        {
            var transacao = new Transacao
            {
                CartaoId = 321,  // ID de cartão inexistente
                Valor = 100,
                EstadoDaTransacao = "Pendente"
            };

            var cartaoExiste = _context.Cartoes.Any(c => c.Id == transacao.CartaoId);

            if (!cartaoExiste)
            {
                _context.Transacoes.Add(transacao);
            }

            Assert.IsFalse(cartaoExiste, "O cartão não deveria existir e a transação não deveria ser criada.");
            Assert.IsNull(_context.Transacoes.FirstOrDefault(t => t.CartaoId == 2), "A transação com o CartaoId 2 não deveria ser criada.");
        }

        [TestMethod]
        public async Task TestarExclusaoDeTransacaoExistente()
        {
            var transacao = new Transacao
            {
                CartaoId = 1,
                Valor = 50,
                EstadoDaTransacao = "Pendente"
            };

            _context.Transacoes.Add(transacao);
            _context.SaveChanges();

            var transacaoService = CriarTransacaoService();

            await transacaoService.DeleteTransacao(transacao.Id);

            Assert.IsNull(_context.Transacoes.FirstOrDefault(t => t.Id == transacao.Id), "A transação deveria ser removida do banco.");
        }

        private TransacaoService CriarTransacaoService()
        {
            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?>
                {
                    { "Cielo:MerchantId", "dummy" },
                    { "Cielo:MerchantKey", "dummy" },
                    { "Cielo:CieloApiCriaTransacaoUrl", "https://example.com" }
                })
                .Build();

            var httpClient = new HttpClient(new FakeHttpMessageHandler());

            return new TransacaoService(
                _context,
                new EntityLimitService(_context),
                new CieloService(httpClient, config));
        }

        private class FakeHttpMessageHandler : HttpMessageHandler
        {
            protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                var response = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent("{}")
                };

                return Task.FromResult(response);
            }
        }
    }
}
