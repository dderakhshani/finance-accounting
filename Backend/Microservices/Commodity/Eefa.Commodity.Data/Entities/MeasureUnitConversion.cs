using Eefa.Common.Data;

namespace Eefa.Commodity.Data.Entities
{
    public partial class MeasureUnitConversion : BaseEntity
    {

        /// <summary>
        /// واحد اندازه گیری اولیه
        /// </summary>
        public int SourceMeasureUnitId { get; set; } = default!;

        /// <summary>
        /// واحد اندازه گیری ثانویه
        /// </summary>
        public int DestinationMeasureUnitId { get; set; } = default!;

        /// <summary>
        /// ضریب تبدیل
        /// </summary>
        public double? Ratio { get; set; }

        public virtual MeasureUnit DestinationMeasureUnit { get; set; } = default!;
        public virtual MeasureUnit SourceMeasureUnit { get; set; } = default!;
    }
}
