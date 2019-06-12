using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

// Rafael Pak Bragagnolo - 18206

namespace ApForca
{
    public partial class Forca : Form
    {
        Palavra[] asPalavras = new Palavra[100];
        Palavra aPalavra = new Palavra();
        string palavra;
        int tempo, pontos = 0, erros = 0;
        bool ganhou;
        string nomePlayer;

        public Forca()
        {
            InitializeComponent();
            pb2.Controls.Add(pbDecoracao1);
            pbDecoracao1.Location = new Point(0, 0);
            pbDecoracao1.BackColor = Color.Transparent;
        }

        private void Form1_Load(object sender, EventArgs e)   // O que acontece quando o Formulário é carregado
        {
            // A maioria das coisas é deixada invisível
            Desabilitação();

            tssLbData.Text = "Data: " + DateTime.Now.ToShortDateString() + "   |";   // Apresenta a data atual no toolStatusStripLabel de Data
            tssLbHora.Text = "Hora: " + DateTime.Now.ToString("HH:mm") + "   |";     // Apresenta o horário atual no toolStatusStripLabel de Horário

            dgvPalavra.RowTemplate.Height = dgvPalavra.Height;

            if (dlgAbrir.ShowDialog() == DialogResult.OK)   // Escolher o arquivo para ser lido e cada posição do vetor asPalavras recebe uma palavra e dica que estão em uma linha
            {
                var PlvDicas = new StreamReader(dlgAbrir.FileName, Encoding.UTF7);

                for (int i = 0; i <= 99; i++)
                {
                    asPalavras[i] = new Palavra();
                    asPalavras[i].LerDados(PlvDicas);
                }
                PlvDicas.Close();
            }
        }

        private void btnIniciar_MouseClick(object sender, MouseEventArgs e)   // O que acontece quando o botão Iniciar é clicado pelo mouse
        {
            if (String.IsNullOrWhiteSpace(txtNome.Text))   // Se o usuário não colocar nome, recebe um aviso, e não executa as demais funções
            {
                MessageBox.Show("Nome inválido !!");
                return;
            }

            // Algumas das coisas que não estavam visíveis, voltam a ficar visíveis
            Habilitação();
            // Os botôes são postos em enabled, para caso haja o reinício do jogo
            btnA.Enabled = true;
            btnB.Enabled = true;
            btnC.Enabled = true;
            btnD.Enabled = true;
            btnE.Enabled = true;
            btnF.Enabled = true;
            btnG.Enabled = true;
            btnH.Enabled = true;
            btnI.Enabled = true;
            btnJ.Enabled = true;
            btnK.Enabled = true;
            btnL.Enabled = true;
            btnM.Enabled = true;
            btnN.Enabled = true;
            btnO.Enabled = true;
            btnP.Enabled = true;
            btnQ.Enabled = true;
            btnR.Enabled = true;
            btnS.Enabled = true;
            btnT.Enabled = true;
            btnU.Enabled = true;
            btnV.Enabled = true;
            btnW.Enabled = true;
            btnX.Enabled = true;
            btnY.Enabled = true;
            btnZ.Enabled = true;
            btnAcentoA1.Enabled = true;
            btnAcentoA2.Enabled = true;
            btnAcentoA3.Enabled = true;
            btnAcentoE1.Enabled = true;
            btnAcentoE2.Enabled = true;
            btnAcentoI.Enabled = true;
            btnAcentoO1.Enabled = true;
            btnAcentoO2.Enabled = true;
            btnAcentoO3.Enabled = true;
            btnAcentoU.Enabled = true;
            btnÇ.Enabled = true;
            btnHifen.Enabled = true;
            btnNada.Enabled = true;

            ganhou = false;                               // A variável ganhou recebe false

            var aleatorio = new Random();                 // Cria uma instância da classe Random
            aPalavra = asPalavras[aleatorio.Next(100)];    // O objeto da classe aPalavra recebe o valor aleatório de uma palavra e dica de uma posição do vetor

            palavra = aPalavra.PalavraEscolhida;          // string palavra recebe a palavra que foi escolhida
            dgvPalavra.ColumnCount = palavra.Length;      // o DataGridView fica com as colunas com um número igual ao tamanho da palavra
            dgvPalavra.RowCount = 1;                      // o DataGridView fica com uma linha apenas

            if (cbxDica.Checked)   // Caso o checkBox dica esteja verificado, o tempo será menor e a dica da palavra será apresentada no label da dica
            {
                tempo = 70;
                lbDica.Text = aPalavra.DicaEscolhida;
            }
            else   // Caso o checkBox não esteja verificado, o tempo será maior
                tempo = 140;

            tmrTempo.Enabled = true;
            cbxDica.Enabled = false;                      // O checkBox é "desligado" para não ser mudado durante a rodada
        }

