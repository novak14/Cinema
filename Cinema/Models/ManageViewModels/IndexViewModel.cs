using Order.Dal.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Cinema.Models.ManageViewModels
{
    public class IndexViewModel
    {
        public IndexViewModel() { }
        public IndexViewModel(string Username, string Email, string PhoneNumber, List<NewOrder> NewOrder)
        {
            this.Username = Username;
            this.Email = Email;
            this.PhoneNumber = PhoneNumber;
            this.NewOrder = NewOrder;
        }
        public string Username { get; set; }

        public bool IsEmailConfirmed { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Phone]
        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; }

        public string StatusMessage { get; set; }

        public List<NewOrder> NewOrder { get; set; }
    }
}
