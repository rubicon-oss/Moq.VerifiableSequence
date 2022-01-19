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
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Moq;

/// <summary>
/// Class used to setup ordered expectations, which execution order can be asserted using <see cref="Verify"/>.
/// </summary>
public sealed class VerifiableSequence
{
  private int _expectedStepIndex = 0;
  private readonly List<string> _steps = new();

  /// <summary>
  /// Verifies that all setups were executed in the specified order using the <see cref="MockExtensions.InVerifiableSequence{T}"/> method.
  /// </summary>
  /// <exception cref="VerifiableSequenceException">Thrown when not all setups were matched.</exception>
  public void Verify ()
  {
    if (_expectedStepIndex < _steps.Count)
    {
      var stringBuilder = new StringBuilder();
      stringBuilder.AppendLine("Verification failed: Not all setups were matched:");

      foreach (var (step, i) in _steps.Select((s, i) => (s, i)))
      {
        if (i < _expectedStepIndex)
          stringBuilder.Append("- [OK]       '").Append(step).AppendLine("'");
        else
          stringBuilder.Append("- [Expected] '").Append(step).AppendLine("'");
      }

      throw new VerifiableSequenceException(stringBuilder.ToString());
    }
  }

  internal void AddStep (string action)
  {
    _steps.Add(action);
  }

  internal void RecordStep (string action)
  {
    if (_expectedStepIndex == _steps.Count)
      throw new VerifiableSequenceException($"All setups in this sequence were matched. Unexpected call '{action}'.");

    var expected = _steps[_expectedStepIndex++];

    if (expected != action)
      throw new VerifiableSequenceException($"Executed action '{action}' does not match setup '{expected}'.");
  }
}