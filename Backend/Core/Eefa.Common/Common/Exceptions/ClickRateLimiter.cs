using Microsoft.Extensions.Caching.Memory;
using System;

namespace Eefa.Common.Common.Exceptions
{
    public class ClickRateLimiter
    {
     

            private readonly IMemoryCache memoryCache = new MemoryCache(new MemoryCacheOptions());

            public bool CanSubmit(int userId, int allowedClickIntervalSeconds, int allowedClickCount)
            {
                // اگر کاربر وجود ندارد یا زمان آخرین کلیک برای او ثبت نشده باشد، اجازه ثبت کلیک جدید بدهید
                if (!memoryCache.TryGetValue(userId, out ClickInfo clickInfo))
                {
                    clickInfo = new ClickInfo { ClickTime = DateTime.Now, ClickCount = 1 };
                    memoryCache.Set(userId, clickInfo, TimeSpan.FromSeconds(allowedClickIntervalSeconds));
                    return true;
                }

                // اگر تعداد کلیک‌ها در مدت زمان مجاز بیشتر از مقدار مجاز شود، اجازه ثبت کلیک جدید ندهید
                if (clickInfo.ClickCount >= allowedClickCount)
                {
                    return false;
                }

                // اگر زمان گذشته از مدت زمان مجاز بیشتر شود، تعداد کلیک‌ها را صفر کنید
                if ((DateTime.Now - clickInfo.ClickTime).TotalSeconds > allowedClickIntervalSeconds)
                {
                    clickInfo.ClickTime = DateTime.Now;
                    clickInfo.ClickCount = 0;
                }

                // افزایش تعداد کلیک‌ها و آپدیت زمان کلیک
                clickInfo.ClickCount++;
                memoryCache.Set(userId, clickInfo, TimeSpan.FromSeconds(allowedClickIntervalSeconds));

                return true;
            }

            private class ClickInfo
            {
                public DateTime ClickTime { get; set; }
                public int ClickCount { get; set; }
            }
        }
    }
 
