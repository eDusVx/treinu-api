using Treinu.Domain.Enums;

namespace Treinu.Domain.Errors;

public static class DomainErrors
{
    public static class Usuario
    {
        public static readonly DomainError CpfInvalido = new("Usuario.CpfInvalido", "O CPF fornecido é matematicamente inválido.", ErrorType.Validation);
        public static readonly DomainError EmailInvalido = new("Usuario.EmailInvalido", "O formato do e-mail não é válido.", ErrorType.Validation);
        public static readonly DomainError SenhaFraca = new("Usuario.SenhaFraca", "A senha deve conter mínimo 8 caracteres, maiúsculas, números e símbolos.", ErrorType.Validation);
        public static readonly DomainError DataNascimentoInvalida = new("Usuario.DataNascimentoInvalida", "A data de nascimento não é válida.", ErrorType.Validation);
        public static readonly DomainError GeneroInvalido = new("Usuario.GeneroInvalido", "Gênero inválido.", ErrorType.Validation);
        public static readonly DomainError ObjetivoInvalido = new("Usuario.ObjetivoInvalido", "Objetivo é obrigatório para perfis de Aluno.", ErrorType.Validation);
        public static readonly DomainError DadosVazios = new("Usuario.DadosVazios", "Existem dados obrigatórios ou arrays que não foram prenchidos.", ErrorType.Validation);
        public static readonly DomainError ConflitoEmEmailOuCpf = new("Usuario.ConflitoDuplicado", "O e-mail ou o CPF inserido já estão em uso.", ErrorType.Conflict);
    }
    
    public static class Credencial
    {
        public static readonly DomainError NaoEncontrada = new("Credencial.NaoEncontrada", "As credenciais solicitadas não foram encontradas.", ErrorType.NotFound);
        public static readonly DomainError SenhaIncorreta = new("Credencial.SenhaIncorreta", "O e-mail ou a senha estão incorretos.", ErrorType.Validation);
        public static readonly DomainError TokenExpirado = new("Credencial.TokenExpirado", "O Refresh Token providenciado expirou ou foi invalidado.", ErrorType.Validation);
    }
    
    public static class Geral
    {
        public static readonly DomainError Desconhecido = new("ErroGeral.FalhaProcessamento", "Ocorreu um erro desconhecido ao processar essa operação.", ErrorType.Failure);
    }
}
