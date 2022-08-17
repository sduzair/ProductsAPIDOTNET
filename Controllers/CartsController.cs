using Microsoft.AspNetCore.Mvc;
using ProductsAPI.Models;
using ProductsAPI.Services;
//using Microsoft.AspNetCore.Http;
//using Microsoft.Net.Http.Headers;

namespace ProductsAPI.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class CartsController : ControllerBase
{
    private readonly ProductsService _productsService;
    //private readonly IHttpContextAccessor _httpContextAccessor;

    public CartsController(ProductsService productsService)
    {
        _productsService = productsService;
        //_httpContextAccessor = new HttpContextAccessor();
    }

    [HttpGet]
    public async Task<Cart> Get()
    {
        string? sessionId = Request.Cookies["sessionId"];
        if (sessionId == null)
        {
            Cart NewCart = new();
            NewCart = await _productsService.CreateCartAsync(NewCart);
            sessionId = NewCart.SessionId;
            CookieOptions option = new()
            {
                Expires = DateTime.Now.AddMilliseconds(1000 * 60 * 60 * 24)
            };
            Response.Cookies.Append("sessionId", NewCart.SessionId, option);
        }
        //Console.WriteLine("Getting cart from session Id:");
        //Console.WriteLine(sessionId);
        //Console.WriteLine("Response:");
        var res = await _productsService.GetCartAsync(sessionId);
        //Console.WriteLine(res.ToString());
        return res;
    }

    [HttpPut]
    public async Task<ActionResult> AddOneProduct([FromBody] ProductCarted p)
    {
        string? SessionId = Request.Cookies["sessionId"];
        if (SessionId == null)
        {
            return BadRequest("No session");
        }
        var res = await _productsService.UpdateCartAsync(SessionId, p);
        if (res.ModifiedCount == 0)
        {
            return BadRequest(res);
        }
        return Ok(res);
    }

    [HttpPut]
    public async Task<ActionResult> RemoveOneProduct([FromBody] ProductCarted p)
    {
        string? SessionId = Request.Cookies["sessionId"];
        if (SessionId == null)
        {
            return BadRequest("No session");
        }
        var res = await _productsService.RemoveOneProduct(SessionId, p);
        return Ok(res);
    }

    [HttpPut]
    public async Task<ActionResult> RemoveAllProductsOfOneType([FromBody] ProductCarted p)
    {
        string? SessionId = Request.Cookies["sessionId"];
        if (SessionId == null)
        {
            return BadRequest("No session");
        }
        var res = await _productsService.RemoveAllProductsOfOneType(SessionId, p.Id);
        return Ok(res);
    }

    // GET api/<ValuesController>/5
    //[HttpGet("{id}")]
    //public string Get(int id)
    //{
    //    return "value";
    //}

    //// POST api/<ValuesController>
    //[HttpPost]
    //public void Post([FromBody] string value)
    //{
    //    string? SessionId = Request.Cookies["sessionId"]; 
    //    if (SessionId == null)
    //}

    // PUT api/<ValuesController>/5
    //[HttpPut("{id}")]
    //public void Put(int id, [FromBody] string value)
    //{
    //}

    //// DELETE api/<ValuesController>/5
    //[HttpDelete("{id}")]
    //public void Delete(int id)
    //{
    //}
}
