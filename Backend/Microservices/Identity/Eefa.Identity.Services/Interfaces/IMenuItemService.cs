using System.Collections.Generic;
using Library.Interfaces;

namespace Eefa.Identity.Services.Interfaces
{
    public interface IMenuItemService : IService
    {
        string PutAllowedMenuesInNodes(IEnumerable<Data.Databases.Entities.MenuItem> menuItems);
    }
}