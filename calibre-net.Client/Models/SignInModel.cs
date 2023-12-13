
using System.ComponentModel.DataAnnotations;

namespace calibre_net.Client.Models;
public class SignInModel
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;

    [Display(Name = "Remember me?")]
    public bool RememberMe { get; set; }

    public string? ReturnUrl { get; set; }
    public object? User { get; set; }
    public string? TwoFactorCode { get; set; }
    public bool RememberMachine { get; set; }

    public byte[]? CredentialId {get;set; }
}