using System.ComponentModel.DataAnnotations;

namespace ApiProject.Data.CustomModels;
public class LoginDTO
{

    [Required]
    public string username{get;set;}

    [Required]
    [StringLength(8,MinimumLength = 4,ErrorMessage = "pls set password with minimum of 4 character and max of 8 character")]
    public string password{get;set;}

}