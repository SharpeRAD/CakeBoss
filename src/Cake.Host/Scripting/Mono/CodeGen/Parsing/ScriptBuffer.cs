﻿using System;
using System.IO;

namespace Cake.Host.Scripting.Mono.CodeGen.Parsing
{
    public sealed class ScriptBuffer : IDisposable
    {
        private readonly StringReader _reader;

        public int Position
        {
            get; private set;
        }

        public int PreviousToken
        {
            get; private set;
        }

        public int CurrentToken
        {
            get; private set;
        }

        public int NextToken
        {
            get; private set;
        }

        public bool ReachedEnd
        {
            get; private set;
        }

        public ScriptBuffer(string content)
        {
            _reader = new StringReader(content);

            // Set initial values.
            CurrentToken = ' ';
            Position = -1;
        }

        public void Dispose()
        {
            _reader.Dispose();
        }

        public bool Read()
        {
            if (_reader.Peek() == -1)
            {
                ReachedEnd = true;
                return false;
            }

            PreviousToken = CurrentToken;
            CurrentToken = _reader.Read();
            NextToken = _reader.Peek();
            Position++;

            return true;
        }

        public int Peek()
        {
            return _reader.Peek();
        }

        public void EatWhiteSpace()
        {
            while (true)
            {
                if (!Read())
                {
                    break;
                }
                var current = (char)CurrentToken;
                if (!char.IsWhiteSpace(current))
                {
                    break;
                }
            }
        }
    }
}