using Microsoft.Extensions.Caching.Hybrid;

namespace Saas.Modules.Ticketing.Application.Carts;

public sealed class CartService
{
    private static readonly HybridCacheEntryOptions _defaultEntryOptions = new()
    {
        Expiration = TimeSpan.FromMinutes(20),
        LocalCacheExpiration = TimeSpan.FromMinutes(20)
    };

    private readonly HybridCache _cache;

    public CartService(HybridCache cache)
    {
        _cache = cache;
    }

    public async Task<Cart> GetAsync(Guid customerId, CancellationToken cancellationToken = default)
    {
        var cacheKey = CreateCacheKey(customerId);

        return await _cache.GetOrCreateAsync(
            cacheKey,
            _ => ValueTask.FromResult(Cart.CreateDefault(customerId)),
            _defaultEntryOptions,
            cancellationToken: cancellationToken);
    }

    public async Task ClearAsync(Guid customerId, CancellationToken cancellationToken = default)
    {
        var cacheKey = CreateCacheKey(customerId);

        var cart = Cart.CreateDefault(customerId);

        await _cache.SetAsync(cacheKey, cart, _defaultEntryOptions, cancellationToken: cancellationToken);
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

        await _cache.SetAsync(cacheKey, cart, _defaultEntryOptions, cancellationToken: cancellationToken);
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

        await _cache.SetAsync(cacheKey, cart, _defaultEntryOptions, cancellationToken: cancellationToken);
    }

    private static string CreateCacheKey(Guid customerId) => $"carts:{customerId}";
}
