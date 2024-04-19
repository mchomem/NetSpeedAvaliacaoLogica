namespace NetSpeedAvaliacaoLogica
{
    public class Pergunta1
    {
        List<Carro> carros = new List<Carro>()
        {
            new Carro(1990, 10500.0m),
            new Carro(1999, 9600.0m),
            new Carro(2000, 11700.0m),
            new Carro(2010, 15000.0m),
            new Carro(2021, 23800.0m),
        };

        public void ExibirCatalogoCarros()
        {
            foreach (Carro carro in carros)
            {
                CalcularValores(carro);

                Console.Write("\nDeseja ver mais do catálogo? [s/n]: ");
                var opcao = Console.ReadLine();
                Console.WriteLine();

                if (opcao.ToLower() == "n")
                    break;
            }

            var carrosAno2000 = carros.Where(c => c.Ano <= 2000).ToList().Count();
            var carrosGeral = carros.ToList().Count();

            Console.WriteLine($"Total de carros do ano 2000: {carrosAno2000}");
            Console.WriteLine($"Total de carros geral: {carrosGeral}");
        }

        public void CalcularValores(Carro carro)
        {
            decimal desconto = 0.0m;
            decimal valorASerPago = 0.0m;

            if(carro.Ano <= 2000)
                desconto = carro.Valor * 0.07m;
            else
                desconto = carro.Valor * 0.12m;

            Console.WriteLine($"O carro do ano {carro.Ano} no valor R$ {carro.Valor} com desconto de R$ {desconto}. Preço final R$ {carro.Valor - desconto}");
        }
    }

    public class Carro
    {
        public Carro(int ano, decimal valor)
        {
            Ano = ano;
            Valor = valor;
        }

        public int Ano { get; set; }
        public decimal Valor { get; set; }
    }
}
