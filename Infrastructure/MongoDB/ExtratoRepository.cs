using Application.Interfaces;
using Application.Models;
using MongoDB.Driver;

namespace Infrastructure.MongoDB
{
    public class ExtratoRepository : IExtratoRepository
    {
        private readonly IMongoCollection<ExtratoReadModel> _collection;

        public ExtratoRepository(IMongoDatabase database)
        {
            _collection = database.GetCollection<ExtratoReadModel>("extratos");
        }

        public async Task<ExtratoReadModel?> ObterPorContaIdAsync(Guid contaId)
        {
            return await _collection.Find(x => x.ContaId == contaId).FirstOrDefaultAsync();
        }

        public async Task UpsertAsync(ExtratoReadModel extrato)
        {
            await _collection.ReplaceOneAsync(
                filter: x => x.ContaId == extrato.ContaId,
                replacement: extrato,
                options: new ReplaceOptions { IsUpsert = true });
        }
    }
}
