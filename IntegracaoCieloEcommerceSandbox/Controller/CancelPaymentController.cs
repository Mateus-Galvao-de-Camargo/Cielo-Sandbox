using Microsoft.AspNetCore.Mvc;
using IntegracaoCieloEcommerceSandbox.Services;

namespace IntegracaoCieloEcommerceSandbox.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class CancelPaymentController : ControllerBase
    {
        private readonly CieloService _cieloService;

        public CancelPaymentController(CieloService cieloService)
        {
            _cieloService = cieloService;
        }

        [HttpPut("{paymentId}")]
        public async Task<IActionResult> CancelarPagamento(string paymentId)
        {
            try
            {
                var resultado = await _cieloService.CancelarPagamento(paymentId);
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
