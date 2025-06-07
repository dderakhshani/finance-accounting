using System.ComponentModel.DataAnnotations;

namespace Eefa.Commodity.Data.Entities
{
    public partial class BomsView
    {
        [Key]
        public int Id { get; set; }
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

        public string CommodityCategoryTitle { get; set; }
    }
  
}
