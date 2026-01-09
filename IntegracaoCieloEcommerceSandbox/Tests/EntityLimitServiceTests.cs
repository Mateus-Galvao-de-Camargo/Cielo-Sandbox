using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.EntityFrameworkCore;
using IntegracaoCieloEcommerceSandbox.Models;
using IntegracaoCieloEcommerceSandbox.Data;
using IntegracaoCieloEcommerceSandbox.Services;
using IntegracaoCieloEcommerceSandbox.Exceptions;
using System.Threading.Tasks;

namespace IntegracaoCieloEcommerceSandbox.Tests
{
    [TestClass]
    public class EntityLimitServiceTests
    {
        private AppDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: System.Guid.NewGuid().ToString())
                .Options;
            
            return new AppDbContext(options);
        }

        [TestMethod]
        public async Task ValidateEntityLimit_ComMenosDe5Cartoes_NaoDeveLancarExcecao()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var entityLimitService = new EntityLimitService(context);
            
            // Adicionar 4 cartões
            for (int i = 0; i < 4; i++)
            {
                context.Cartoes.Add(new Cartao
                {
                    NumeroDoCartao = $"411111111111111{i}",
                    Cvv = "123",
                    Mes = 12,
                    Ano = 2025,
                    NomeNoCartao = $"Teste {i}"
                });
            }
            await context.SaveChangesAsync();

            // Act & Assert - não deve lançar exceção
            await entityLimitService.ValidateEntityLimit<Cartao>("Cartão");
        }

        [TestMethod]
        public async Task ValidateEntityLimit_Com5Cartoes_DeveLancarExcecao()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var entityLimitService = new EntityLimitService(context);
            
            // Adicionar 5 cartões
            for (int i = 0; i < 5; i++)
            {
                context.Cartoes.Add(new Cartao
                {
                    NumeroDoCartao = $"411111111111111{i}",
                    Cvv = "123",
                    Mes = 12,
                    Ano = 2025,
                    NomeNoCartao = $"Teste {i}"
                });
            }
            await context.SaveChangesAsync();

            // Act & Assert
            await Assert.ThrowsExceptionAsync<EntityLimitExceededException>(
                async () => await entityLimitService.ValidateEntityLimit<Cartao>("Cartão")
            );
        }

        [TestMethod]
        public async Task ValidateEntityLimit_Com5Cartoes_DeveLancarExcecaoComMensagemCorreta()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var entityLimitService = new EntityLimitService(context);
            
            // Adicionar 5 cartões
            for (int i = 0; i < 5; i++)
            {
                context.Cartoes.Add(new Cartao
                {
                    NumeroDoCartao = $"411111111111111{i}",
                    Cvv = "123",
                    Mes = 12,
                    Ano = 2025,
                    NomeNoCartao = $"Teste {i}"
                });
            }
            await context.SaveChangesAsync();

            // Act
            try
            {
                await entityLimitService.ValidateEntityLimit<Cartao>("Cartão");
                Assert.Fail("Deveria ter lançado EntityLimitExceededException");
            }
            catch (EntityLimitExceededException ex)
            {
                // Assert
                Assert.IsTrue(ex.Message.Contains("Limite de 5 Cartão(s) atingido"));
                Assert.IsTrue(ex.Message.Contains("sandbox"));
            }
        }

        [TestMethod]
        public async Task CartaoService_CreateCartao_ComMenosDe5Cartoes_DeveCriarComSucesso()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var entityLimitService = new EntityLimitService(context);
            var cartaoService = new CartaoService(context, entityLimitService);

            // Act
            var novoCartao = new Cartao
            {
                NumeroDoCartao = "4111111111111111",
                Cvv = "123",
                Mes = 12,
                Ano = 2025,
                NomeNoCartao = "Teste Usuario"
            };

            var resultado = await cartaoService.CreateCartao(novoCartao);

            // Assert
            Assert.IsNotNull(resultado);
            Assert.AreEqual("TESTE USUARIO", resultado.NomeNoCartao);
            Assert.AreEqual("12/2025", resultado.Validade);
            Assert.AreEqual("Visa", resultado.Bandeira);
            Assert.AreEqual(1, await context.Cartoes.CountAsync());
        }

        [TestMethod]
        public async Task CartaoService_CreateCartao_Com5Cartoes_DeveLancarExcecao()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var entityLimitService = new EntityLimitService(context);
            var cartaoService = new CartaoService(context, entityLimitService);
            
            // Adicionar 5 cartões
            for (int i = 0; i < 5; i++)
            {
                context.Cartoes.Add(new Cartao
                {
                    NumeroDoCartao = $"411111111111111{i}",
                    Cvv = "123",
                    Mes = 12,
                    Ano = 2025,
                    NomeNoCartao = $"Teste {i}"
                });
            }
            await context.SaveChangesAsync();

            // Act & Assert
            var novoCartao = new Cartao
            {
                NumeroDoCartao = "5111111111111111",
                Cvv = "456",
                Mes = 6,
                Ano = 2026,
                NomeNoCartao = "Novo Usuario"
            };

            await Assert.ThrowsExceptionAsync<EntityLimitExceededException>(
                async () => await cartaoService.CreateCartao(novoCartao)
            );

            // Verificar que o cartão não foi adicionado
            Assert.AreEqual(5, await context.Cartoes.CountAsync());
        }
    }
}
