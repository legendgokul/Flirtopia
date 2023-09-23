
using System.ComponentModel.DataAnnotations;

public class RegisterDTO
{
    [Required]
    [MinLength(8)]
    public String userName {get;set;}

     [Required]
    public String password {get;set;}
}