using IntegracaoCieloEcommerceSandbox.Data;
using IntegracaoCieloEcommerceSandbox.Models;
using IntegracaoCieloEcommerceSandbox.Exceptions;

namespace IntegracaoCieloEcommerceSandbox.Services
{
    public class CartaoService
    {
        private readonly AppDbContext _context;
        private readonly EntityLimitService _entityLimitService;

        public CartaoService(AppDbContext context, EntityLimitService entityLimitService)
        {
            _context = context;
            _entityLimitService = entityLimitService;
        }

        public async Task<Cartao> CreateCartao(Cartao cartao)
        {
            // Validar limite de entidades antes de criar
            await _entityLimitService.ValidateEntityLimit<Cartao>("Cartão");
            
            // Processar validade
            cartao.Validade = $"{cartao.Mes.ToString("D2")}/{cartao.Ano}";
            
            // Converter nome para maiúsculas
            cartao.NomeNoCartao = cartao.NomeNoCartao.ToUpper();
            
            // Definir bandeira
            if (cartao.NumeroDoCartao.StartsWith("5"))
            {
                cartao.Bandeira = "Master";
            }
            else
            {
                cartao.Bandeira = "Visa";
            }
            
            _context.Cartoes.Add(cartao);
            await _context.SaveChangesAsync();
            
            return cartao;
        }
    }
}
