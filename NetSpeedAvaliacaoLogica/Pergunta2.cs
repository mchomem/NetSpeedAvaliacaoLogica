namespace NetSpeedAvaliacaoLogica
{
    public class Pergunta2
    {
        private List<Aluno> alunos = new List<Aluno>()
        {
            new Aluno(1, new List<int>() {1, 2, 3 }),
            new Aluno(2, new List<int>() {6, 7, 8 }),
            new Aluno(3, new List<int>() {10, 7, 5 }),
            new Aluno(4, new List<int>() {7, 6, 8 }),
            new Aluno(5, new List<int>() {10, 1, 2 }),
        };

        public void ApresentarAlunos()
        {
            foreach (var aluno in alunos)
            {
                var mediaPonderada = CalcularMediaPonderada(aluno);

                Console.Clear();
                Console.WriteLine($"Aluno código {aluno.Codigo}");
                Console.WriteLine($"Notas: {string.Join(", ", aluno.Notas)}");
                Console.WriteLine($"Média ponderada: {mediaPonderada}");
                
                Console.WriteLine("Resultado final: {0}", mediaPonderada >= 6 ? "APROVADO" : "REPROVADO");
                Console.WriteLine();

                Console.Write("\nInforme zero para para a exibição! ");
                var opcao = Console.ReadLine();
                Console.WriteLine();

                if (opcao.ToLower() == "0")
                    break;
            }
        }

        public double CalcularMediaPonderada(Aluno aluno)
        {
            double resultado = 0.0;

            var notasAluno = new List<int>();
            var maiorNota = aluno.Notas.Max();
            var pesos = new List<int>();

            for (int i = 0; i < aluno.Notas.Count; i++)
            {
                notasAluno.Add(aluno.Notas[i]);

                if (notasAluno[i] == maiorNota)
                {
                    notasAluno[i] *= 4;
                    pesos.Add(4);
                }
                else
                {
                    notasAluno[i] *= 3;
                    pesos.Add(3);
                }
            }

            var somatorioNotasPesos = notasAluno.Sum();
            var somatorioPesos  = pesos.Sum();

            return resultado = somatorioNotasPesos / somatorioPesos;
        }
    }

    public class Aluno
    {
        public Aluno(int codigo, List<int> notas)
        {
            Codigo = codigo;
            Notas = notas;
        }

        public int Codigo { get; set; }
        public List<int> Notas { get; set; }
    }
}
