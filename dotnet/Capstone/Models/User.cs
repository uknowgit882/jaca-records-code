using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Capstone.Models
{
    /// <summary>
    /// Model to accept registration parameters
    /// </summary>
    public class RegisterUser
    {
        [Required]
        [MinLength(2, ErrorMessage = "Please create a unique username")]
        public string Username { get; set; }

        [Required]
        [MinLength(2, ErrorMessage = "Please enter your first name, or an alias")]
        public string First_Name { get; set; }

        [Required]
        [MinLength(2, ErrorMessage = "Please enter your last name, or an alias")]
        public string Last_Name { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Please enter a valid eamail address")]
        public string Email_Address { get; set; }

        [Required]
        [MinLength(12, ErrorMessage = "Password must be at least 12 characters")]
        public string Password { get; set; }

        [Required]
        [Compare(nameof(Password), ErrorMessage = "Please ensure your password matches")]
        public string ConfirmPassword { get; set; }

        public string Role { get; set; } = "free";
    }

    /// <summary>
    /// Model to accept login parameters
    /// </summary>
    public class LoginUser
    {
        [Required(ErrorMessage = "Please enter your username")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Please enter your password")]
        public string Password { get; set; }
    }

    /// <summary>
    /// Model to return upon successful login (user data + token)
    /// </summary>
    public class LoginResponse
    {
        public ReturnUser User { get; set; }
        public string Token { get; set; }
    }

    /// <summary>
    /// Model of user data to return upon successful login
    /// </summary>
    public class ReturnUser
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Role { get; set; }
        public DateTime Last_Login { get; set; }
    }

    /// <summary>
    /// User object used in C# code. Not sent to front end
    /// </summary>
    public class User
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string First_Name { get; set; }
        public string Last_Name { get; set; }
        public string Email_Address { get; set; }
        public string PasswordHash { get; set; }
        public string Salt { get; set; }
        public string Role { get; set; }
        public bool IsActive { get; set; }
        public DateTime Created_Date { get; set; }
        public DateTime Updated_Date { get; set; }
        public DateTime Last_Login { get; set; }
    }
}
