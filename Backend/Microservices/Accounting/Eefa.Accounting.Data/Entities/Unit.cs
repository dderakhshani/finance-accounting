using System.Collections.Generic;
using Library.Common;

namespace Eefa.Accounting.Data.Entities
{

    public partial class Unit : BaseEntity
    {


        /// <summary>
        /// کد
        /// </summary>
         

        /// <summary>
        /// کد سطح
        /// </summary>
        public string LevelCode { get; set; } = default!;

        /// <summary>
        /// کد والد
        /// </summary>
        public int? ParentId { get; set; }

        /// <summary>
        /// عنوان
        /// </summary>
        public string Title { get; set; } = default!;

        /// <summary>
        /// کد شعبه
        /// </summary>
        public int BranchId { get; set; } = default!;

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
        

        public virtual Branch Branch { get; set; } = default!;
        public virtual User CreatedBy { get; set; } = default!;
        public virtual User? ModifiedBy { get; set; } = default!;
        public virtual Role OwnerRole { get; set; } = default!;
        public virtual Unit? Parent { get; set; } = default!;
        public virtual ICollection<Unit> InverseParent { get; set; } = default!;
        public virtual ICollection<UnitPosition> UnitPositions { get; set; } = default!;
    }
}
