namespace Domain.Exceptions
{
    public class SaldoInsuficienteException : Exception
    {
        public SaldoInsuficienteException(decimal saldoAtual, decimal valorSaque) 
            : base($"Saldo insuficiente. Saldo atual: R$ {saldoAtual}, Valor do saque: R$ {valorSaque}")
        {
        }
    }
}
