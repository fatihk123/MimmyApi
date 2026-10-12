using ApiProjeKampi.WebApi.Context;
using ApiProjeKampi.WebApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace ApiProjeKampi.WebApi.Data;

public static class DemoDataSeeder
{
    public static async Task SeedAsync(ApiContext context)
    {
        // Explicit opt-in only; never modify an existing restaurant's data.
        if (await context.Categories.AnyAsync() || await context.Products.AnyAsync() || await context.Reservations.AnyAsync() || await context.Messages.AnyAsync() || await context.Features.AnyAsync() || await context.Abouts.AnyAsync() || await context.Contacts.AnyAsync() || await context.Chefs.AnyAsync() || await context.Services.AnyAsync() || await context.Testimonials.AnyAsync() || await context.MimmyEvents.AnyAsync() || await context.Images.AnyAsync() || await context.Notifications.AnyAsync()) return;
        const string img = "/yummy-red-1.0.0/assets/img/";
        var starters = new Category { CategoryName = "Başlangıçlar" };
        var mains = new Category { CategoryName = "Ana Yemekler" };
        context.Categories.AddRange(starters, mains);
        context.Products.AddRange(
            new Product { ProductName = "Mimmy Salata", ProductDescription = "Taze mevsim yeşillikleri ve özel sos", Price = 180, ImageUrl = img + "menu/menu-item-1.png", Category = starters },
            new Product { ProductName = "Sebzeli Tabak", ProductDescription = "Mevsim sebzeleri ile hafif bir başlangıç", Price = 220, ImageUrl = img + "menu/menu-item-2.png", Category = starters },
            new Product { ProductName = "Şefin Özel Tabağı", ProductDescription = "Mimmy mutfağından özenle hazırlanmış bir lezzet", Price = 420, ImageUrl = img + "menu/menu-item-3.png", Category = mains });
        context.Features.Add(new Feature { Title = "Lezzetli Yemekler, Keyifli Anlar", SubTitle = "Mimmy'ye Hoş Geldiniz", Description = "Taze malzemeler, özenli tarifler ve samimi bir sofra. Bir sonraki buluşmanız Mimmy'de olsun.", ImageUrl = img + "hero-img.png", VideoUrl = "" });
        context.Abouts.Add(new About { Title = "Her Sofrada Yeni Bir Hikâye", Descrption = "Mimmy, lezzetli yemeklerin insanları bir araya getirdiği bir buluşma noktasıdır. Taze ürünler ve özenli sunumlarla her ziyaretinizi özel kılmak için çalışıyoruz.", ImageUrl = img + "about.jpg", VideoCoverUrl = img + "about-2.jpg", VideoUrl = "", ReservationNumber = "Rezervasyon formunu kullanabilirsiniz." });
        context.Services.AddRange(
            new Service { Title = "Taze Malzemeler", Description = "Mevsiminde ürünlerle hazırlanan yemekler.", IconUrl = "bi bi-basket" },
            new Service { Title = "Özenli Sunum", Description = "Her tabakta kalite ve lezzet.", IconUrl = "bi bi-gem" },
            new Service { Title = "Samimi Ortam", Description = "Sevdiklerinizle keyifli bir buluşma.", IconUrl = "bi bi-heart" });
        context.Chefs.Add(new Chef { NameSurName = "Mimmy Mutfak Ekibi", Title = "Şef Ekibi", Description = "Her yemeği özenle hazırlayan ekibimiz.", ImageUrl = img + "chefs/chefs-1.jpg" });
        context.Testimonials.Add(new Testimonial { NameSurName = "Örnek Misafir", Title = "Demo yorumu", Comment = "Keyifli bir ortam ve özenle hazırlanmış lezzetler.", ImageUrl = img + "testimonials/testimonials-1.jpg" });
        context.MimmyEvents.Add(new MimmyEvent { Title = "Özel Buluşmalar", Description = "Kutlamalar ve özel günler için Mimmy.", Price = 500, Status = true, ImageUrl = img + "events-1.jpg" });
        context.Contacts.Add(new Contact { Address = "İstanbul · Demo restoran adresi", Email = "demo@example.com", Phone = "Demo iletişim bilgisi", OpenHours = "Pazartesi–Cumartesi 11.00–23.00 · Örnek çalışma saatleri", MapLocation = "https://maps.google.com/maps?q=Istanbul&output=embed" });
        for (var i = 1; i <= 8; i++) context.Images.Add(new Image { Title = $"Mimmy Galeri {i}", ImageUrl = img + $"gallery/gallery-{i}.jpg" });
        await context.SaveChangesAsync();
    }
}
