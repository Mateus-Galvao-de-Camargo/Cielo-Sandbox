using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using IntegracaoCieloEcommerceSandbox.Models;
using IntegracaoCieloEcommerceSandbox.Services;
using IntegracaoCieloEcommerceSandbox.Exceptions;

namespace IntegracaoCieloEcommerceSandbox.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly TransacaoService _transacaoService;

        public PaymentController(TransacaoService transacaoService)
        {
            _transacaoService = transacaoService;
        }

        [HttpPost]
        public async Task<IActionResult> MakePayment(Transacao transacao, [FromQuery] int cartaoId)
        {
            try
            {
                var paymentResult = await _transacaoService.CreateTransacao(transacao, cartaoId);
                return Ok(paymentResult);
            }
            catch (EntityLimitExceededException ex)
            {
                return StatusCode(429, new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}