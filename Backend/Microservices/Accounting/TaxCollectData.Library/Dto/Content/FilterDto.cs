using TaxCollectData.Library.Enums;

namespace TaxCollectData.Library.Dto.Content
{
    public record FilterDto()
    {
        public string Field { get; }

        public string Value { get; } = "";

        public OperatorType Operator { get; }

        private bool Or { get; }

        public FilterDto(string field, OperatorType operatorType, string value) : this(field, operatorType)
        {
            Value = value;
        }

        public FilterDto(string field, OperatorType operatorType, string value, bool or) : this(field, operatorType, value)
        {
            Or = or;
        }

        public FilterDto(string field, OperatorType operatorType) : this()
        {
            Field = field;
            Operator = operatorType;
        }
    }
}