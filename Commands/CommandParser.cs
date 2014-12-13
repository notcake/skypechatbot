using System;
using System.Collections.Generic;
using System.Text;

namespace ChatBot.Commands
{
    public class CommandParser
    {
        private int Position;
        private string Command;

        public CommandParser()
        {
        }

        public Tuple<string, string[], string> Parse(string command)
        {
            // command           := whitespace? (argument whitespace?)*
            // argument          := unquoted-argument | quoted-argument
            // unquoted-argument := ([^ \t"]|\.)+
            // quoted-argument   := "([^"]|\.)*"
            // whitespace        := [ \t\r\n]*

            this.Command = command;
            this.Position = 0;

            List<string> arguments = new List<string>();
            string fullArguments = "";

            while (true)
            {
                this.Whitespace();

                if (arguments.Count == 1)
                {
                    fullArguments = this.Command.Substring(this.Position);
                }

                string argument = this.Argument();
                if (argument == null) { break; }
                arguments.Add(argument);
            }

            string commandName = "";
            if (arguments.Count > 0)
            {
                commandName = arguments[0].Substring(1);
                arguments.RemoveAt(0);
            }
            return Tuple.Create(commandName, arguments.ToArray(), fullArguments);
        }

        private string Argument()
        {
            // argument          := unquoted-argument | quoted-argument
            // unquoted-argument := ([^ \t"]|\.)+
            // quoted-argument   := "([^"]|\.)*"

            if (this.IsOutOfInput) { return null; }

            if (this.Peek() == '\"')
            {
                return this.QuotedArgument();
            }
            return this.UnquotedArgument();
        }

        private string UnquotedArgument()
        {
            // unquoted-argument := ([^ \t"]|\.)+

            if (this.IsOutOfInput) { return null; }

            StringBuilder argument = new StringBuilder();

            while (!this.IsOutOfInput &&
                   !char.IsWhiteSpace(this.Peek()))
            {
                if (this.Peek() == '\"') { break; }
                if (this.Accept('\\'))
                {
                    if (this.IsOutOfInput) { argument.Append('\\'); break; }

                    switch (this.Peek())
                    {
                        case '\\':
                        case '\"': argument.Append(this.AcceptAny()); break;
                        case ' ':
                        case '\r':
                        case '\n':
                        case '\t': argument.Append(this.AcceptAny()); break;
                        case 'r': argument.Append('\r'); break;
                        case 'n': argument.Append('\n'); break;
                        case 't': argument.Append('\t'); break;
                        default:
                            argument.Append('\\');
                            argument.Append(this.AcceptAny());
                            break;
                    }
                }
                else
                {
                    argument.Append(this.AcceptAny());
                }
            }

            return argument.ToString();
        }

        private string QuotedArgument()
        {
            // quoted-argument := "([^"]|\.)*"

            if (this.IsOutOfInput) { return null; }

            // "
            if (!this.Accept('\"')) { return null; }

            StringBuilder argument = new StringBuilder();

            while (!this.IsOutOfInput &&
                   !this.Accept('\"'))
            {
                if (this.Accept('\\'))
                {
                    if (this.IsOutOfInput) { argument.Append('\\'); break; }

                    switch (this.Peek())
                    {
                        case '\\':
                        case '\"': argument.Append(this.AcceptAny()); break;
                        case ' ':
                        case '\r':
                        case '\n':
                        case '\t': argument.Append(this.AcceptAny()); break;
                        case 'r': argument.Append('\r'); break;
                        case 'n': argument.Append('\n'); break;
                        case 't': argument.Append('\t'); break;
                        default:
                            argument.Append('\\');
                            argument.Append(this.AcceptAny());
                            break;
                    }
                }
                else
                {
                    argument.Append(this.AcceptAny());
                }
            }

            return argument.ToString();
        }

        private void Whitespace()
        {
            // whitespace := [ \t\r\n]*

            // Nom all the whitespace
            while (!this.IsOutOfInput &&
                   char.IsWhiteSpace(this.Command[this.Position]))
            {
                this.Position++;
            }
        }

        private bool Accept(char c)
        {
            if (this.IsOutOfInput) { return false; }
            if (this.Command[this.Position] != c) { return false; }

            this.Position = this.Position + 1;

            return true;
        }

        private char AcceptAny()
        {
            if (this.IsOutOfInput) { return '\0'; }
            this.Position = this.Position + 1;
            return this.Command[this.Position - 1];
        }

        private char Peek()
        {
            if (this.IsOutOfInput) { return '\0'; }

            return this.Command[this.Position];
        }

        private bool IsOutOfInput
        {
            get
            {
                return this.Position >= this.Command.Length;
            }
        }
    }
}