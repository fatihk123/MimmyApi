using ApiProjeKampi.WebUI.Dtos.ContactDtos;
using Microsoft.AspNetCore.Mvc;
namespace ApiProjeKampi.WebUI.ViewComponents;

public class _FooterDefaultComponentPartial(IHttpClientFactory httpClientFactory) : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync()
    {
        var contacts = await httpClientFactory.CreateClient("Api").GetFromJsonAsync<List<ResultContactDto>>("api/Contacts");
        return View(contacts?.FirstOrDefault() ?? new ResultContactDto());
    }
}
