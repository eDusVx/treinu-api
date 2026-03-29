using Treinu.Domain.Enums;

namespace Treinu.Domain.Errors;

public static class DomainErrors
{
    public static class Usuario
    {
        public static readonly DomainError CpfInvalido = new("Usuario.CpfInvalido",
            "O CPF fornecido é matematicamente inválido.", ErrorType.Validation);

        public static readonly DomainError EmailInvalido = new("Usuario.EmailInvalido",
            "O formato do e-mail não é válido.", ErrorType.Validation);

        public static readonly DomainError SenhaFraca = new("Usuario.SenhaFraca",
            "A senha deve conter mínimo 8 caracteres, maiúsculas, números e símbolos.", ErrorType.Validation);

        public static readonly DomainError DataNascimentoInvalida = new("Usuario.DataNascimentoInvalida",
            "A data de nascimento não é válida.", ErrorType.Validation);

        public static readonly DomainError GeneroInvalido =
            new("Usuario.GeneroInvalido", "Gênero inválido.", ErrorType.Validation);

        public static readonly DomainError ObjetivoInvalido = new("Usuario.ObjetivoInvalido",
            "Objetivo é obrigatório para perfis de Aluno.", ErrorType.Validation);

        public static readonly DomainError DadosVazios = new("Usuario.DadosVazios",
            "Existem dados obrigatórios ou arrays que não foram prenchidos.", ErrorType.Validation);

        public static readonly DomainError ConflitoEmEmailOuCpf = new("Usuario.ConflitoDuplicado",
            "O e-mail ou o CPF inserido já estão em uso.", ErrorType.Conflict);

        public static readonly DomainError UsuarioNaoEncontrado = new("Usuario.NaoEncontrado",
            "Usuário não encontrado.", ErrorType.NotFound);

        public static readonly DomainError TreinadorNaoEncontrado = new("Treinador.NaoEncontrado",
            "Treinador não encontrado.", ErrorType.NotFound);

        public static readonly DomainError AlunoNaoEncontrado = new("Aluno.NaoEncontrado",
            "Aluno não encontrado.", ErrorType.NotFound);

        public static readonly DomainError ContatoNaoEncontrado = new("Contato.NaoEncontrado",
            "Contato não encontrado para este usuário.", ErrorType.NotFound);

        public static readonly DomainError CertificadoNaoEncontrado = new("Certificado.NaoEncontrado",
            "Certificado não encontrado para este treinador.", ErrorType.NotFound);

        public static readonly DomainError EspecializacaoNaoEncontrada = new("Especializacao.NaoEncontrada",
            "Especialização não encontrada para este treinador.", ErrorType.NotFound);

        public static readonly DomainError AvaliacaoFisicaNaoEncontrada = new("AvaliacaoFisica.NaoEncontrada",
            "Avaliação física não encontrada para este aluno.", ErrorType.NotFound);
    }

    public static class Credencial
    {
        public static readonly DomainError NaoEncontrada = new("Credencial.NaoEncontrada",
            "As credenciais solicitadas não foram encontradas.", ErrorType.NotFound);

        public static readonly DomainError SenhaIncorreta = new("Credencial.SenhaIncorreta",
            "O e-mail ou a senha estão incorretos.", ErrorType.Validation);

        public static readonly DomainError TokenExpirado = new("Credencial.TokenExpirado",
            "O Refresh Token providenciado expirou ou foi invalidado.", ErrorType.Validation);

        public static readonly DomainError TokenRecuperacaoInvalido = new("Credencial.TokenRecuperacaoInvalido",
            "O token de recuperação é inválido ou expirou.", ErrorType.Validation);

        public static readonly DomainError CodigoLoginInvalido = new("Credencial.CodigoLoginInvalido",
            "O código de login fornecido é inválido ou já expirou.", ErrorType.Validation);
    }

    public static class Convite
    {
        public static readonly DomainError ConviteNaoEncontrado = new("Convite.NaoEncontrado",
            "Convite não encontrado.", ErrorType.NotFound);

        public static readonly DomainError ConviteJaUtilizado = new("Convite.JaUtilizado",
            "Este convite já foi utilizado ou não é mais válido.", ErrorType.Validation);

        public static readonly DomainError ConviteExpirado = new("Convite.Expirado",
            "Este convite expirou.", ErrorType.Validation);

        public static readonly DomainError PerfilInvalido = new("Convite.PerfilInvalido",
            "O perfil do convite não condiz com o cadastro solicitado.", ErrorType.Validation);
    }

    public static class Geral
    {
        public static readonly DomainError Desconhecido = new("ErroGeral.FalhaProcessamento",
            "Ocorreu um erro desconhecido ao processar essa operação.", ErrorType.Failure);
    }
}