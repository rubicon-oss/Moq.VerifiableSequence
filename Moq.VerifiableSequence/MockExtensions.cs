// BSD 3-Clause License
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

namespace Moq;

/// <summary>
/// Contains extension methods that are related to <see cref="Mock"/> instances.
/// </summary>
public static class MockExtensions
{
  /// <summary>
  /// Perform an expectation in a <see cref="VerifiableSequence"/>. Call <see cref="VerifiableSequence"/>.<see cref="VerifiableSequence.Verify"/>
  /// to ensure all setups in the sequence were executed in the expected order.
  /// </summary>
  public static MockWrapper<T> InVerifiableSequence<T> (this Mock<T> mock, VerifiableSequence sequence) where T : class
  {
    if (mock == null) throw new ArgumentNullException(nameof(mock));
    if (sequence == null) throw new ArgumentNullException(nameof(sequence));

    return new MockWrapper<T>(mock, sequence);
  }
}