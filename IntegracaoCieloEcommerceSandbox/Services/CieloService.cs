using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using IntegracaoCieloEcommerceSandbox.Models;

namespace IntegracaoCieloEcommerceSandbox.Services
{
    public class CieloService
{
    private readonly HttpClient _httpClient;
    private readonly string _merchantId = null!;
    private readonly string _merchantKey = null!;
    private readonly string _apiUrl = null!;

    public CieloService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _merchantId = configuration["Cielo:MerchantId"]!;
        _merchantKey = configuration["Cielo:MerchantKey"]!;
        _apiUrl = configuration["Cielo:CieloApiCriaTransacaoUrl"]!;
    }

    public async Task<string> CreatePayment(Transacao Transacao)
    {
        var Cartao = Transacao.Cartao;
        var paymentRequest = new
        {
            MerchantOrderId = Transacao.Id,
            Customer = new
            {
                Name = "Goku League of Legends da Silva"
            },
            Payment = new
            {
                Type = "CreditCard",
                Amount = Transacao.Valor, // Valor em centavos
                Installments = 1, // Parcelas
                SoftDescriptor = "Loja",
                CreditCard = new
                {
                    CardNumber = Cartao.NumeroDoCartao,
                    Holder = Cartao.NomeNoCartao,
                    ExpirationDate = Cartao.Validade,
                    SecurityCode = Cartao.Cvv,
                    Brand = Cartao.Bandeira
                }
            }
        };

        var jsonContent = JsonConvert.SerializeObject(paymentRequest);
        var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
        
        // Adicionar os cabeçalhos de autenticação
        _httpClient.DefaultRequestHeaders.Add("MerchantId", _merchantId);
        _httpClient.DefaultRequestHeaders.Add("MerchantKey", _merchantKey);

        var response = await _httpClient.PostAsync(_apiUrl, content);

        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadAsStringAsync();
            return result;
        }
        else
        {
            var error = await response.Content.ReadAsStringAsync();
            throw new Exception($"Erro na requisição: {response.StatusCode} - {error}");
        }
    }

    public async Task<string> CancelarPagamento(string paymentId)
    {
        var cancelUrl = $"{_apiUrl}/{paymentId}/void";

        _httpClient.DefaultRequestHeaders.Add("MerchantId", _merchantId);
        _httpClient.DefaultRequestHeaders.Add("MerchantKey", _merchantKey);

        var response = await _httpClient.PutAsync(cancelUrl, null);

        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadAsStringAsync();
            return result;
        }
        else
        {
            var error = await response.Content.ReadAsStringAsync();
            throw new Exception($"Erro ao cancelar o pagamento: {response.StatusCode} - {error}");
        }
    }

    public async Task<string> CapturePay(string paymentId)
    {
        var captureUrl = $"{_apiUrl}/{paymentId}/capture";

        _httpClient.DefaultRequestHeaders.Add("MerchantId", _merchantId);
        _httpClient.DefaultRequestHeaders.Add("MerchantKey", _merchantKey);

        var response = await _httpClient.PutAsync(captureUrl, null);

        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadAsStringAsync();
            return result;
        }
        else
        {
            var error = await response.Content.ReadAsStringAsync();
            throw new Exception($"Erro ao cancelar o pagamento: {response.StatusCode} - {error}");
        }
    }
}
}
