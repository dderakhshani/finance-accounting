using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;

namespace Eefa.Common.Web
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ApiControllerBase : ControllerBase
    {
        private IMemoryCache _cache;

        public ApiControllerBase()
        {
           
        }

        public ApiControllerBase(IMemoryCache memoryCache)
        {
            _cache = memoryCache;
        }

        private IMediator _mediator { get; set; }

        protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();


        public T GetCacheValue<T>(string key)
        {
            object cacheEntry;
            if (!_cache.TryGetValue(key, out cacheEntry))
                return default;
            else
                return (T)cacheEntry;
        }

        /// <summary>
        /// To cache a object 
        /// This method requires Microsoft.Extensions.Caching.Memory package
        /// </summary>
        /// <param name="key">Cache Key</param>
        /// <param name="value">Object to be cached</param>
        /// <param name="options">Default is null, Default duration is 3 mintues</param>
        protected void CreateCache(string key, object value, MemoryCacheEntryOptions options = null)
        {
            if (options == null)
                options = new MemoryCacheEntryOptions
                {
                    AbsoluteExpiration = System.DateTimeOffset.Now.AddMinutes(3)
                };

            _cache.Set(key, value, options);
        }

    }
}