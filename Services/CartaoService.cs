using ModuloPagamentos.Models;
using ModuloPagamentos.Repositories;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ModuloPagamentos.Services
{
    public class CartaoService
    {
        private readonly ICartaoRepository _repository;

        public CartaoService(ICartaoRepository repository) => _repository = repository;

        public List<ValidationError> RegistrarCartao(Cartao cartao)
        {
            var errors = new List<ValidationError>();

            if (string.IsNullOrWhiteSpace(cartao.Numero) || !Regex.IsMatch(cartao.Numero, "^\\d{13,19}$"))
                errors.Add(new ValidationError("Numero", "Número do cartão inválido."));

            if (!ValidarValidade(cartao.Validade))
                errors.Add(new ValidationError("Validade", "Validade do cartão inválida ou expirada."));

            if (string.IsNullOrWhiteSpace(cartao.CVV) || !Regex.IsMatch(cartao.CVV, "^\\d{3,4}$"))
                errors.Add(new ValidationError("CVV", "CVV inválido. Deve conter 3 ou 4 dígitos."));

            if (string.IsNullOrWhiteSpace(cartao.NomeTitular) || cartao.NomeTitular.Trim().Length < 2)
                errors.Add(new ValidationError("NomeTitular", "Nome do titular inválido. Informe pelo menos 2 caracteres."));

            if (!ValidarCPF(cartao.CPF))
                errors.Add(new ValidationError("CPF", "CPF inválido."));

            if (errors.Count == 0)
                _repository.Add(cartao);

            return errors;
        }

        private static bool ValidarCPF(string cpf)
        {
            cpf = Regex.Replace(cpf ?? "", "[^\\d]", "");
            
            // Verifica se possui 11 dígitos e não é uma sequência repetida
            if (cpf.Length != 11 || new string(cpf[0], cpf.Length) == cpf)
                return false;
            
            int[] mult1 = { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] mult2 = { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            string tempCpf = cpf.Substring(0, 9);
            int sum = 0;

            // Calcula o primeiro dígito verificador
            for (int i = 0; i < 9; i++)
                sum += int.Parse(tempCpf[i].ToString()) * mult1[i];

            int resto = sum % 11;
            if (resto < 2) resto = 0; 
            else resto = 11 - resto;
            
            string digito = resto.ToString();
            tempCpf += digito;
            sum = 0;

            // Calcula o segundo dígito verificador
            for (int i = 0; i < 10; i++)
                sum += int.Parse(tempCpf[i].ToString()) * mult2[i];

            resto = sum % 11;
            if (resto < 2) resto = 0; 
            else resto = 11 - resto;
            
            digito += resto.ToString();

            // Verifica se os dígitos calculados são iguais aos informados
            return cpf.EndsWith(digito);
        }

        private static bool ValidarValidade(string validade)
        {
            if (string.IsNullOrWhiteSpace(validade)) return false;

            var match = Regex.Match(validade, @"^(0[1-9]|1[0-2])/(\d{2,4})$");
            if (!match.Success) return false;
            
            int mes = int.Parse(match.Groups[1].Value);
            int ano = int.Parse(match.Groups[2].Value);
            if (ano < 100) ano += 2000;
            
            var dt = new DateTime(ano, mes, DateTime.DaysInMonth(ano, mes));
            return dt >= DateTime.Today;
        }
    }
}
