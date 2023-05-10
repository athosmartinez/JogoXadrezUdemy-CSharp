using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xadrez_Console.Tabuleiro
{
    internal class TabuleiroExecption : Exception
    {
        public TabuleiroExecption(string msg) : base(msg) { }
    }
}
