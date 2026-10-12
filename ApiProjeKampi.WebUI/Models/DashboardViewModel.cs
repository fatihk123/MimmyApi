using ApiProjeKampi.WebUI.Dtos.ReservationDtos;
using ApiProjeKampi.WebUI.Dtos.MessageDtos;
namespace ApiProjeKampi.WebUI.Models;

public class DashboardViewModel
{
    public int ProductCount { get; set; }
    public int CategoryCount { get; set; }
    public int ChefCount { get; set; }
    public int ReservationCount { get; set; }
    public int GuestCount { get; set; }
    public int PendingReservationCount { get; set; }
    public int UnreadMessageCount { get; set; }
    public List<ResultReservationDto> LatestReservations { get; set; } = new();
    public List<ResultMessageDto> LatestMessages { get; set; } = new();
}
