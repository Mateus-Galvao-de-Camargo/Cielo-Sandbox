using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using IntegracaoCieloEcommerceSandbox.Models;

namespace IntegracaoCieloEcommerceSandbox.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly CieloService _cieloService;

        public PaymentController(CieloService cieloService)
        {
            _cieloService = cieloService;
        }

        [HttpPost]
        public async Task<IActionResult> MakePayment(Transacao Transacao)
        {
            try
            {
                var paymentResult = await _cieloService.CreatePayment(Transacao);
                return Ok(paymentResult);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}