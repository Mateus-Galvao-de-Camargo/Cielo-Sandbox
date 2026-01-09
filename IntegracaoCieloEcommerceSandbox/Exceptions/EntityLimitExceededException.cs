namespace IntegracaoCieloEcommerceSandbox.Exceptions
{
    public class EntityLimitExceededException : Exception
    {
        public EntityLimitExceededException(string entityName, int limit)
            : base($"Limite de {limit} {entityName}(s) atingido. Este é um ambiente sandbox com recursos limitados.")
        {
        }
    }
}
