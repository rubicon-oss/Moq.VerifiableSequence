﻿// BSD 3-Clause License
// 
// Copyright (c) RUBICON IT GmbH
// All rights reserved.
// 
// Redistribution and use in source and binary forms, with or without
// modification, are permitted provided that the following conditions are met:
// 
// * Redistributions of source code must retain the above copyright notice, this
//   list of conditions and the following disclaimer.
// 
// * Redistributions in binary form must reproduce the above copyright notice,
//   this list of conditions and the following disclaimer in the documentation
//   and/or other materials provided with the distribution.
// 
// * Neither the name of the copyright holder nor the names of its
//   contributors may be used to endorse or promote products derived from
//   this software without specific prior written permission.
// 
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS"
// AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE
// IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
// DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE
// FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL
// DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR
// SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER
// CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY,
// OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE
// OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

using System;

namespace Moq.Protected;

/// <summary>
/// Enables the <see cref="Protected{T}"/> method on <see cref="MockWrapper{T}"/>, to allow setups for protected members by using their 
/// name as a string, rather than strong-typing them which is not possible due to their visibility.
/// </summary>
public static class ProtectedExtensions
{
  /// <summary>
  /// Enable protected setups for the mock.
  /// </summary>
  /// <typeparam name="T">Mocked object type. Typically omitted as it can be inferred from the mock instance.</typeparam>
  /// <param name="mock">The mock to set the protected setups on.</param>
  public static ProtectedMockWrapper<T> Protected<T> (this MockWrapper<T> mock)
      where T : class
  {
    if (mock == null) throw new ArgumentNullException(nameof(mock));

    return new ProtectedMockWrapper<T>(mock);
  }
}