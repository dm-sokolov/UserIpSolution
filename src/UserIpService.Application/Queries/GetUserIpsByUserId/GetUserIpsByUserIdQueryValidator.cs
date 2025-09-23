using FluentValidation;

namespace UserIpService.Application.Queries.GetUserIps
{
    /// <summary>
    /// Валидатор для запроса получения списка IP-адресов пользователя.
    /// </summary>
    public class GetUserIpsByUserIdQueryValidator : AbstractValidator<GetUserIpsByUserIdQuery>
    {
        public GetUserIpsByUserIdQueryValidator()
        {
            RuleFor(x => x.UserId)
                .GreaterThan(0)
                .WithMessage("Идентификатор пользователя должен быть больше 0.");
        }
    }
}
