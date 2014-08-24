﻿//******************************************************************************************************
//  Scram.cs - Gbtc
//
//  Copyright © 2014, Grid Protection Alliance.  All Rights Reserved.
//
//  Licensed to the Grid Protection Alliance (GPA) under one or more contributor license agreements. See
//  the NOTICE file distributed with this work for additional information regarding copyright ownership.
//  The GPA licenses this file to you under the Eclipse Public License -v 1.0 (the "License"); you may
//  not use this file except in compliance with the License. You may obtain a copy of the License at:
//
//      http://www.opensource.org/licenses/eclipse-1.0.php
//
//  Unless agreed to in writing, the subject software distributed under the License is distributed on an
//  "AS-IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. Refer to the
//  License for the specific language governing permissions and limitations.
//
//  Code Modification History:
//  ----------------------------------------------------------------------------------------------------
//  8/23/2014 - Steven E. Chisholm
//       Generated original version of source code. 
//       
//
//******************************************************************************************************

using System;
using System.Text;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Digests;

namespace GSF.Security
{
    internal static class Scram
    {
        internal const int PasswordSize = 64;
        internal readonly static UTF8Encoding Utf8 = new UTF8Encoding(true);
        internal readonly static byte[] StringClientKey = Utf8.GetBytes("Client Key");
        internal readonly static byte[] StringServerKey = Utf8.GetBytes("Server Key");
        internal static IDigest CreateDigest()
        {
            return new Sha256Digest();
        }

        internal static byte[] XOR(byte[] a, byte[] b)
        {
            if (a.Length != b.Length)
                throw new Exception();
            byte[] rv = new byte[a.Length];
            for (int x = 0; x < a.Length; x++)
            {
                rv[x] = (byte)(a[x] ^ b[x]);
            }
            return rv;
        }

        internal static byte[] ComputeAuthMessage(byte[] serverNonce, byte[] clientNonce, byte[] salt, byte[] username, int iterations)
        {
            byte[] data = new byte[serverNonce.Length + clientNonce.Length + salt.Length + username.Length + 4];
            serverNonce.CopyTo(data, 0);
            clientNonce.CopyTo(data, serverNonce.Length);
            salt.CopyTo(data, serverNonce.Length + clientNonce.Length);
            username.CopyTo(data, serverNonce.Length + clientNonce.Length + salt.Length);
            data[data.Length - 4] = (byte)(iterations);
            data[data.Length - 3] = (byte)(iterations >> 8);
            data[data.Length - 2] = (byte)(iterations >> 16);
            data[data.Length - 1] = (byte)(iterations >> 24);
            return data;
        }

        internal static byte[] ComputeClientKey(byte[] saltedPassword)
        {
            return ComputeHMAC(saltedPassword, StringClientKey);
        }

        internal static byte[] ComputeServerKey(byte[] saltedPassword)
        {
            return ComputeHMAC(saltedPassword, StringServerKey);
        }

        internal static byte[] ComputeStoredKey(byte[] clientKey)
        {
            return Hash.Compute(CreateDigest(), clientKey);
        }

        internal static byte[] ComputeHMAC(byte[] key, byte[] message)
        {
            return HMAC.Compute(CreateDigest(), key, message);
        }

        internal static byte[] GenerateSaltedPassword(string password, byte[] salt, int iterations)
        {
            byte[] passwordBytes = Utf8.GetBytes(password.Normalize(NormalizationForm.FormKC));
            return GenerateSaltedPassword(passwordBytes, salt, iterations);
        }

        internal static byte[] GenerateSaltedPassword(byte[] passwordBytes, byte[] salt, int iterations)
        {
            using (var pass = new PBKDF2(HMACMethod.SHA512, passwordBytes, salt, iterations))
            {
                return pass.GetBytes(PasswordSize);
            }
        }
    }
}


