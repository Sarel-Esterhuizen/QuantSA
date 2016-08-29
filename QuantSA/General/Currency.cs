﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace General
{
    public class Currency
    {
        private string code;

        public Currency(string code)
        { this.code = code.ToUpper(); }

        /// <summary>
        /// Make sure that the string value will be unique for each unique instance.
        /// </summary>
        /// <returns></returns>
        public sealed override string ToString()
        { return code; }


        public sealed override bool Equals(object obj)
        {
            return ToString().Equals(obj.ToString());
        }

        public sealed override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

    }
}