using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace IntegracaoCieloEcommerceSandbox.Models
{
    public class Transacao
    {
        [Key]
        public int Id { get; set; }

        public Cartao Cartao { get; set; } = null!;

        [Required(ErrorMessage = "Por favor, selecione um cartão válido.")]
        public int CartaoId { get; set; }

        [Required(ErrorMessage = "Por favor, insira um valor válido.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "O valor deve ser maior que zero.")]
        public double Valor { get; set; }

        public string? EstadoDaTransacao { get; set; }

        public string? PaymentId { get; set; }

        public string? Log { get; set; }
    }
}

