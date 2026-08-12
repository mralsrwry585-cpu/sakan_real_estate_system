using FluentValidation;

namespace SAKAN.Application.Features.ViewingRequests.Commands.CreateViewingRequest
{
    public class CreateViewingRequestCommandValidator
        : AbstractValidator<CreateViewingRequestCommand>
    {
        public CreateViewingRequestCommandValidator()
        {
            // =========================================================
            // Property
            // =========================================================
            RuleFor(v => v.PropertyId)
                .NotEmpty()
                .WithMessage("ãÚÑøÝ ÇáÚÞÇÑ ãØáæÈ.");

            // =========================================================
            // Requested Date
            // =========================================================
            RuleFor(v => v.RequestedDate)
                .Must(date => date != default)
                .WithMessage("ÊÇÑíÎ ÇáãÚÇíäÉ ãØáæÈ.")
                .GreaterThanOrEqualTo(DateTime.Today)
                .WithMessage("ÊÇÑíÎ ÇáãÚÇíäÉ íÌÈ Ãä íßæä Çáíæã Ãæ ÊÇÑíÎðÇ ãÓÊÞÈáíðÇ.");

            // =========================================================
            // Requested Time
            // =========================================================
            RuleFor(v => v.RequestedTime)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithMessage("æÞÊ ÇáãÚÇíäÉ ãØáæÈ.")
                .MaximumLength(5)
                .WithMessage("æÞÊ ÇáãÚÇíäÉ íÌÈ Ãä íßæä ÈÕíÛÉ HH:mm.")
                .Must(BeValidTime)
                .WithMessage("æÞÊ ÇáãÚÇíäÉ ÛíÑ ÕÇáÍ. ÇÓÊÎÏã ÕíÛÉ HH:mm ãËá 10:30.");

            // =========================================================
            // Note
            // =========================================================
            RuleFor(v => v.Note)
                .MaximumLength(1000)
                .WithMessage("ÇáãáÇÍÙÉ íÌÈ ÃáÇ ÊÊÌÇæÒ 1000 ÍÑÝ.");

            // =========================================================
            // Note - trim / whitespace
            // =========================================================
            When(v => v.Note != null, () =>
            {
                RuleFor(v => v.Note!)
                    .Must(note => note.Trim().Length <= 1000)
                    .WithMessage("ÇáãáÇÍÙÉ íÌÈ ÃáÇ ÊÊÌÇæÒ 1000 ÍÑÝ.");
            });
        }

        private static bool BeValidTime(string? time)
        {
            if (string.IsNullOrWhiteSpace(time))
                return false;

            // íÌÈ Ãä Êßæä ÇáÕíÛÉ ÈÇáÖÈØ HH:mm
            if (!TimeSpan.TryParseExact(
                    time.Trim(),
                    @"hh\:mm",
                    System.Globalization.CultureInfo.InvariantCulture,
                    out var parsedTime))
            {
                return false;
            }

            // 00:00 Åáì 23:59
            return parsedTime >= TimeSpan.Zero
                   && parsedTime < TimeSpan.FromDays(1);
        }
    }
}