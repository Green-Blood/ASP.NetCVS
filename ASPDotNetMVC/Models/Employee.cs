using System.ComponentModel.DataAnnotations;

namespace ASPDotNetMVC.Models
{
    public class Employee
    {
         
        [Key]
        public string Payroll_Number { get; set; }
        public string Forename { get; set; }
        public string Surname { get; set; }

        public string Date_Of_Birth { get; set; }

        public string PhoneNumber { get; set; }
        public string MobileNumber { get; set; }
        public string Address { get; set; }
        public string Address_2 { get; set; }
        public string Postcode { get; set; }

        public string Email { get; set; }

        public string Start_Date { get; set; }
    }
}