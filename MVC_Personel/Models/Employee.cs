using System;
using System.ComponentModel.DataAnnotations;

namespace MVC_Personel.Models
{
    public class Employee
    {
        public int ID { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }

        [StringLength(11, MinimumLength = 11, ErrorMessage = "Lütfen geçerli bir T.C. Kimlik numarası giriniz")]
        public string IdentityNumber { get; set; }

        [StringLength(11, MinimumLength = 11, ErrorMessage = "Lütfen geçerli bir telefon numarası giriniz.")]
        public string PhoneNumber { get; set; }
        public char Gender { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Address { get; set; }

        [StringLength(13, MinimumLength = 13, ErrorMessage = "Sigorta numarası 13 hane olmalıdır.")]
        public string RegistirationNumber { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DateOfStart { get; set; }
        public bool IsActive { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<DateTime> DateOfLeave { get; set; }
        public int DepartmentID { get; set; }
        public int PositionID { get; set; }

        public Department Department { get; set; }
        public Position Position { get; set; }
    }
}
