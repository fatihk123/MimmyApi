using ApiProjeKampi.WebApi.Context;
using ApiProjeKampi.WebApi.Dtos.MessageDtos;
using ApiProjeKampi.WebApi.Dtos.ProductDtos;
using ApiProjeKampi.WebApi.Entities;
using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace ApiProjeKampi.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ApiContext _context;
        private readonly IValidator<Product> _validator;
        private readonly IMapper _mapper;

        public ProductsController(ApiContext context, IValidator<Product> validator, IMapper mapper)
        {
            _context = context;
            _validator = validator;
            _mapper = mapper;
        }


        [HttpGet]

        public IActionResult ProductList()
        {
            var value = _context.Products.ToList();

            return Ok(_mapper.Map<List<ResultProductDto>>(value));

        }
        [HttpPost]
        public async Task<IActionResult> CreateProduct(CreateProductDto createProductDto)
        {
            var value = _mapper.Map<Product>(createProductDto);

            var validationResult = await _validator.ValidateAsync(value); 

            if (validationResult.IsValid)
            {
                _context.Products.Add(value);
                await _context.SaveChangesAsync();
                return Ok("Ekleme İşlemi Başarılı");
            }
            else
            {
                return BadRequest(validationResult.Errors.Select(x => x.ErrorMessage));
            }
        }
        [HttpDelete]
        public IActionResult DeleteProduct(int id)
        {
            var value = _context.Products.Find(id);
            _context.Products.Remove(value);
            _context.SaveChanges();
            return Ok("Silme İşlemi Başarılı");
        }


        [HttpPut]
        public async Task<IActionResult> UpdateProduct(UpdateProductDto updateProductDto)
        {
            var value = _mapper.Map<Product>(updateProductDto);
            var validationResult =await _validator.ValidateAsync(value);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors.Select(x => x.ErrorMessage));
            }
            else
            {
                _context.Products.Update(value);
                _context.SaveChanges();
                return Ok("Ürün güncelleme işlemi başarılı");
            }
        }



        [HttpGet("GetById")]
        public IActionResult GetByIdProduct(int id)
        {
            var value = _context.Products.Find(id);
            return Ok(_mapper.Map<ResultProductDto>(value));
        }

        

        [HttpGet("GetProductsWithCategory")]

       public IActionResult GetProductsWithCategory()
        {
            var value = _context.Products.Include(x=>x.Category).ToList();
            return Ok(_mapper.Map<List<ResultProductWithCategoryDto>>(value));

        }



    }
}
