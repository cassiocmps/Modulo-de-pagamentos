using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ModuloPagamentos.Services;
using ModuloPagamentos.Models;
using System.Collections.Generic;
using CartaoModel = ModuloPagamentos.Models.Cartao;

namespace ModuloPagamentos.Pages.Cartao
{
    public class CreateModel : PageModel
    {
        private readonly CartaoService _service;
        public CreateModel(CartaoService service)
        {
            _service = service;
        }

        [BindProperty]
        public CartaoModel Cartao { get; set; }
        public bool Sucesso { get; set; }
        public List<string> Erros { get; set; } = new();

        public void OnGet()
        {
            Sucesso = false;
            Erros = new List<string>();
        }

        public IActionResult OnPost()
        {
            Erros = new List<string>();
            Sucesso = false;

            var validationErrors = _service.RegistrarCartao(Cartao);
            if (validationErrors.Count == 0)
            {
                Sucesso = true;
                Cartao = new CartaoModel();
            }
            else
            {
                foreach (var error in validationErrors)
                {
                    Erros.Add($"{error.Message}");
                }
            }

            return Page();
        }
    }
}
