using System;
using System.Collections.Generic;
using System.Text;

namespace ChatBot.Commands
{
    public class CommandParser
    {
        private string Command;
        private int Position;

        private bool IsOutOfInput => Position >= Command.Length;

        public Tuple<string, string[], string> Parse(string command)
        {
            // command           := whitespace? (argument whitespace?)*
            // argument          := unquoted-argument | quoted-argument
            // unquoted-argument := ([^ \t"]|\.)+
            // quoted-argument   := "([^"]|\.)*"
            // whitespace        := [ \t\r\n]*

            Command = command;
            Position = 0;

            var arguments = new List<string>();
            var fullArguments = "";

            while (true)
            {
                Whitespace();

                if (arguments.Count == 1)
                {
                    fullArguments = Command.Substring(Position);
                }

                var argument = Argument();
                if (argument == null)
                {
                    break;
                }
                arguments.Add(argument);
            }

            var commandName = "";
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

            if (IsOutOfInput)
            {
                return null;
            }

            if (Peek() == '\"')
            {
                return QuotedArgument();
            }
            return UnquotedArgument();
        }

        private string UnquotedArgument()
        {
            // unquoted-argument := ([^ \t"]|\.)+

            if (IsOutOfInput)
            {
                return null;
            }

            var argument = new StringBuilder();

            while (!IsOutOfInput &&
                   !char.IsWhiteSpace(Peek()))
            {
                if (Peek() == '\"')
                {
                    break;
                }
                if (Accept('\\'))
                {
                    if (IsOutOfInput)
                    {
                        argument.Append('\\');
                        break;
                    }

                    switch (Peek())
                    {
                        case '\\':
                        case '\"':
                            argument.Append(AcceptAny());
                            break;
                        case ' ':
                        case '\r':
                        case '\n':
                        case '\t':
                            argument.Append(AcceptAny());
                            break;
                        case 'r':
                            argument.Append('\r');
                            break;
                        case 'n':
                            argument.Append('\n');
                            break;
                        case 't':
                            argument.Append('\t');
                            break;
                        default:
                            argument.Append('\\');
                            argument.Append(AcceptAny());
                            break;
                    }
                }
                else
                {
                    argument.Append(AcceptAny());
                }
            }

            return argument.ToString();
        }

        private string QuotedArgument()
        {
            // quoted-argument := "([^"]|\.)*"

            if (IsOutOfInput)
            {
                return null;
            }

            // "
            if (!Accept('\"'))
            {
                return null;
            }

            var argument = new StringBuilder();

            while (!IsOutOfInput &&
                   !Accept('\"'))
            {
                if (Accept('\\'))
                {
                    if (IsOutOfInput)
                    {
                        argument.Append('\\');
                        break;
                    }

                    switch (Peek())
                    {
                        case '\\':
                        case '\"':
                            argument.Append(AcceptAny());
                            break;
                        case ' ':
                        case '\r':
                        case '\n':
                        case '\t':
                            argument.Append(AcceptAny());
                            break;
                        case 'r':
                            argument.Append('\r');
                            break;
                        case 'n':
                            argument.Append('\n');
                            break;
                        case 't':
                            argument.Append('\t');
                            break;
                        default:
                            argument.Append('\\');
                            argument.Append(AcceptAny());
                            break;
                    }
                }
                else
                {
                    argument.Append(AcceptAny());
                }
            }

            return argument.ToString();
        }

        private void Whitespace()
        {
            // whitespace := [ \t\r\n]*

            // Nom all the whitespace
            while (!IsOutOfInput &&
                   char.IsWhiteSpace(Command[Position]))
            {
                Position++;
            }
        }

        private bool Accept(char c)
        {
            if (IsOutOfInput)
            {
                return false;
            }
            if (Command[Position] != c)
            {
                return false;
            }

            Position = Position + 1;

            return true;
        }

        private char AcceptAny()
        {
            if (IsOutOfInput)
            {
                return '\0';
            }
            Position = Position + 1;
            return Command[Position - 1];
        }

        private char Peek()
        {
            if (IsOutOfInput)
            {
                return '\0';
            }

            return Command[Position];
        }
    }
}