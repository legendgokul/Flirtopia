using System.ComponentModel.DataAnnotations;

namespace ApiProject.Data.CustomModels;
public class LoginDTO
{

    [Required]
    public string username{get;set;}

    [Required]
    public string password{get;set;}

}