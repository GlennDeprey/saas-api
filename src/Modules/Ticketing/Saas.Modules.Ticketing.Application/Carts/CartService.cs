using Microsoft.Extensions.Caching.Hybrid;

namespace Saas.Modules.Ticketing.Application.Carts;

public sealed class CartService
{
    private static readonly HybridCacheEntryOptions _cacheOptions = new()
    {
        Expiration = TimeSpan.FromMinutes(20),
        LocalCacheExpiration = TimeSpan.FromMinutes(20)
    };

    private readonly HybridCache _hybridCache;

    public CartService(HybridCache hybridCache)
    {
        _hybridCache = hybridCache;
    }

    public async Task<Cart> GetAsync(Guid customerId, CancellationToken cancellationToken = default)
    {
        var cacheKey = CreateCacheKey(customerId);

        return await _hybridCache.GetOrCreateAsync(
            cacheKey,
            _ => ValueTask.FromResult(Cart.CreateDefault(customerId)),
            _cacheOptions,
            cancellationToken: cancellationToken);
    }

    public async Task ClearAsync(Guid customerId, CancellationToken cancellationToken = default)
    {
        var cacheKey = CreateCacheKey(customerId);

        var cart = Cart.CreateDefault(customerId);

        await _hybridCache.SetAsync(cacheKey, cart, _cacheOptions, cancellationToken: cancellationToken);
    }

    public async Task AddItemAsync(Guid customerId, CartItem cartItem, CancellationToken cancellationToken = default)
    {
        var cacheKey = CreateCacheKey(customerId);

        var cart = await GetAsync(customerId, cancellationToken);

        var existingCartItem = cart.Items.Find(c => c.TicketTypeId == cartItem.TicketTypeId);

        if (existingCartItem is null)
        {
            cart.Items.Add(cartItem);
        }
        else
        {
            existingCartItem.Quantity += cartItem.Quantity;
        }

        await _hybridCache.SetAsync(cacheKey, cart, _cacheOptions, cancellationToken: cancellationToken);
    }

    public async Task RemoveItemAsync(Guid customerId, Guid ticketTypeId, CancellationToken cancellationToken = default)
    {
        var cacheKey = CreateCacheKey(customerId);

        var cart = await GetAsync(customerId, cancellationToken);

        var cartItem = cart.Items.Find(c => c.TicketTypeId == ticketTypeId);

        if (cartItem is null)
        {
            return;
        }

        cart.Items.Remove(cartItem);

        await _hybridCache.SetAsync(cacheKey, cart, _cacheOptions, cancellationToken: cancellationToken);
    }

    private static string CreateCacheKey(Guid customerId) => $"carts:{customerId}";
}
