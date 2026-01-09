using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using IntegracaoCieloEcommerceSandbox.Models;
using IntegracaoCieloEcommerceSandbox.Services;

namespace IntegracaoCieloEcommerceSandbox.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class CapturePaymentController : ControllerBase
    {
        private readonly CieloService _cieloService;

        public CapturePaymentController(CieloService cieloService)
        {
            _cieloService = cieloService;
        }

        [HttpPost]
        public async Task<IActionResult> CaptureFullPayment(string PaymentId)
        {
            try
            {
                var paymentResult = await _cieloService.CapturePay(PaymentId);
                return Ok(paymentResult);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
