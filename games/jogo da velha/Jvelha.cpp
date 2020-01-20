#include <iostream>
#include <string.h>
#include <stdlib.h>
#include <stdio.h>
#include <string.h>
#include <time.h>
using namespace std;
//cabeçalhos de funcoes
void menuInicial();
void limpaTela() //funcao para limpar informacoes
{
    system("CLS");
}

void iniciaTabuleiro(char tabuleiro[3][3])
{
    int linha, coluna;
    for (linha = 0; linha < 3; linha++)
    {
        for (coluna = 0; coluna < 3; coluna++) //popula com agua
        {
            tabuleiro[linha][coluna] = '*';
        }
    }
}

void exibeTabuleiro(char tabuleiro[3][3])
{
    int i, linha, coluna;
    for (i = 0; i < 3; i++)
    {
        if (i == 0)
        {
            cout << "    ";
        }
        cout << i << " ";
    }
    cout << "\n";
    for (linha = 0; linha < 3; linha++)
    {
        cout << linha << "- ";
        for (coluna = 0; coluna < 3; coluna++) //imprime o tabuleiro
        {
            cout << " " << tabuleiro[linha][coluna]; //imprime a mascara
        }
        cout << "\n";
    }
}
int confereTabuleiro(char tabuleiro[3][3])
{
    int linha, coluna;
    for (linha = 0; linha < 3; linha++) //confere linhas
    {
        if (tabuleiro[linha][0] == 'X' && tabuleiro[linha][0] == tabuleiro[linha][1] && tabuleiro[linha][1] == tabuleiro[linha][2])
        {
            return 1;
        }
        else if (tabuleiro[linha][0] == 'O' && tabuleiro[linha][0] == tabuleiro[linha][1] && tabuleiro[linha][1] == tabuleiro[linha][2])
        {
            return 2;
        }
    }
    for (coluna = 0; coluna < 3; coluna++) //confere coluna
    {
        if (tabuleiro[0][coluna] == 'X' && tabuleiro[0][coluna] == tabuleiro[1][coluna] && tabuleiro[1][coluna] == tabuleiro[2][coluna])
        {
            return 1;
        }
        else if (tabuleiro[0][coluna] == 'O' && tabuleiro[0][coluna] == tabuleiro[1][coluna] && tabuleiro[1][coluna] == tabuleiro[2][coluna])
        {
            return 2;
        }
    }
    if (tabuleiro[0][0] != '*' && tabuleiro[0][0] == tabuleiro[1][1] && tabuleiro[1][1] == tabuleiro[2][2]) //confere diagonal principal
    {
        if (tabuleiro[0][0] == 'X')
        {
            return 1;
        }
        else
        {
            return 2;
        }
    }
    if (tabuleiro[0][2] != '*' && tabuleiro[0][2] == tabuleiro[1][1] && tabuleiro[1][1] == tabuleiro[2][0]) //confere diagonal secundaria
    {
        if (tabuleiro[0][2] == 'X')
        {
            return 1;
        }
        else
        {
            return 2;
        }
    }
    return 0;
}

