using Microsoft.EntityFrameworkCore;
using IntegracaoCieloEcommerceSandbox.Data;
using IntegracaoCieloEcommerceSandbox.Exceptions;

namespace IntegracaoCieloEcommerceSandbox.Services
{
    public class EntityLimitService
    {
        private readonly AppDbContext _context;
        private const int MAX_ENTITIES = 5;

        public EntityLimitService(AppDbContext context)
        {
            _context = context;
        }

        public async Task ValidateEntityLimit<T>(string entityDisplayName) where T : class
        {
            var count = await _context.Set<T>().CountAsync();
            
            if (count >= MAX_ENTITIES)
            {
                throw new EntityLimitExceededException(entityDisplayName, MAX_ENTITIES);
            }
        }
    }
}
