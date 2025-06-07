public class PermissionCondition : AuditableEntity
{
    public PermissionCondition()
    {

    }
    public PermissionCondition(string tablename, string conditions, string jsonCondition)
    {
        TableName = tablename;
        Condition = conditions;
        JsonCondition = jsonCondition;
    }
    public int PermissionId { get; set; }
    public string TableName { get; set; }
    public int? MenuId { get; set; }
    public string Condition { get; set; }
    public string JsonCondition { get; set; }
    //public Table Table { get; set; }


    public MenuItem MenuItem { get; set; }
    public User CreatedBy { get; set; }
    public User ModifiedBy { get; set; }
    public Role OwnerRole { get; set; }
    public Permission Permission { get; set; }
}