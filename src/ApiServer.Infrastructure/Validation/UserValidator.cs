using FluentValidation;

namespace ApiServer.Infrastructure.Validation
{
    public class UserValidator : AbstractValidator<SysUser>
    {
        public UserValidator()
        {
            CascadeMode = CascadeMode.Stop;
            RuleFor(x => x.username).NotEmpty().WithMessage("用户名不能为空！");
            RuleFor(x => x.password).NotEmpty().WithMessage("密码不能为空！");
        }
    }
}
