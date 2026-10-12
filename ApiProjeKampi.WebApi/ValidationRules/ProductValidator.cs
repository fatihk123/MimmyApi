using ApiProjeKampi.WebApi.Context;
using ApiProjeKampi.WebApi.Entities;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

public class ProductValidator : AbstractValidator<Product>
{
    private readonly ApiContext _context;

    public ProductValidator(ApiContext context)
    {
        _context = context;

        RuleFor(x => x.ProductName)
            .NotEmpty().WithMessage("Ürün adı boş geçilemez")
            .MinimumLength(2).WithMessage("Ürün adı en az 2 karakter olmalıdır")
            .MaximumLength(50).WithMessage("Ürün adı en fazla 50 karakter olmalıdır");

        RuleFor(x => x.Price)
            .NotEmpty().WithMessage("Ürün fiyatı boş geçilemez")
            .GreaterThan(0).WithMessage("Birim fiyatı 0'dan büyük olmalıdır")
            .LessThan(1000).WithMessage("Ürün fiyatı 1000’den küçük olmalıdır");

        RuleFor(x => x.CategoryId)
            .NotNull().WithMessage("Kategori Id boş geçilemez")
            .MustAsync(async (id, cancellation) =>
                await _context.Categories.AnyAsync(c => c.CategoryId == id, cancellation))
            .WithMessage("Geçerli bir kategori seçilmelidir.");
    }
}