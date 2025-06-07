using Library.Common;

namespace Eefa.WorkflowAdmin.WebApi.Domain.Databases.Entities
{
    public partial class CommodityProperty : BaseEntity
    {
         

        /// <summary>
        /// کد کالا
        /// </summary>
        public int CommodityId { get; set; } = default!;

        /// <summary>
        /// کد گروه کالا
        /// </summary>
        public int CategoryPropertyId { get; set; } = default!;

        /// <summary>
        /// واحد اندازه گیری مقدار
        /// </summary>
        public int? ValueBaseId { get; set; }

        /// <summary>
        /// مقدار
        /// </summary>
        public string? Value { get; set; }

        /// <summary>
        /// نقش صاحب سند
        /// </summary>
         

        /// <summary>
        /// ایجاد کننده
        /// </summary>
         

        /// <summary>
        /// تاریخ و زمان ایجاد
        /// </summary>
         

        /// <summary>
        /// اصلاح کننده
        /// </summary>
         

        /// <summary>
        /// تاریخ و زمان اصلاح
        /// </summary>
         

        /// <summary>
        /// آیا حذف شده است؟
        /// </summary>
        

        public virtual CommodityCategoryProperty CategoryProperty { get; set; } = default!;
        public virtual Commodity Commodity { get; set; } = default!;
        public virtual User CreatedBy { get; set; } = default!;
        public virtual User? ModifiedBy { get; set; } = default!;
        public virtual Role OwnerRole { get; set; } = default!;
        public virtual BaseValue? ValueBase { get; set; } = default!;
    }
}