        private void btnNada_Click(object sender, EventArgs e)    // Evento do click em todos os botões
        {
            Button botao = (Button)sender;   // Foi criada uma instância do objeto botão que devolve o valor que o botão tem
            char letra = Convert.ToChar(botao.Text);              // A string que tem dentro do botao, é convertida para char

            if (aPalavra.ExisteLetra(letra))   // Se a letra existe, então é necessário mostrar a letra no GridView, aumentar a pontuação e "desligar" o botão
            {
                int[] posicoes = aPalavra.PosicoesLetra(letra);
                pontos++;
                lbPontos.Text = Convert.ToString(pontos);
                for (int i = 0; i < aPalavra.PalavraEscolhida.Length; i++)
                {
                    if (posicoes[i] >= 0)
                    aPalavra.GravarDados(dgvPalavra, i);          // Aloca o valor da letra presente no botão no local certo da palavra
                    botao.Enabled = false;
                }

                string palavraDG = "";
                for (int i = 0; i < palavra.Length; i++)   // A variável palavraDg, recebe, letra por letra, a palavra escrita no GridView
                    palavraDG += dgvPalavra.Rows[0].Cells[i].Value;

                if (palavraDG == palavra)   // E então a compara com a palavra que foi escolhida, caso seja, a variável lógica ganhou recebe true
                    ganhou = true;

                if (ganhou)   // Quando a variável ganhou for true, então o método Ganhar() será executado
                {
                    Ganhar();
                }
            }
            else   // Se a letra não existe na palavra, então a contagem de erros aumenta, a de pontos diminui, e as partes do corpo vão sendo mostrados a cada erro
            {
                botao.Enabled = false;
                pontos--;
                lbPontos.Text = Convert.ToString(pontos);
                erros++;
                lbErros.Text = Convert.ToString(erros);

                if (erros == 1)
                    pbErro1.Visible = true;
                else
                if (erros == 2)
                    pbErro2.Visible = true;
                else
                if (erros == 3)
                    pbErro3.Visible = true;
                else
                if (erros == 4)
                    pbErro4.Visible = true;
                else
                if (erros == 5)
                    pbErro5.Visible = true;
                else
                if (erros == 6)
                    pbErro6.Visible = true;
                else
                if (erros == 7)
                    pbErro7.Visible = true;
                else
                if (erros == 8)
                {
                    pontos--;
                    pbErro8.Visible = true;
                    pbErroTotal.Visible = true;
                    pbDecoracao1.Visible = true;
                    pbDecoracao2.Visible = true;
                    pbDecoracao3.Visible = true;
                    Perder();
                }
            }
        }

        private void tmrTempo_Tick(object sender, EventArgs e)   // O tempo está contando, e se chegar no 0, o usuário perde
        {
            tempo--;
            lbTempo.Text = $"{tempo}s";
            if (tempo == 0)
                Perder();
        }

        public void Reiniciar()   // Quando termina uma rodada, e o usuário deseja recomeçar, esse método reinicializa tudo
        {
            palavra = " ";
            lbDica.Text = "__________________________________________________";
            pontos = 0;
            lbPontos.Text = Convert.ToString(pontos);
            erros = 0;
            lbErros.Text = Convert.ToString(erros);
            txtNome.Text = "";
            Desabilitação();
            cbxDica.Enabled = true;
            btnIniciar.Enabled = true;
        }

