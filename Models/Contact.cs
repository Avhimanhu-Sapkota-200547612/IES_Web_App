using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace IES_WebAuth_Project.Models
{
    // Contact class represents a contact entity in the application.
    public class Contact
    {
        // Unique identifier for the contact.
        public Guid Id { get; set; }

        // First name of the contact.
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        // Last name of the contact.
        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        // Email address of the contact.
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        // Address of the contact.
        [Required]
        [Display(Name = "Address")]
        public string Address { get; set; }

        // City where the contact is located.
        [Required]
        [Display(Name = "City")]
        public string City { get; set; }

        // State where the contact is located.
        [Required]
        [Display(Name = "State")]
        public string State { get; set; }

        // Zip code of the contact's location.
        [Required]
        [Display(Name = "Zip")]
        public string Zip { get; set; }

        // Status of the contact (e.g., Approved, Rejected, etc.).
        [Required]
        [Display(Name = "Status")]
        public string Status { get; set; }

        // User ID associated with the contact.
        [Required]
        [Display(Name = "User Id")]
        public string? UserId { get; set; }
    }
}
