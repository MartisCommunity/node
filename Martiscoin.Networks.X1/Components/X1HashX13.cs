﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using HashLib;

namespace Martiscoin.NBitcoin.Crypto
{
    // this hashing class is not thread safe to use with static instances.
    // the hashing objects maintain state during hash calculation.
    // to use in a multi threaded environment create a new instance for every hash.

    public sealed class X1HashX13
    {
        private readonly List<IHash> hashers;

        private readonly object hashLock;

        private static readonly Lazy<X1HashX13> SingletonInstance = new Lazy<X1HashX13>(LazyThreadSafetyMode.PublicationOnly);

        public X1HashX13()
        {
            this.hashers = new List<IHash>
            {
                HashFactory.Crypto.SHA3.CreateBlake512(),
                HashFactory.Crypto.SHA3.CreateBlueMidnightWish512(),
                HashFactory.Crypto.SHA3.CreateGroestl512(),
                HashFactory.Crypto.SHA3.CreateSkein512_Custom(),
                HashFactory.Crypto.SHA3.CreateJH512(),
                HashFactory.Crypto.SHA3.CreateKeccak512(),
                HashFactory.Crypto.SHA3.CreateLuffa512(),
                HashFactory.Crypto.SHA3.CreateCubeHash512(),
                HashFactory.Crypto.SHA3.CreateSHAvite3_512_Custom(),
                HashFactory.Crypto.SHA3.CreateSIMD512(),
                HashFactory.Crypto.SHA3.CreateEcho512(),
                HashFactory.Crypto.SHA3.CreateHamsi512(),
                HashFactory.Crypto.SHA3.CreateFugue512(),
            };

            this.hashLock = new object();
            this.Multiplier = 1;
        }

        public uint Multiplier { get; private set; }

        /// <summary>
        /// using the instance method is not thread safe. 
        /// to calling the hashing method in a multi threaded environment use the create() method
        /// </summary>
        public static X1HashX13 Instance => SingletonInstance.Value;

        public static X1HashX13 Create()
        {
            return new X1HashX13();
        }

        public byte[] Hash(byte[] input)
        {
            byte[] buffer = input;

            lock (this.hashLock)
            {
                foreach (IHash hasher in this.hashers)
                {
                    buffer = hasher.ComputeBytes(buffer).GetBytes();
                }

                foreach (IHash hasher in this.hashers)
                {
                    var bufferTemp = hasher.ComputeBytes(buffer).GetBytes();
                    if (bufferTemp[39] % 2 == 0) continue;
                    buffer = bufferTemp;
                }
            }

            return buffer.ToArray();
        }
    }
}
