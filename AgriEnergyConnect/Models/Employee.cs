using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AgriEnergyConnect.Models
{
    public class Employee
    {
        // Primary Key
        public int Id { get; set; }

        // Foreign Key to ApplicationUser
        [Required]
        public string ApplicationUserId { get; set; } = string.Empty;

        [ForeignKey(nameof(ApplicationUserId))]
        public ApplicationUser? ApplicationUser { get; set; }

        // Employee-specific fields
        [Required]
        public string FullName { get; set; } = string.Empty;

        public string? ContactInfo { get; set; }
    }
}
