
using System.ComponentModel.DataAnnotations;

namespace csharp.dtos;
public class Pedido
{
    [Required(ErrorMessage = "O Id é obrigatório.")]
    [Range(1, long.MaxValue, ErrorMessage = "O Id deve ser maior que zero.")]
    public long? Id { get; set; }
    [Required(ErrorMessage = "O valor é obrigatório.")]
    [Range(0.01, (double)decimal.MaxValue, ErrorMessage = "O valor deve ser maior que 0 e menor que 10 milhões.")]
    public decimal? Valor { get; set; }
}