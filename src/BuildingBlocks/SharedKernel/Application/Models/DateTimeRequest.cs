using FluentValidation;

namespace SharedKernel.Application;

public class DateTimeRequest
{
    public long FromDate { get; set; }

    public long ToDate { get; set; }
}

public class DateTimeRequestValidator : AbstractValidator<DateTimeRequest>
{
    public DateTimeRequestValidator()
    {
        RuleFor(x => x).NotEmpty().WithMessage("TimeRequest cannot be null");
        RuleFor(x => x.FromDate).NotEmpty().WithMessage("FromDate cannot be null");
        RuleFor(x => x.ToDate).NotEmpty().WithMessage("ToDate cannot be null");
        RuleFor(x => x).Must(x => x.FromDate <= x.ToDate).WithMessage("FromDate must be less than or equal to ToDate");
    }
}