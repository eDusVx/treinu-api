using Treinu.Domain.Core;

namespace Treinu.Domain.Entities;

public class TemplateEmail : Entity
{
    public string Nome { get; private set; }
    public string AssuntoPadrao { get; private set; }
    public string ConteudoHtml { get; private set; }

    protected TemplateEmail() 
    { 
        Nome = null!;
        AssuntoPadrao = null!;
        ConteudoHtml = null!;
    }

    public TemplateEmail(Guid id, string nome, string assuntoPadrao, string conteudoHtml) : base(id)
    {
        Nome = nome;
        AssuntoPadrao = assuntoPadrao;
        ConteudoHtml = conteudoHtml;
    }

    public TemplateEmail(string nome, string assuntoPadrao, string conteudoHtml)
    {
        Id = Guid.NewGuid();
        Nome = nome;
        AssuntoPadrao = assuntoPadrao;
        ConteudoHtml = conteudoHtml;
    }
}
