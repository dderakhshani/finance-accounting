public class DynamicFilter
{
    public string PropertyName { get; set; }
    public string Comparison { get; set; }
    public string NextOperand { get; set; }
    public object[] Values { get; set; }
}
