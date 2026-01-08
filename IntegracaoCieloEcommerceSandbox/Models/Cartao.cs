using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace IntegracaoCieloEcommerceSandbox.Models
{
    public class Cartao
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "O cartão é obrigatório.")]
        [StringLength(16, MinimumLength = 13, ErrorMessage = "O cartão deve ter 13 (Visa) ou 16 (Visa e MasterCard) dígitos.")]
        [RegularExpression(@"^(4\d{12}(\d{3})?|5\d{15})$", ErrorMessage = "O cartão deve começar com 4 (Visa, pode ter 13 ou 16 dígitos) ou 5 (Mastercard, deve ter 16 dígitos) e conter apenas números")]
        public string NumeroDoCartao { get; set; } = null!;

        [Required(ErrorMessage = "O código de segurança é obrigatório.")]
        [StringLength(3, MinimumLength = 3, ErrorMessage = "O CVV deve ter 3 dígitos.")]
        [RegularExpression(@"^\d{3}$", ErrorMessage = "Somente números são permitidos.")]
        public string Cvv { get; set; } = null!;

        [Required(ErrorMessage = "O mês de validade é obrigatório.")]
        [Range(1, 12, ErrorMessage = "O mês deve ser entre 1 e 12.")]
        public int Mes { get; set; }

        [Required(ErrorMessage = "O ano de validade é obrigatório.")]
        [Range(2024, 2100, ErrorMessage = "O ano deve ser entre 2024 e 2100.")]
        public int Ano { get; set; }

        public string? Validade { get; set; } = null!;

        [Required(ErrorMessage = "O nome no cartão é obrigatório.")]
        [LettersOnly(ErrorMessage = "O nome deve conter apenas letras e espaços em branco simples.")]
        public string NomeNoCartao { get; set; } = null!;

        public string? Bandeira { get; set; }
    }

    public class LettersOnlyAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is string stringValue)
            {
                // Verifica se contém apenas letras e espaços
                if (!Regex.IsMatch(stringValue, @"^[a-zA-Z ]+$"))
                {
                    return new ValidationResult("O nome do titular deve conter apenas letras.");
                }
            }

            return ValidationResult.Success;
        }
    }
}
