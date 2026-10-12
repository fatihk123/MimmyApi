using System.ComponentModel.DataAnnotations;

namespace ApiProjeKampi.WebUI.Models;

public sealed class AiRequestViewModel
{
    [Required(ErrorMessage = "Malzemeleri veya isteğinizi yazın.")]
    [StringLength(2000, MinimumLength = 3, ErrorMessage = "İstek 3 ile 2000 karakter arasında olmalıdır.")]
    public string Prompt { get; set; } = "";
    public string? Result { get; set; }
    public bool IsConfigured { get; set; }
}
