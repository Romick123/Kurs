using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace work
{
    public class Troyka
    {
        public Token operand1;
        public Token operand2;
        public Token operat;
        public Troyka(Token operat, Token opd2, Token opd1)
        {
            this.operat = operat;
            operand2 = opd2;
            operand1 = opd1;
        }
    }
    public class Bauer_Zamelzon
    {
        public List<Troyka> listTroyka = new List<Troyka>();
        private List<Token> tokens = new List<Token>();
        private Stack<Token> E = new Stack<Token>();
        private Stack<Token> T = new Stack<Token>();
        private int lex = 0;
        public int index = 1;
        public List<string> strings = new List<string>();
        private void ListTroykaToStrings(List<Troyka> listTroyka)
        {
            int ind = 1;
            foreach (Troyka t in listTroyka)
            {
                strings.Add($"M{ind}: {t.operat.Value}, {t.operand1.Value}, {t.operand2.Value}");
                ind++;
            }
        }
        private Token GetLexem(int lex)
        {
            return tokens[lex];
        }
        public Bauer_Zamelzon(List<Token> expr)
        {
            tokens = expr;
        }
        private void K_id()
        {
            E.Push(GetLexem(lex));
            lex++;
        }
        private void K_op()
        {
            if (E.Count < 2)
                throw new Exception("Невозможно выполнить арифметическое выражение: число операндов не удовлетворяет условию");
            Troyka k = new Troyka(T.Pop(), E.Pop(), E.Pop());
            listTroyka.Add(k);
            Token token = new Token(TokenType.IDENTIFIER);
            token.Value = $"M{index}";
            E.Push(token);
            index++;
        }
        public void Start()
        {
            CheckContains(tokens);
            CheckSyntax();
            Method();
            ListTroykaToStrings(listTroyka);
        }
        private void Method()
        {
            if (lex == tokens.Count)
            {
                if (T.Count == 0)
                {
                    return;
                }
                else
                {
                    EndList();
                }
            }
            else
            {
                switch (GetLexem(lex).Type)
                {
                    case TokenType.IDENTIFIER:
                        K_id();
                        break;
                    case TokenType.LITERAL:
                        K_id();
                        break;
                    case TokenType.LPAR:
                        Lpar();
                        break;
                    case TokenType.PLUS:
                        PlusOrMinus();
                        break;
                    case TokenType.MINUS:
                        PlusOrMinus();
                        break;
                    case TokenType.MULTIPLY:
                        MultiplicationOrDivision();
                        break;
                    case TokenType.DIVIDE:
                        MultiplicationOrDivision();
                        break;
                    case TokenType.RPAR:
                        Rpar();
                        break;
                }
            }
            Method();
        }
        private void Rpar()
        {
            if (T.Count == 0)
                D5("лишняя \")\"");
            else
            {
                switch (T.Peek().Type)
                {
                    case TokenType.LPAR:
                        D3();
                        break;
                    case TokenType.PLUS:
                        D4();
                        break;
                    case TokenType.MINUS:
                        D4();
                        break;
                    case TokenType.MULTIPLY:
                        D4();
                        break;
                    case TokenType.DIVIDE:
                        D4();
                        break;
                }
            }
        }
        private void MultiplicationOrDivision()
        {
            if (T.Count == 0)
                D1();
            else
            {
                switch (T.Peek().Type)
                {
                    case TokenType.LPAR:
                        D1();
                        break;
                    case TokenType.PLUS:
                        D1();
                        break;
                    case TokenType.MINUS:
                        D1();
                        break;
                    case TokenType.MULTIPLY:
                        D2();
                        break;
                    case TokenType.DIVIDE:
                        D2();
                        break;
                }
            }
        }
        private void PlusOrMinus()
        {
            if (T.Count == 0)
                D1();
            else
            {
                switch (T.Peek().Type)
                {
                    case TokenType.LPAR:
                        D1();
                        break;
                    case TokenType.PLUS:
                        D2();
                        break;
                    case TokenType.MINUS:
                        D2();
                        break;
                    case TokenType.MULTIPLY:
                        D4();
                        break;
                    case TokenType.DIVIDE:
                        D4();
                        break;
                }
            }
        }
        private void Lpar()
        {
            if (T.Count == 0)
                D1();
            else
            {
                switch (T.Peek().Type)
                {
                    case TokenType.LPAR:
                        D1();
                        break;
                    case TokenType.PLUS:
                        D1();
                        break;
                    case TokenType.MINUS:
                        D1();
                        break;
                    case TokenType.MULTIPLY:
                        D1();
                        break;
                    case TokenType.DIVIDE:
                        D1();
                        break;
                }
            }
        }
        private void EndList()
        {
            if (T.Count == 0)
            {
                return;
            }
            else
            {
                switch (T.Peek().Type)
                {
                    case TokenType.LPAR:
                        D5("лишняя \"(\"");
                        break;
                    case TokenType.PLUS:
                        D4();
                        break;
                    case TokenType.MINUS:
                        D4();
                        break;
                    case TokenType.MULTIPLY:
                        D4();
                        break;
                    case TokenType.DIVIDE:
                        D4();
                        break;
                }
            }
        }
        private void D1()
        {
            T.Push(GetLexem(lex));
            lex++;
        }
        private void D2()
        {
            K_op();
            T.Push(GetLexem(lex));
            lex++;
        }
        private void D3()
        {
            T.Pop();
            lex++;
        }
        private void D4()
        {
            K_op();
        }
        private void D5(string error)
        {
            throw new Exception($"Ошибка в арифметическом выражении: {error}");
        }
        private void CheckSyntax()
        {
            int current = 0;
            int next = 1;

            while (current < tokens.Count - 1)
            {
                if (tokens[current].Type == TokenType.IDENTIFIER ||
                    tokens[current].Type == TokenType.LITERAL ||
                    tokens[current].Type == TokenType.RPAR)
                {
                    if (tokens[next].Type == TokenType.IDENTIFIER ||
                       tokens[next].Type == TokenType.LITERAL ||
                       tokens[next].Type == TokenType.LPAR)
                        throw new Exception($"Ошибка в арифметическом выражении. Ожидалось: или +, или -, или *, или /, а встретилось: {tokens[next].Value}");
                    else { current++; next++; }
                }
                else { current++; next++; }
            }
        }
        private void CheckContains(List<Token> tokens)
        {
            foreach (Token token in tokens)
            {
                if (token.Type != TokenType.LPAR &&
                    token.Type != TokenType.RPAR &&
                    token.Type != TokenType.MINUS &&
                    token.Type != TokenType.PLUS &&
                    token.Type != TokenType.DIVIDE &&
                    token.Type != TokenType.MULTIPLY &&
                    token.Type != TokenType.IDENTIFIER &&
                    token.Type != TokenType.LITERAL)
                    throw new Exception($"Недопустимый символ в арифмeтическом выражении: {token.Value}");
            }
        }
    }
}

