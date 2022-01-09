using FluentValidation;
using Repository.Models;

namespace Repository.Validators
{
    public class ProductValidator : AbstractValidator<Product>
    {
        public ProductValidator()
        {
            RuleFor(p => p.Name).NotEmpty().WithMessage("Error: Property Name is null, empty or whitespace");
            RuleFor(p => p.StoreName).NotEmpty().WithMessage("Error: Property Name is null, empty or whitespace");
            RuleFor(p => p.Price).NotNull().GreaterThanOrEqualTo(0).WithMessage("Error: Property Price should be greater thn or equal to zero");
        }
    }
}