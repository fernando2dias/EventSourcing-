namespace Application.DTOs
{
    public class MovimentacaoDto
    {
        public string Tipo { get; set; } = string.Empty; // "Abertura", "Depósito", "Saque"
        public decimal Valor { get; set; }
        public DateTime Data { get; set; }
    }
}
