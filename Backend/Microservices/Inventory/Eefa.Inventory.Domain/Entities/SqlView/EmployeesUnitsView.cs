namespace Eefa.Inventory.Domain
{

    public partial class EmployeesUnitsView
    {
       
        
        [System.ComponentModel.DataAnnotations.Key]
        public int PersonId { get; set; } = default!;
        public int UnitId { get; set; } = default!;
        public int? AccountReferenceId { get; set; } = default!;
        public string FullName { get; set; } = default!;
       
    }
   

}
