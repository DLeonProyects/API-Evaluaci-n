using APIEvaluacion.DTOs;
using FluentValidation;



namespace APIEvaluacion.Validators
{
    /// <Comentarios>
    /// Validador para la creación de usuarios basado en las reglas de negocio definidas.
    /// </Comentarios>
    public class RegisterUserValidator : AbstractValidator<RegisterUserDto>
    {
        public RegisterUserValidator()
        {
            // Validación para el campo 'Nombre': No puede estar vacío
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("El campo 'Nombre' no debe estar vacío.");

            // Validación para el campo 'Email': Obligatorio y con formato de correo electrónico válido
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("El correo es obligatorio.")
                .EmailAddress().WithMessage("El correo electrónico no tiene un formato válido.");

            // Validación para el campo 'Password': 
            // Se asegura de que cumpla con la política de seguridad mínima
            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("La contraseña es obligatoria.")
                .MinimumLength(8).WithMessage("La contraseña debe tener al menos 8 caracteres.")
                .Matches(@"[A-Z]").WithMessage("La contraseña debe incluir al menos una letra mayúscula.")
                .Matches(@"[a-z]").WithMessage("La contraseña debe incluir al menos una letra minúscula.")
                .Matches(@"\d").WithMessage("La contraseña debe incluir al menos un número.")
                .Matches(@"[\W_]").WithMessage("La contraseña debe incluir al menos un símbolo.");
        }
    }
}
