using Newtonsoft.Json;
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

        public async Task<Transacao> CreateTransacao(Transacao transacao)
        {
            // Validar limite de entidades antes de criar
            await _entityLimitService.ValidateEntityLimit<Transacao>("Transação");
            
            // Buscar cartão selecionado
            var cartaoSelecionado = await _context.Cartoes.FindAsync(transacao.CartaoId);
            if (cartaoSelecionado == null)
            {
                throw new InvalidOperationException("Cartão não encontrado.");
            }
            
            transacao.Cartao = cartaoSelecionado;
            
            // Criar pagamento via Cielo API
            var result = await _cieloService.CreatePayment(transacao);
            transacao.Log = result;
            
            // Processar resposta da API
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
    }
}
