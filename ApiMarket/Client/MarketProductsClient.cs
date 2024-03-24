
namespace ApiClientMarket.Client
{
    public class MarketProductsClient : IMarketProductsClient
    {
        private readonly HttpClient client = new HttpClient();
        public async Task<bool> ProductExistsAsync(Guid productId)
        {
            using HttpResponseMessage response = await client.GetAsync($"http://warehousehost:8080/Product/ExistsProduct?existingProductId={productId.ToString()}");

            response.EnsureSuccessStatusCode(); // проверяет ответ на правильность

            string responseBody = await response.Content.ReadAsStringAsync();

            if (responseBody == "true")
            {
                return true;
            }

            if (responseBody == "false")
            {
                return false;
            }

            throw new Exception("Unknown response");
        }
    }
}
