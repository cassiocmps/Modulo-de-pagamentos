using ModuloPagamentos.Models;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace ModuloPagamentos.Repositories
{
    public interface ICartaoRepository
    {
        void Add(Cartao cartao);
        IEnumerable<Cartao> GetAll();
    }

    public class CartaoRepository : ICartaoRepository
    {
        private readonly string _filePath;
        private readonly object _lock = new();
        private const string Header = "Numero,Validade,CVV,NomeTitular,CPF";

        public CartaoRepository(string filePath)
        {
            _filePath = filePath;
            EnsureFileExists();
        }

        private void EnsureFileExists()
        {
            if (!File.Exists(_filePath))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(_filePath));
                File.WriteAllText(_filePath, Header + "\n");
            }
        }

        public void Add(Cartao cartao)
        {
            lock (_lock)
            {
                var line = $"{cartao.Numero},{cartao.Validade},{cartao.CVV},\"{cartao.NomeTitular}\",{cartao.CPF}";
                File.AppendAllText(_filePath, line + "\n");
            }
        }

        public IEnumerable<Cartao> GetAll()
        {
            EnsureFileExists();
            var lines = File.ReadAllLines(_filePath).Skip(1);
            foreach (var line in lines)
            {
                var parts = line.Split(',');
                if (parts.Length == 5)
                {
                    yield return new Cartao
                    {
                        Numero = parts[0],
                        Validade = parts[1],
                        CVV = parts[2],
                        NomeTitular = parts[3].Trim('"'),
                        CPF = parts[4]
                    };
                }
            }
        }
    }
}
