using Application.DTOs;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Application.Models
{
    public class ExtratoReadModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public Guid ContaId { get; set; }

        public string Titular { get; set; } = string.Empty;
        public string NumeroConta { get; set; } = string.Empty;
        public decimal Saldo { get; set; }
        public int UltimaVersaoProcessada { get; set; }
        public List<MovimentacaoDto> Movimentacoes { get; set; } = new();
    }
}
