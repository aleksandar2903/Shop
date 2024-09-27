using SmartShop.Models;
using SmartShop.Models.Request;
using SmartShop.Services.RequestProvider;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SmartShop.Services
{
    public class OrderService : IOrderService
    {
        private readonly string url = $"{Config.APIUrl}/sales";
        private readonly IRequestProvider _requestProvider;
        public OrderService()
        {
            _requestProvider = DependencyService.Get<IRequestProvider>();
        }
        public async Task<string> AddOrderAsync(OrderRequest request, string token)
        {
            return await _requestProvider.PostAsync<OrderRequest, string>(url, request, token);
        }

        public async Task<IEnumerable<Order>> GetOrdersAsync(string token)
        {
            var orders = await _requestProvider.GetAsync<IEnumerable<Order>>(url, token);

            return orders ?? Enumerable.Empty<Order>();
        }
    }
}
