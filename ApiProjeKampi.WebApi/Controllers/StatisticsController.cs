using ApiProjeKampi.WebApi.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiProjeKampi.WebApi.Controllers;

[Route("api/[controller]"), ApiController]
public class StatisticsController(ApiContext context) : ControllerBase
{
    [HttpGet("Summary")]
    public async Task<IActionResult> Summary(CancellationToken cancellationToken)
    {
        return Ok(new
        {
            ProductCount = await context.Products.CountAsync(cancellationToken),
            CategoryCount = await context.Categories.CountAsync(cancellationToken),
            ChefCount = await context.Chefs.CountAsync(cancellationToken),
            ReservationCount = await context.Reservations.CountAsync(cancellationToken),
            GuestCount = await context.Reservations.SumAsync(x => (int?)x.CountofPeople, cancellationToken) ?? 0,
            PendingReservationCount = await context.Reservations.CountAsync(x => x.ReservationStatus == "Beklemede" || x.ReservationStatus == "Onay Bekliyor", cancellationToken),
            UnreadMessageCount = await context.Messages.CountAsync(x => !x.IsRead, cancellationToken),
            LatestReservations = await context.Reservations.AsNoTracking().OrderByDescending(x => x.ReservationID).Take(5).ToListAsync(cancellationToken),
            LatestMessages = await context.Messages.AsNoTracking().OrderByDescending(x => x.SendDate).Take(5).ToListAsync(cancellationToken)
        });
    }
}
