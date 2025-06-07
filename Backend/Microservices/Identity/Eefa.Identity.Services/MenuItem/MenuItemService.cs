using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Eefa.Identity.Services.Interfaces;
using Newtonsoft.Json;

namespace Eefa.Identity.Services.MenuItem
{
    public class MenuItemService : IMenuItemService
    {
        private readonly IMapper _mapper;
        private readonly List<Node> _nodes = new();


        public MenuItemService(IMapper mapper)
        {
            _mapper = mapper;
        }

        public string PutAllowedMenuesInNodes(IEnumerable<Data.Databases.Entities.MenuItem> menuItems)
        {
           var menuItemModels = menuItems.Select(menuItem => _mapper.Map<MenuItemModel>(menuItem)).ToList();
            foreach (var menuItem in menuItemModels)
            {
                if (menuItem.ParentId is 0 or null)
                {
                    if (_nodes.All(x => x.Key != menuItem))
                    {
                        _nodes.Add(new Node() { Key = menuItem, Child = new List<Node>() });
                    }
                }
                else
                {
                    AddNode(menuItem);
                }
            }

            return JsonConvert.SerializeObject(_nodes);
        }


        private void AddNode(MenuItemModel menuItemModel)
        {
            foreach (var node in _nodes)
            {
                var a = FindTheNode(node, menuItemModel.ParentId);
                if(a is null)continue;
                if (a.Child is null)
                {
                    a.Child = new List<Node>(){ new Node() { Key = menuItemModel } };
                }
                else
                {
                    a.Child.Add(new Node() {Key = menuItemModel});
                }
            }
        }

        private Node FindTheNode(Node n, int? id)
        {
            if (id is null) return null;
            if (n.Key.Id == id)
            {
                return n;
            }
            else
            {
                if (n.Child.Count > 0)
                {
                    foreach (var node in n.Child)
                    {
                        return FindTheNode(node, id);
                    }
                }
            }
            return null;
        }
    }

    public class Node
    {
        public MenuItemModel Key;
        public List<Node> Child = new List<Node>();
    };
}