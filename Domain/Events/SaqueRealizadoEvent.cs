using Domain.Interfaces;

namespace Domain.Eventos
{
    public record SaqueRealizadoEvent(
        Guid ContaId,
        decimal Valor,
        DateTime OccurredOn
        ) : IDomainEvent
    {
        public Guid EventId { get; } = Guid.NewGuid();
    }
}
