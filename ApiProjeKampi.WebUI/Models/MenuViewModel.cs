using ApiProjeKampi.WebUI.Dtos.CategoryDtos;
using ApiProjeKampi.WebUI.Dtos.ProductDtos;
namespace ApiProjeKampi.WebUI.Models;

public class MenuViewModel
{
    public List<ResultCategoryDto> Categories { get; set; } = new();
    public List<ResultProductDto> Products { get; set; } = new();
}
