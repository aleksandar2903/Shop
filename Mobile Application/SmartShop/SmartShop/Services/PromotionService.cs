using SmartShop.Models;
using SmartShop.Services.RequestProvider;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SmartShop.Services
{
    public class PromotionService : IPromotionService
    {
        private readonly IRequestProvider _requestProvider;
        private readonly string url = $"{Config.APIUrl}/promotions";

        public PromotionService()
        {
            _requestProvider = DependencyService.Get<IRequestProvider>();
        }
        public async Task<Promotion> GetPromotion(int promotionId, string token = "")
        {
            var promotion = await _requestProvider.GetAsync<Promotion>($"{url}/{promotionId}", token).ConfigureAwait(false);

            return promotion ?? new Promotion();
        }

        public async Task<IEnumerable<Promotion>> GetPromotions()
        {
            var promotions = await _requestProvider.GetAsync<IEnumerable<Promotion>>($"{url}").ConfigureAwait(false);

            return promotions ?? Enumerable.Empty<Promotion>();
        }
    }
}
