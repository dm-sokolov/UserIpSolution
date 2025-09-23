using FluentValidation;

namespace UserIpService.Application.Queries.GetUserLastConnection
{
    /// <summary>
    /// Валидатор для запроса получения последнего подключения пользователя.
    /// </summary>
    public class GetUserLastConnectionByUserIdQueryValidator : AbstractValidator<GetUserLastConnectionByUserIdQuery>
    {
        public GetUserLastConnectionByUserIdQueryValidator()
        {
            RuleFor(x => x.UserId)
                .GreaterThan(0)
                .WithMessage("Идентификатор пользователя должен быть больше 0.");
        }
    }
}
