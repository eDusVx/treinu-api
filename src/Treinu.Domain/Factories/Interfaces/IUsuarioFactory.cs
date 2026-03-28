using FluentResults;
using Treinu.Domain.Entities;

namespace Treinu.Domain.Factories.Interfaces;

public interface IUsuarioFactory
{
    Result<Usuario> Fabricar(FabricarUsuarioProps props);
}