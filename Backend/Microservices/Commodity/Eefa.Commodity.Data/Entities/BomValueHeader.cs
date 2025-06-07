using Eefa.Common.Domain;
using System;
using System.Collections.Generic;

namespace Eefa.Commodity.Data.Entities
{
    public partial class BomValueHeader : DomainBaseEntity
    {

        /// <summary>
        /// کد فرمول ساخت
        /// </summary>
        public int BomId { get; set; } = default!;

        /// <summary>
        /// کد کالا
        /// </summary>
        public int CommodityId { get; set; } = default!;

        public string Name { get; set; } = default!;

        /// <summary>
        /// تاریخ فرمول ساخت
        /// </summary>
        public DateTime BomDate { get; set; } = default!;

        public virtual Bom Bom { get; set; } = default!;
        public virtual Commodity Commodity { get; set; } = default!;
        public virtual ICollection<BomValue> BomValues { get; set; } = default!;
        public BomValue AddItem(BomValue bomItem)
        {
            BomValues ??= new List<BomValue>();
            this.BomValues.Add(bomItem);
            return bomItem;
        }
    }
}
