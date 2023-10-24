using System.ComponentModel.DataAnnotations;

namespace ApiProject.Data.CustomModels;
public class RegisterDTO
{
    [Required] [MinLength(4)] public String username {get;set;}

    [Required] public String KnownAs {get;set;}
    [Required] public String Gender {get;set;}
    [Required] public DateTime DateOfBirth {get;set;}  
    [Required] public String City {get;set;}
    [Required] public String Country {get;set;}
    [Required] [StringLength(16,MinimumLength=4)] public String password {get;set;}

}