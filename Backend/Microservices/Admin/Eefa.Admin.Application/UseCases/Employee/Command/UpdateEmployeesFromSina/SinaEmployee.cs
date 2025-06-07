using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eefa.Admin.Application.UseCases.Employee.Command.UpdateEmployeesFromSina
{
    public class SinaEmployee
    {
        public string EmployeeCode { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public string SSN { get; set; }
        public string NationalCode { get; set; }
        public string BirthDate { get; set; }
        public DateTime BirthDateUTC { get; set; }
        public string Mobile { get; set; }
        public string GenderName { get; set; }
        public int GenderBaseID { get; set; }
        public string MaritalStatus { get; set; }
        public int MaritalStatusID { get; set; }
        public string PositionName { get; set; }
        public string UniquePositionName { get; set; }
        public string UnitName { get; set; }
        public string UniqueUnitName { get; set; }
        public string EmploymentDate { get; set; }
        public DateTime EmploymentDateUTC { get; set; }
        public string Address { get; set; }
    }
}
