using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using IntegracaoCieloEcommerceSandbox.Data;
using IntegracaoCieloEcommerceSandbox.Models;
using IntegracaoCieloEcommerceSandbox.Exceptions;

namespace IntegracaoCieloEcommerceSandbox.Services
{
    public class TransacaoService
    {
        private readonly AppDbContext _context;
        private readonly EntityLimitService _entityLimitService;
        private readonly CieloService _cieloService;

        public TransacaoService(
            AppDbContext context, 
            EntityLimitService entityLimitService,
            CieloService cieloService)
        {
            _context = context;
            _entityLimitService = entityLimitService;
            _cieloService = cieloService;
        }

        public async Task<Transacao> CreateTransacao(Transacao transacao, int cartaoId)
        {
            // Validar limite de entidades antes de criar
            await _entityLimitService.ValidateEntityLimit<Transacao>("Transação");
            
            if (cartaoId <= 0)
            {
                throw new InvalidOperationException("Por favor, selecione um cartão.");
            }
            
            var cartaoSelecionado = await _context.Cartoes.FindAsync(cartaoId);
            
            if (cartaoSelecionado == null)
            {
                throw new InvalidOperationException("Cartão não encontrado.");
            }
            
            transacao.NumeroDoCartao = cartaoSelecionado.NumeroDoCartao;
            transacao.NomeNoCartao = cartaoSelecionado.NomeNoCartao;
            
            var result = await _cieloService.CreatePayment(transacao, cartaoSelecionado);
            transacao.Log = result;
            
            var log = JsonConvert.DeserializeObject<LogDaTransacao>(transacao.Log);
            
            if (log == null || string.IsNullOrEmpty(log.Payment.PaymentId))
            {
                throw new InvalidOperationException("PaymentId não identificado");
            }
            
            transacao.PaymentId = log.Payment.PaymentId;
            transacao.EstadoDaTransacao = log.Payment.ReturnMessage;
            
            _context.Transacoes.Add(transacao);
            await _context.SaveChangesAsync();
            
            return transacao;
        }

        public async Task DeleteTransacao(int id)
        {
            var transacao = await _context.Transacoes.FindAsync(id);

            if (transacao is null)
            {
                throw new KeyNotFoundException($"Transação com id {id} não encontrada.");
            }

            _context.Transacoes.Remove(transacao);
            await _context.SaveChangesAsync();
        }
    }
}
