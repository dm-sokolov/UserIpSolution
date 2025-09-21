using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace UserIpService.Application.Commands.ProcessConnection
{
    public class ProcessConnectionCommandValidator : AbstractValidator<ProcessConnectionCommand>
    {
        public ProcessConnectionCommandValidator()
        {
            RuleFor(x => x.UserId)
                .GreaterThan(0).WithMessage("UserId должен быть больше 0.");

            RuleFor(x => x.IpText)
                .Must(BeValidIp).WithMessage("Некорректный IPv4 или IPv6 адрес.");
        }

        private bool BeValidIp(string ip)
        {            
            return IPAddress.TryParse(ip, out _);
        }
    }
}