        public void Ganhar()   // Caso o usuário ganhe
        {
            dgvPalavra.Columns.Clear();
            dgvPalavra.Rows.Clear();
            tmrTempo.Stop();   // O timer para
            Rankear();
            DialogResult dialogResult = MessageBox.Show($"Parabéns, {txtNome.Text}, você ganhou !!\nDeseja jogar novamente ?", "A rodada acabou !!",MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                Reiniciar();
            }
            else
            if (dialogResult == DialogResult.No)
            {
                Close();
            }
        }

        public void Perder()   // Caso o usuário perca
        {
            dgvPalavra.Columns.Clear();
            dgvPalavra.Rows.Clear();
            tmrTempo.Stop();   // O timer para
            pbPerdeu.Visible = true;
            DialogResult dialogResult = MessageBox.Show($"Você perdeu {txtNome.Text} !!\nDeseja jogar novamente ?", "A rodada acabou !!", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                Reiniciar();
            }
            else
            if (dialogResult == DialogResult.No)
            {
                Close();
            }
        }

        public void Rankear()   // O nome e a pontuação do jogador são armazenadas e depois apresentadas para o jogador no final do jogo
        {
            string lugarDoRanking = dlgAbrir.FileName.Substring(0, dlgAbrir.FileName.IndexOf("ForcaPalavrasDicas.txt")) + @"\Ranking.txt";
            var Leitor = new StreamReader(lugarDoRanking);
            string linha = "";
            int numeroDeRegistros = 0;
            bool Existe = false;
            nomePlayer = txtNome.Text;
            nomePlayer = nomePlayer.PadRight(txtNome.MaxLength, ' ');

            while (!Leitor.EndOfStream)   // O leitor lê linha por linha, e ve se tem um nome igual ao noem atual no textBox Nome
            {
                linha = Leitor.ReadLine();
                string nomeRanking = linha.Substring(0, 45);
                numeroDeRegistros++;

                if (nomeRanking == nomePlayer)
                    Existe = true;
            }
            Leitor.Close();

            Leitor = new StreamReader(lugarDoRanking);
            string[] line = new string[numeroDeRegistros];
            for (int i = 0; i < numeroDeRegistros; i++)
                line[i] = Leitor.ReadLine();
            Leitor.Close();

            var Escritor = new StreamWriter(lugarDoRanking);

            if (Existe)   // Caso o nome já exista, o Escritor escreve novamente o arquivo inteiro, com os valores atualizados
            {
                for (int i = 0; i < numeroDeRegistros; i++)
                {
                    linha = line[i];
                    if (nomePlayer == linha.Substring(0, linha.IndexOf("=>") - 1))
                    {
                        int PontuacaoAntiga = int.Parse(linha.Substring(line[i].IndexOf("=>") + 2));
                        int PontuacaoNova = PontuacaoAntiga + pontos;

                        linha = nomePlayer + " => " + PontuacaoNova;
                        line[i] = linha;
                    }
                }
                for (int i = 0; i < numeroDeRegistros; i++)
                    Escritor.WriteLine(line[i]);
            }
            else   // Caso o nome não exista, o Escritor escreve novamente o arquivo e adiciona mais uma linha para adicionar esse novo usuário ao Rank
            {
                string novaLinha = nomePlayer + " => " + pontos.ToString();
                for (int i = 0; i < numeroDeRegistros; i++)
                    Escritor.WriteLine(line[i]);
                Escritor.WriteLine(novaLinha);
            }
            Escritor.Close();
        }

