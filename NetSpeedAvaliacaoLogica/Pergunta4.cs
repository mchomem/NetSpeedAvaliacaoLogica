namespace NetSpeedAvaliacaoLogica
{
    public class Pergunta4
    {
        public void ProcessarBoletos()
        {
            List<Boleto> boletos = ObterBoletos();
            List<Boleto> recalcuados = new List<Boleto>();

            foreach (var boleto in boletos)
                recalcuados.Add(VerificarSituacao(boleto));

            foreach (var boleto in recalcuados)
            {
                Console.WriteLine("Boleto recalculado");
                Console.WriteLine($"Observação: {boleto.Observacao}");
                Console.WriteLine($"Data original: {boleto.VencimentoOriginal.ToString("dd/MM/yyyy")}");
                Console.WriteLine($"Nova data: {boleto.DataPagamento.ToString("dd/MM/yyyy")}");
                Console.WriteLine($"Valor: {boleto.Valor}");
                Console.WriteLine($"Juros: {boleto.Juros}");
                Console.WriteLine($"Multa: {boleto.Multa}");
                Console.WriteLine($"Valor recalculado: {boleto.ValorRecalculado}");

                Console.WriteLine();
            }
        }

        private Boleto VerificarSituacao(Boleto boleto)
        {
            // regra 7
            if (boleto.DataPagamento < boleto.VencimentoOriginal)
                return boleto;

            // regra 3
            else if (VerificarFeriado(boleto.VencimentoOriginal) && boleto.DataPagamento == boleto.VencimentoOriginal.AddDays(1) && !VerificarFinalSemana(boleto.DataPagamento))
                return boleto;

            // regra 5
            else if (VerificarFeriado(boleto.VencimentoOriginal) && boleto.DataPagamento == ObterProximoDiaUtilConsecutivo(boleto.VencimentoOriginal).AddDays(1) && !VerificarFinalSemana(boleto.DataPagamento))
                return CalcularEncargos(boleto);

            // Regra 4
            else if (VerificarFeriado(boleto.VencimentoOriginal) && VerificarFinalSemana(boleto.VencimentoOriginal.AddDays(1)))
                return boleto;

            // Regra 6
            else if (!VerificarFeriado(boleto.VencimentoOriginal) && boleto.VencimentoOriginal == boleto.DataPagamento)
                return boleto;

            // regra 2
            else if ((VerificarFinalSemana(boleto.VencimentoOriginal) || VerificarFeriado(boleto.VencimentoOriginal)) && boleto.DataPagamento > ObterProximoDiaUtilConsecutivo(boleto.VencimentoOriginal))
                return CalcularEncargos(boleto);

            // regra 1
            else if ((VerificarFinalSemana(boleto.VencimentoOriginal) || VerificarFeriado(boleto.VencimentoOriginal)) && boleto.DataPagamento == ObterProximoDiaUtilConsecutivo(boleto.VencimentoOriginal))
                return boleto;

            else if (!VerificarFinalSemana(boleto.VencimentoOriginal) && boleto.DataPagamento == boleto.VencimentoOriginal.AddDays(1) && !VerificarFinalSemana(boleto.DataPagamento))
                return CalcularEncargos(boleto);

            return boleto;
        }

        private Boleto CalcularEncargos(Boleto boleto)
        {
            int totalDias = ObterDiferencaDias(boleto.VencimentoOriginal, boleto.DataPagamento);

            decimal jurosAoDia = 0.03m;
            decimal totalJurosDia = jurosAoDia * totalDias;
            boleto.Juros = totalJurosDia;
            boleto.Multa = 2.00m;

            boleto.ValorRecalculado = boleto.Valor + boleto.Juros + boleto.Multa;

            return boleto;
        }
        
        private DateTime ObterProximoDiaUtilConsecutivo(DateTime data)
        {
            if(data.DayOfWeek == DayOfWeek.Friday)
                data = data.AddDays(1);

            while (VerificarFinalSemana(data))
                data = data.AddDays(1);

            return data;
        }

        private bool VerificarFinalSemana(DateTime data)
            => data.DayOfWeek == DayOfWeek.Saturday || data.DayOfWeek == DayOfWeek.Sunday;

        private int ObterDiferencaDias(DateTime dataVencimentoOriginal, DateTime novaDataVencimento)
        {
            TimeSpan diferenca = novaDataVencimento.Subtract(dataVencimentoOriginal);
            return diferenca.Days;
        }

        private bool VerificarFeriado(DateTime data)
        {
            List<Feriado> feriados = new List<Feriado>()
            {
                  new Feriado("Ano Novo",                   new DateTime(2023, 01, 01)),
                  new Feriado("Carnaval (Segunda-feira)",   new DateTime(2023, 02, 20)),
                  new Feriado("Carnaval (Terça-feira)",     new DateTime(2023, 02, 21)),
                  new Feriado("Cinzas (até 14 horas)",      new DateTime(2023, 02, 22)),
                  new Feriado("Paixão de Cristo",           new DateTime(2023, 04, 07)),
                  new Feriado("Páscoa",                     new DateTime(2023, 04, 09)),
                  new Feriado("Tiradentes",                 new DateTime(2023, 04, 21)),
                  new Feriado("Dia do Trabalho",            new DateTime(2023, 05, 01)),
                  new Feriado("Corpo de Deus",              new DateTime(2023, 06, 08)),
                  new Feriado("Independência",              new DateTime(2023, 09, 07)),
                  new Feriado("Nossa Senhora de Aparecida", new DateTime(2023, 10, 12)),
                  new Feriado("Finados",                    new DateTime(2023, 11, 02)),
                  new Feriado("Proclamação da República",   new DateTime(2023, 11, 15)),
                  new Feriado("Natal",                      new DateTime(2023, 12, 25)),
            };

            return feriados
                    .Where(f => f.Data.Day == data.Day && f.Data.Month == data.Month && f.Data.Year == data.Year)
                    .Count() > 0;
        }

        private List<Boleto> ObterBoletos()
        {
            return new List<Boleto>()
            {
                new Boleto() { VencimentoOriginal = new DateTime(2023, 05, 06), DataPagamento = new DateTime(2023, 05, 08), Valor = 100.0m, Observacao = "Rregra 1" },
                new Boleto() { VencimentoOriginal = new DateTime(2023, 05, 07), DataPagamento = new DateTime(2023, 05, 09), Valor = 100.0m, Observacao = "Rregra 2" },
                new Boleto() { VencimentoOriginal = new DateTime(2023, 05, 01), DataPagamento = new DateTime(2023, 05, 02), Valor = 100.0m, Observacao = "Rregra 3" },
                new Boleto() { VencimentoOriginal = new DateTime(2023, 04, 21), DataPagamento = new DateTime(2023, 04, 24), Valor = 100.0m, Observacao = "Rregra 4" },
                new Boleto() { VencimentoOriginal = new DateTime(2023, 04, 07), DataPagamento = new DateTime(2023, 04, 11), Valor = 100.0m, Observacao = "Rregra 5" },
                new Boleto() { VencimentoOriginal = new DateTime(2023, 05, 10), DataPagamento = new DateTime(2023, 05, 10), Valor = 100.0m, Observacao = "Rregra 6" },
                new Boleto() { VencimentoOriginal = new DateTime(2023, 05, 11), DataPagamento = new DateTime(2023, 05, 10), Valor = 100.0m, Observacao = "Rregra 7" },
                new Boleto() { VencimentoOriginal = new DateTime(2023, 05, 08), DataPagamento = new DateTime(2023, 05, 09), Valor = 100.0m, Observacao = "Rregra 8" },
            };
        }
    }

    public class Boleto
    {
        public DateTime VencimentoOriginal { get; set; }
        public DateTime DataPagamento { get; set; }
        public decimal Valor { get; set; }
        public decimal ValorRecalculado { get; set; }
        public decimal Juros { get; set; }
        public decimal Multa { get; set; }
        public string Observacao { get; set; } = string.Empty;
    }

    public class Feriado
    {
        public Feriado(string nome, DateTime data)
        {
            Nome = nome;
            Data = data;
        }

        public string Nome { get; set; }
        public DateTime Data { get; set; }
    }
}
