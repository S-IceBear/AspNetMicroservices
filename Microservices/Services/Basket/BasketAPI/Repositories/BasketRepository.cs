﻿using Newtonsoft.Json;
using BasketAPI.Entities;
using BasketAPI.Repositories.Interfaces;
using Microsoft.Extensions.Caching.Distributed;

namespace BasketAPI.Repositories
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDistributedCache _redisCache;

        public BasketRepository(IDistributedCache redisChache)
        {
            _redisCache = redisChache ?? throw new ArgumentNullException(nameof(redisChache));
        }

        public async Task<ShoppingCart?> GetBasket(string userName)
        {
            string basketJson = await _redisCache.GetStringAsync(userName);
            if (string.IsNullOrWhiteSpace(basketJson)) return null;
            ShoppingCart? deserializedObject = JsonConvert.DeserializeObject<ShoppingCart>(basketJson);
            return deserializedObject;
        }

        public async Task<ShoppingCart?> UpdateBasket(ShoppingCart shoppingCart)
        {
            string serializedShoppingCartJson = JsonConvert.SerializeObject(shoppingCart);
            await _redisCache.SetStringAsync(shoppingCart.UserName, serializedShoppingCartJson);
            return await GetBasket(shoppingCart.UserName);//To insure that it's synced with Redis DB
        }

        public async Task DeleteBasket(string userName) => await _redisCache.RemoveAsync(userName);
    }
}
