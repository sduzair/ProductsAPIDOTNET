using Microsoft.AspNetCore.SignalR;

namespace ProductsAPI.Hubs;

public class ProductsHub : Hub
{
    public async Task CartProduct(string productId, int quantity = 1)
    {
        await Clients.All.SendAsync("UpdateProductQuantity", productId, quantity);
    }
}
