using Microsoft.AspNetCore.Identity;

namespace BookingBuddy.Server.Services
{
    /// <summary>
    /// Classe que representa a descrição de erros (Identity) em português.
    /// </summary>
    public class PortugueseIdentityErrorDescriber : IdentityErrorDescriber
    {
        /// <summary>
        /// Erro desconhecido.
        /// </summary>
        /// <returns>Retorna um erro desconhecido.</returns>
        public override IdentityError DefaultError() { return new IdentityError { Code = nameof(DefaultError), Description = $"Um erro desconhecido ocorreu." }; }

        /// <summary>
        /// Erro de concorrência.
        /// </summary>
        /// <returns>Rertorna um erro de concorrência.</returns>
        public override IdentityError ConcurrencyFailure() { return new IdentityError { Code = nameof(ConcurrencyFailure), Description = "Falha de concorrência otimista, o objeto foi modificado." }; }

        /// <summary>
        /// Erro de palavra-passe errada.
        /// </summary>
        /// <returns>Retorna um erro de palavra-passe errada.</returns>
        public override IdentityError PasswordMismatch() { return new IdentityError { Code = nameof(PasswordMismatch), Description = "Palavra-passe incorreta." }; }

        /// <summary>
        /// Erro de token inválido.
        /// </summary>
        /// <returns>Retorna um erro de token inválido.</returns>
        public override IdentityError InvalidToken() { return new IdentityError { Code = nameof(InvalidToken), Description = "Token inválido." }; }

        /// <summary>
        /// Erro de email já associado.
        /// </summary>
        /// <returns>Retorna um erro de email já associado.</returns>
        public override IdentityError LoginAlreadyAssociated() { return new IdentityError { Code = nameof(LoginAlreadyAssociated), Description = "Já existe um utilizador com este login." }; }
        
        /// <summary>
        /// Erro de nome de utilizador inválido.
        /// </summary>
        /// <param name="userName">Nome de utilizador inválido.</param>
        /// <returns>Retorna um erro de nome de utilizador inválido.</returns>
        public override IdentityError InvalidUserName(string? userName) { return new IdentityError { Code = nameof(InvalidUserName), Description = $"Username '{userName}' é inválido, pode conter apenas letras ou dígitos." }; }
        
        /// <summary>
        /// Erro de email inválido.
        /// </summary>
        /// <param name="email">Email inválido.</param>
        /// <returns>Retorna um erro de email inválido.</returns>
        public override IdentityError InvalidEmail(string? email) { return new IdentityError { Code = nameof(InvalidEmail), Description = $"Email '{email}' é inválido." }; }
        
        /// <summary>
        /// Erro de nome de utilizador já em uso.
        /// </summary>
        /// <param name="userName">Nome de utilizador já em uso.</param>
        /// <returns>Retorna um erro de nome de utilizador já em uso.</returns>
        public override IdentityError DuplicateUserName(string userName) { return new IdentityError { Code = nameof(DuplicateUserName), Description = $"Username '{userName}' já está em uso." }; }
        
        /// <summary>
        /// Erro de email já em uso.
        /// </summary>
        /// <param name="email">Email já em uso.</param>
        /// <returns>Retorna um erro de email já em uso.</returns>
        public override IdentityError DuplicateEmail(string email) { return new IdentityError { Code = nameof(DuplicateEmail), Description = $"Email '{email}' já está em uso." }; }
        
        /// <summary>
        /// Erro de permissão inválida.
        /// </summary>
        /// <param name="role">Permissão inválida.</param>
        /// <returns>Retorna um erro de permissão inválida.</returns>
        public override IdentityError InvalidRoleName(string? role) { return new IdentityError { Code = nameof(InvalidRoleName), Description = $"A permissão '{role}' é inválida." }; }
        
        /// <summary>
        /// Erro de permissão já em uso.
        /// </summary>
        /// <param name="role">Permissão já em uso.</param>
        /// <returns>Retorna um erro de permissão já em uso.</returns>
        public override IdentityError DuplicateRoleName(string role) { return new IdentityError { Code = nameof(DuplicateRoleName), Description = $"A permissão '{role}' já está em uso." }; }
        
