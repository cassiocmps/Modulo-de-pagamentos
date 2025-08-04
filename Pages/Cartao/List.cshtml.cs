using Microsoft.AspNetCore.Mvc.RazorPages;
using ModuloPagamentos.Repositories;
using System.Collections.Generic;
using CartaoModel = ModuloPagamentos.Models.Cartao;

namespace ModuloPagamentos.Pages.Cartao
{
    public class ListModel : PageModel
    {
        private readonly ICartaoRepository _repository;
        public List<CartaoModel> Cartoes { get; set; }

        public ListModel(ICartaoRepository repository)
        {
            _repository = repository;
        }

        public void OnGet()
        {
            Cartoes = new List<CartaoModel>(_repository.GetAll());
        }
    }
}