void jogo(string *nomeJogador, string *nomeJogador2, int *pontosJogador, int *pontosJogador2)
{
    char tabuleiro[3][3]; //matriz do tabuleiro do jogo
    int linha, coluna;
    int linhaJogada = NULL, colunaJogada = NULL;
    int estadoDeJogo = 1;                  //mostra o estado do jogo
    int pontos = 0;                        //pontuacao atual
    int opcao;                             //opcoes no fim do jogo
    int maximodeTentativas = 0;            //maximo de tentativas
    int turno = 1;                         //turno do jogador
    string mensagem = "bem vindo ao jogo"; //mensagem enviada
    iniciaTabuleiro(tabuleiro);
    while (maximodeTentativas < 9 && estadoDeJogo == 1)
    {
        limpaTela();
        exibeTabuleiro(tabuleiro); //exibe o tabuleiro
        cout << "\t\t\t|" << maximodeTentativas << "| Tentativas";
        cout << "\t\tJogador X: " << *nomeJogador << "\t Pontos: " << *pontosJogador << "\t\tJogador O: " << *nomeJogador2 << "\tPontos: " << *pontosJogador2 << "\t[ " << mensagem << " ]";
        cout << "\nDigite uma linha: ";
        cin >> linhaJogada;
        if (linhaJogada > 2)
        {
            while (linhaJogada > 2)
            {
                limpaTela();
                exibeTabuleiro(tabuleiro);               //exibe o tabuleiro
                cout << "\nDIGITE UM VALOR MENOR QUE 3"; //correcao de valor maior que 3
                cout << "\nDigite uma linha: ";
                cin >> linhaJogada;
            }
        }
        cout << "\nDigite uma coluna: ";
        cin >> colunaJogada;
        if (colunaJogada > 2)
        {
            while (colunaJogada > 2)
            {
                limpaTela();
                exibeTabuleiro(tabuleiro);             //exibe o tabuleiro
                cout << "DIGITE UM VALOR MENOR QUE 3"; //correcao de valor maior que 3
                cout << "\nDigite uma linha: ";
                cin >> colunaJogada;
            }
        }
        if (tabuleiro[linhaJogada][colunaJogada] != '*')
        {
            while (tabuleiro[linhaJogada][colunaJogada] != '*' || colunaJogada > 2 || linhaJogada > 2)
            {
                limpaTela();
                exibeTabuleiro(tabuleiro); //exibe o tabuleiro
                cout << "posição já jogada";
                cout << "\nDigite uma linha: ";
                cin >> linhaJogada;
                cout << "\nDigite uma coluna: ";
                cin >> colunaJogada;
            }
        }
        if (turno == 1)
        {
            tabuleiro[linhaJogada][colunaJogada] = 'X';
            turno = 2;
        }
        else
        {
            tabuleiro[linhaJogada][colunaJogada] = 'O';
            turno = 1;
        }
        limpaTela();
        if (confereTabuleiro(tabuleiro) == 1)
        {
            cout << "O jogador X venceu\n";
            estadoDeJogo = 0;
            *pontosJogador = *pontosJogador + 1;
        }
        else if (confereTabuleiro(tabuleiro) == 2)
        {
            cout << "O jogador O venceu\n";
            estadoDeJogo = 0;
            *pontosJogador2 = *(pontosJogador2) + 1;
        }
        else
        {
            cout << "Nenhum jogador venceu\n";
        }
        maximodeTentativas++;
        exibeTabuleiro(tabuleiro); //exibe o tabuleiro
    }                              //case de fim de jogo
    cout << "\n\n fim de jogo,\n deseja \t 1- Jogar Novamente 2- Menu 3- Sair\n";
    cin >> opcao;
    switch (opcao)
    {
    case 1:
        jogo(nomeJogador, nomeJogador2, pontosJogador, pontosJogador2);
        break;
    case 2:
        menuInicial();
        break;
    case 3:
        cout << "Até mais!\n";
        exit;
        break;
    }
}

void menuInicial()
{
    int opcao = 0; //opcao escolhida no menu
    int pontosJogador = 0;
    int pontosJogador2 = 0; //pontuacao dos jogadores
    string nomeJogador;     //string do nome do jogador
    string nomeJogador2;    //string do nome do jogador
    limpaTela();
    while (opcao < 1 || opcao > 3) //enquanto o jogador nao digita o menu continua em loop
    {
        limpaTela();
        cout << "Bem vindo ao Jogo";
        cout << "\n 1 - Jogar";
        cout << "\n 2 - Sobre";
        cout << "\n 3 - Sair";
        cout << "\n Escolha uma opção e tecle ENTER: ";
        cin >> opcao;
        switch (opcao) //case de leitura da opcao
        {
        case 1:
            cout << "Digite seu nome: \n";
            cin >> nomeJogador;
            cout << "Digite seu nome: \n";
            cin >> nomeJogador2;
            cout << "Jogo iniciado\n";
            jogo(&nomeJogador, &nomeJogador2, &pontosJogador, &pontosJogador2);
            break;
        case 2:
            limpaTela();
            cout << "Informações do jogo\n";
            cout << "Developer for Caio Ribeiro inc #2020#";
            break;
        case 3:
            cout << "Até mais!\n";
            exit;
            break;
        }
    }
}

int main()
{
    int
        srand((unsigned)time(NULL)); //posicoes aleatorias
    setlocale(LC_ALL, "portuguese");
    menuInicial();
} //fim do programa