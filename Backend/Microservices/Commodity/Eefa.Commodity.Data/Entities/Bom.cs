using Eefa.Common.Domain;
using System.Collections.Generic;

namespace Eefa.Commodity.Data.Entities
{
    public partial class Bom : DomainBaseEntity
    {

        /// <summary>
        /// کد والد
        /// </summary>
        public int? RootId { get; set; }
        public string Title { get; set; } = default!;
        public string Description { get; set; } = default!;
        public bool IsActive { get; set; } = default!;

        /// <summary>
        /// کد سطح
        /// </summary>
        public string LevelCode { get; set; } = default!;

        /// <summary>
        /// کد گروه کالا
        /// </summary>
        public int CommodityCategoryId { get; set; } = default!;

        public virtual CommodityCategory CommodityCategory { get; set; } = default!;
        public virtual ICollection<BomItem> Items { get; set; } = default!;
        public virtual ICollection<BomValueHeader> BomValueHeaders { get; set; } = default!;
        
        public BomItem AddItem(BomItem bomItem)
        {
            Items ??= new List<BomItem>();
            this.Items.Add(bomItem);
            return bomItem;
        }
    }
  
}
