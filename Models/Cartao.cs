using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace ModuloPagamentos.Models;

public class Cartao
{
    public string Numero { get; set; }
    public string Validade { get; set; }
    public string CVV { get; set; }
    public string NomeTitular { get; set; }
    public string CPF { get; set; }
}
