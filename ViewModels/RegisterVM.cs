using System.ComponentModel.DataAnnotations;

namespace SecurityAdd.ViewModels
{
    public class RegisterVM
    {
        public string UserName { get; set; }
        public string FName { get; set; }
        public string Lname { get; set; }
        public string Address { get; set; }
        [DataType(DataType.Password)]
        public string password { get; set; }
    }
}
