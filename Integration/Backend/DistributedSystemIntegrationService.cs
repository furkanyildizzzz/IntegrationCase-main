using Integration.Common;
using Integration.Service;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Integration.Backend
{
    public class DistributedSystemIntegrationService
    {
        private readonly CacheService cache = new CacheService();
        private ItemOperationBackend ItemIntegrationBackend { get; set; } = new();

        public DistributedSystemIntegrationService()
        {
        }

        public Result SaveItem(string itemContent)
        {
            bool lockAcquired = cache.LockTake(itemContent);

            if(!lockAcquired)
            {
                return new Result(false, $"Unable to acquire lock for key {itemContent}");
            }

            try
            {
                // Check the backend to see if the content is already saved.
                if (ItemIntegrationBackend.FindItemsWithContent(itemContent).Count != 0)
                {
                    return new Result(false, $"Duplicate item received with content {itemContent}.");
                }

                // Save new item.
                var item = ItemIntegrationBackend.SaveItem(itemContent);
                return new Result(true, $"Item with content {itemContent} saved with id {item.Id}");
            }
            finally
            {
                cache.LockRelease(itemContent);
            }
        }

        public List<Item> GetAllItems()
        {
            return ItemIntegrationBackend.GetAllItems();
        }

    }
}
