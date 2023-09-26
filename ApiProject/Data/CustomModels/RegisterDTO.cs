using System.ComponentModel.DataAnnotations;

namespace ApiProject.Data.CustomModels;
public class RegisterDTO
{
    [Required]
    [MinLength(8)]
    public String userName {get;set;}

     [Required]
    public String password {get;set;}
}