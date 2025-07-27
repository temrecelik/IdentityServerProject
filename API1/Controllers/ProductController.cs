using API1.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API1.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]


    public class ProductController : ControllerBase
    {
        [Authorize(Policy = "Read")]
        [HttpGet]
        public IActionResult GetProducts()
        {
            List<GetProductDto>  products = new List<GetProductDto>() {

                new GetProductDto(){name="ürün1" ,Category="Kozmetik" ,price=100},
                new GetProductDto(){name="ürün1" ,Category="Temizlik" ,price=200},
                new GetProductDto(){name="ürün1" ,Category="Oyuncak" , price=300},
            };

            return Ok(products);
        }

        [Authorize(Policy ="UpdateOrCreateOrDelete")]
        [HttpPut]
        public IActionResult UpdateProduct(int id)
        {
            return Ok($"id değeri {id} olan ürün güncellenmiştir");

        }

        [Authorize(Policy = "UpdateOrCreateOrDelete")]
        [HttpPost]
        public IActionResult CreateProduct(CreateProductDto productDto)
        {
            return Ok("Product güncelleme işlemi başarılı");
        }

        [Authorize(Policy = "UpdateOrCreateOrDelete")]
        [HttpDelete]
        public IActionResult DeleteProduct(int id)
        {
            return Ok($"id değeri {id} olan ürün silinmiştir.");
        }
    }
}