        /// <summary>
        /// Erro de utilizador já possui palavra-passe.
        /// </summary>
        /// <returns>Retorna um erro de utilizador já possui palavra-passe.</returns>
        public override IdentityError UserAlreadyHasPassword() { return new IdentityError { Code = nameof(UserAlreadyHasPassword), Description = "Utilizador já possui uma palavra-passe definida." }; }
        
        /// <summary>
        /// Erro de bloqueio de conta não se encontra ativado.
        /// </summary>
        /// <returns>Rertorna um erro de bloqueio de conta não se encontra ativado.</returns>
        public override IdentityError UserLockoutNotEnabled() { return new IdentityError { Code = nameof(UserLockoutNotEnabled), Description = "Lockout não está habilitado para este utilizador." }; }
        
        /// <summary>
        /// Erro de utilizador já se encontra com cargo/permissão.
        /// </summary>
        /// <param name="role">Cargo/Permissão já associada.</param>
        /// <returns>Retorna um erro de utilizador já se encontra com cargo/permissão.</returns>
        public override IdentityError UserAlreadyInRole(string role) { return new IdentityError { Code = nameof(UserAlreadyInRole), Description = $"Utilizador já possui a permissão '{role}'." }; }
       
        /// <summary>
        /// Erro de utilizador não se encontra com cargo/permissão.
        /// </summary>
        /// <param name="role">Cargo/Permissão não associada.</param>
        /// <returns>Retorna um erro de utilizador não se encontra com cargo/permissão.</returns>
        public override IdentityError UserNotInRole(string role) { return new IdentityError { Code = nameof(UserNotInRole), Description = $"Utilizador não tem a permissão '{role}'." }; }
        
        /// <summary>
        /// Erro de palavra-passe demasiado curta.
        /// </summary>
        /// <param name="length">Comprimento mínimo da palavra-passe.</param>
        /// <returns>Retorna um erro de palavra-passe demasiado curta.</returns>
        public override IdentityError PasswordTooShort(int length) { return new IdentityError { Code = nameof(PasswordTooShort), Description = $"Palavras-passe devem conter pelo menos {length} caracteres." }; }
        
        /// <summary>
        /// Erro de palavra-passe introduzida não contém caracteres não alfanuméricos.
        /// </summary>
        /// <returns>Retorna um erro de palavra-passe não contém caracteres não alfanuméricos.</returns>
        public override IdentityError PasswordRequiresNonAlphanumeric() { return new IdentityError { Code = nameof(PasswordRequiresNonAlphanumeric), Description = "Palavras-passe devem conter pelo menos um caracter não alfanumérico." }; }
        
        /// <summary>
        /// Erro de palavra-passe introduzida não contém digito.
        /// </summary>
        /// <returns>Retorna um erro de palavra-passe não contém digito.</returns>
        public override IdentityError PasswordRequiresDigit() { return new IdentityError { Code = nameof(PasswordRequiresDigit), Description = "Palavras-passe devem conter pelo menos um digito ('0'-'9')." }; }
        
        /// <summary>
        /// Erro de palavra-passe introduzida não contém caracter em minúsculo.
        /// </summary>
        /// <returns>Retorna um erro de palavra-passe não contém caracter em minúsculo.</returns>
        public override IdentityError PasswordRequiresLower() { return new IdentityError { Code = nameof(PasswordRequiresLower), Description = "Palavras-passe devem conter pelo menos um caracter em minúsculo ('a'-'z')." }; }
        
        /// <summary>
        /// Erro de palavra-passe introduzida não contém caracter em maiúsculo.
        /// </summary>
        /// <returns>Retorna um erro de palavra-passe não contém caracter em maiúsculo.</returns>
        public override IdentityError PasswordRequiresUpper() { return new IdentityError { Code = nameof(PasswordRequiresUpper), Description = "Palavras-passe devem conter pelo menos um caracter em maiúsculo ('A'-'Z')." }; }
    }
}
