using Treinu.Domain.Entities;

namespace Treinu.Domain.Repositories;

public interface ITemplateEmailRepository
{
    Task<TemplateEmail?> ObterPorNomeAsync(string nome);
    Task AdicionarAsync(TemplateEmail template);
    Task AtualizarAsync(TemplateEmail template);
}
