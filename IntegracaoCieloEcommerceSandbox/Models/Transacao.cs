using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;

namespace IntegracaoCieloEcommerceSandbox.Models
{
    public class Transacao
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Por favor, insira um valor válido.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "O valor deve ser maior que zero.")]
        public double Valor { get; set; }

        public string NumeroDoCartao { get; set; } = null!;

        public string NomeNoCartao { get; set; } = null!;

        public string? EstadoDaTransacao { get; set; }

        public string? PaymentId { get; set; }

        public string? Log { get; set; }

        [NotMapped]
        [Range(1, int.MaxValue, ErrorMessage = "Por favor, selecione um cartão.")]
        public int CartaoId { get; set; }

        
    }
}

