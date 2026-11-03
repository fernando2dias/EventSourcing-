using Domain.Interfaces;

namespace Domain.Eventos
{
    public record ContaCriadaEvent(
        Guid ContaId,
        string Titular,
        string NumeroConta,
        DateTime OccurredOn
    ) : IDomainEvent
    {
        public Guid EventId { get; } = Guid.NewGuid();
    }
}
