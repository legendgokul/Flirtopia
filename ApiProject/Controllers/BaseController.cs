using ApiProject.Helpers;
using Microsoft.AspNetCore.Mvc;


namespace ApiProject.Controllers;

[ServiceFilter(typeof(LogUserActivity))]
[ApiController]
[Route("api/[controller]")] 
public class BaseController : ControllerBase
{ 


}