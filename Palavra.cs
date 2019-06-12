using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

// Rafael Pak Bragagnolo - 18206

namespace ApForca
{
    class Palavra
    {
        string palavra;
        const int tamanhoPalavra = 15;
        string dica;
        const int tamanhoDica = 200;
        int[] posicoes;

        public void LerDados(StreamReader arq)   // Este método lê os dados do arquivo texto com as Palavras e Dicas, e atribui às variáveis dica e palavra
        {
            if (!arq.EndOfStream)
            {
                int indiceFimDaPalavra = -1;
                string linha = arq.ReadLine();
                palavra = linha.Substring(0, 15);

                if (palavra[14] == ' ')
                {
                    indiceFimDaPalavra = palavra.IndexOf("  ");
                    if (indiceFimDaPalavra > 0)
                        palavra = palavra.Substring(0, palavra.IndexOf("  "));
                    else
                        palavra = palavra.Substring(0, 14);
                }
       
                dica = linha.Substring(15);
            }
        }

        public string PalavraEscolhida   // Devolve a palavra
        {
            get
            {
                return palavra;
            }
        }

        public string DicaEscolhida   // Devolve a dica
        {
            get
            {
                return dica;
            }
        }
        
        public bool ExisteLetra(char letra)   // Verifica se a letra escolhida pelo usuário existe
        {
            posicoes = new int[palavra.Length];
            bool existe = false;

            for (int i = 0; i < palavra.Length; i++)
            {
                posicoes[i] = -1;
                if (Char.ToLower(palavra[i]) == Char.ToLower(letra))
                    existe = true;
            }
            return existe;
        }

        public int[] PosicoesLetra(char letra)   // Retorna um vetor posições com a posição das letras
        {
            int index = 0;

            for (int i = 0; i < palavra.Length; i++)
            {
                if (Char.ToLower(Convert.ToChar(palavra[i])) == Char.ToLower(letra))
                {
                    posicoes[index] = i;
                    index++;
                }
            }
            return posicoes;
        }

        public void GravarDados (DataGridView dgvAlgumaCoisa, int i)   // Escreve a letra na posição correta da palavra
        {
            dgvAlgumaCoisa.Rows[0].Cells[(posicoes[i])].Value = palavra[(posicoes[i])];
        }
    }
}
