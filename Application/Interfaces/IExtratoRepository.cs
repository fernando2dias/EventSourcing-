using Application.Models;

namespace Application.Interfaces
{
    public interface IExtratoRepository
    {
        Task<ExtratoReadModel> ObterPorContaIdAsync(Guid contaId);
        Task UpsertAsync(ExtratoReadModel extrato);
    }
}
