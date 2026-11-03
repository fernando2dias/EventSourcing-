using Domain.Eventos;
using Domain.Exceptions;
using Domain.Interfaces;

namespace Domain.Aggregates
{
    public class ContaBancaria
    {
        public Guid Id { get; private set; }
        public string Titular { get; private set; } = string.Empty;
        public string NumeroConta { get; private set; } = string.Empty;
        public decimal Saldo { get; private set; }
        public int Versao { get; private set; }

        // Eventos gerados no ciclo de vida atual que ainda não foram salvos no Event Store
        private readonly List<IDomainEvent> _uncommittedEvents = new();
        public IReadOnlyCollection<IDomainEvent> UncommittedEvents => _uncommittedEvents.AsReadOnly();


        // Construtor privado para forçar criação via Factory Method ou Reconstrução
        private ContaBancaria() { }

        // Factory Method para abertura de conta
        public static ContaBancaria AbrirConta(string titular, string numeroConta) 
        { 
            if(string.IsNullOrWhiteSpace(titular))
                throw new ArgumentException("Titular não pode ser vazio.", nameof(titular));
            
            if(string.IsNullOrWhiteSpace(numeroConta))
                throw new ArgumentException("Número da conta não pode ser vazio.", nameof(numeroConta));

            var conta = new ContaBancaria();
            var @event = new ContaCriadaEvent(Guid.NewGuid(), titular, numeroConta, DateTime.UtcNow);
            conta.ApplyChange(@event, isNew: true);
            return conta;
        }

        // Operação de Depósito
        public void Depositar(decimal valor)
        {
            if (valor <= 0)
                throw new ArgumentException("O valor do depósito deve ser maior que zero.", nameof(valor));
            
            var @event = new DepositoRealizadoEvent(Id, valor, DateTime.UtcNow);
            ApplyChange(@event, isNew: true);
        }

        // Operação de Saque com validação de saldo
        public void Sacar(decimal valor)
        {
            if (valor <= 0)
                throw new ArgumentException("O valor do saque deve ser maior que zero.", nameof(valor));

            if (Saldo < valor)
                throw new SaldoInsuficienteException(Saldo, valor);

            var @event = new SaqueRealizadoEvent(Id, valor, DateTime.UtcNow);
            ApplyChange(@event, isNew: true);
        }

        // Reconstrói a ContaBancaria reexecutando todos os eventos salvos no banco (Event Replay)
        public static ContaBancaria ReconstruirDoHistorico(IEnumerable<IDomainEvent> historico)
        {
            var conta = new ContaBancaria();
            foreach (var @event in historico)
            {
                conta.ApplyChange(@event, isNew: false);
            }
            return conta;
        }

        public void LimparEventosNaoSalvos() => _uncommittedEvents.Clear();

        private void ApplyChange(IDomainEvent @event, bool isNew) 
        { 
            MutarEstado(@event); Versao++;
            if (isNew)
            {
                _uncommittedEvents.Add(@event);
            }
        }

        // Aplica as mutações no estado a partir de cada tipo de evento
        private void MutarEstado(IDomainEvent @event)
        {
            switch (@event)
            {
                case ContaCriadaEvent e:
                    Id = e.ContaId;
                    Titular = e.Titular;
                    NumeroConta = e.NumeroConta;
                    Saldo = 0m;
                    break;
                case DepositoRealizadoEvent e:
                    Saldo += e.Valor;
                    break;
                case SaqueRealizadoEvent e:
                    Saldo -= e.Valor;
                    break;
                default:
                    throw new InvalidOperationException($"Evento desconhecido: {@event.GetType().Name}");
            }
        }

    }
}