        public void Desabilitação()
        {
            btnA.Visible = false;
            btnB.Visible = false;
            btnC.Visible = false;
            btnD.Visible = false;
            btnE.Visible = false;
            btnF.Visible = false;
            btnG.Visible = false;
            btnH.Visible = false;
            btnI.Visible = false;
            btnJ.Visible = false;
            btnK.Visible = false;
            btnL.Visible = false;
            btnM.Visible = false;
            btnN.Visible = false;
            btnO.Visible = false;
            btnP.Visible = false;
            btnQ.Visible = false;
            btnR.Visible = false;
            btnS.Visible = false;
            btnT.Visible = false;
            btnU.Visible = false;
            btnV.Visible = false;
            btnW.Visible = false;
            btnX.Visible = false;
            btnY.Visible = false;
            btnZ.Visible = false;
            btnÇ.Visible = false;
            btnAcentoA1.Visible = false;
            btnAcentoA2.Visible = false;
            btnAcentoA3.Visible = false;
            btnAcentoE1.Visible = false;
            btnAcentoE2.Visible = false;
            btnAcentoI.Visible = false;
            btnAcentoO1.Visible = false;
            btnAcentoO2.Visible = false;
            btnAcentoO3.Visible = false;
            btnAcentoU.Visible = false;
            btnHifen.Visible = false;
            btnNada.Visible = false;
            pb1.Visible = false;
            pb2.Visible = false;
            pb3.Visible = false;
            pb4.Visible = false;
            pb5.Visible = false;
            pb6.Visible = false;
            pb7.Visible = false;
            pbDecoracao1.Visible = false;
            pbDecoracao2.Visible = false;
            pbDecoracao3.Visible = false;
            pbErro1.Visible = false;
            pbErro2.Visible = false;
            pbErro3.Visible = false;
            pbErro4.Visible = false;
            pbErro5.Visible = false;
            pbErro6.Visible = false;
            pbErro7.Visible = false;
            pbErro8.Visible = false;
            pbErroTotal.Visible = false;
            pbPerdeu.Visible = false;
            lbDica.Visible = false;
            lbDica2.Visible = false;
            lbErros.Visible = false;
            lbErros2.Visible = false;
            lbPontos.Visible = false;
            lbPontos2.Visible = false;
            lbTempo.Visible = false;
            lbTempo2.Visible = false;
        }

        public void Habilitação()
        {
            btnA.Visible = true;
            btnB.Visible = true;
            btnC.Visible = true;
            btnD.Visible = true;
            btnE.Visible = true;
            btnF.Visible = true;
            btnG.Visible = true;
            btnH.Visible = true;
            btnI.Visible = true;
            btnJ.Visible = true;
            btnK.Visible = true;
            btnL.Visible = true;
            btnM.Visible = true;
            btnN.Visible = true;
            btnO.Visible = true;
            btnP.Visible = true;
            btnQ.Visible = true;
            btnR.Visible = true;
            btnS.Visible = true;
            btnT.Visible = true;
            btnU.Visible = true;
            btnV.Visible = true;
            btnW.Visible = true;
            btnX.Visible = true;
            btnY.Visible = true;
            btnZ.Visible = true;
            btnÇ.Visible = true;
            btnAcentoA1.Visible = true;
            btnAcentoA2.Visible = true;
            btnAcentoA3.Visible = true;
            btnAcentoE1.Visible = true;
            btnAcentoE2.Visible = true;
            btnAcentoI.Visible = true;
            btnAcentoO1.Visible = true;
            btnAcentoO2.Visible = true;
            btnAcentoO3.Visible = true;
            btnAcentoU.Visible = true;
            btnHifen.Visible = true;
            btnNada.Visible = true;
            pb1.Visible = true;
            pb2.Visible = true;
            pb3.Visible = true;
            pb4.Visible = true;
            pb5.Visible = true;
            pb6.Visible = true;
            pb7.Visible = true;
            lbDica.Visible = true;
            lbDica2.Visible = true;
            lbErros.Visible = true;
            lbErros2.Visible = true;
            lbPontos.Visible = true;
            lbPontos2.Visible = true;
            lbTempo.Visible = true;
            lbTempo2.Visible = true;
            dgvPalavra.Visible = true;
            btnIniciar.Enabled = false;
        }
    }
}
