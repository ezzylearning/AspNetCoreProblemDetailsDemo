using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreProblemDetailsDemo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductsController : ControllerBase
    {
        //public IActionResult Get()
        //{
        //    throw new Exception("Product Not Found.");
        //}

        //public IActionResult Get()
        //{
        //    return NotFound(new ProblemDetails
        //    {
        //        Title = "Product Not Found",
        //        Status = StatusCodes.Status404NotFound,
        //        Detail = "The requested product could not be found."
        //    });
        //}

        public IActionResult Get()
        {
            return Problem(
                type: "Bad Request",
                title: "Invalid request",
                detail: "The provided request parameters are invalid.",
                statusCode: StatusCodes.Status400BadRequest);
        }
    }
}
