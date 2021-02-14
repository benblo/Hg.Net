// 
//  CommandResult.cs
//  
//  Author:
//       Levi Bard <levi@unity3d.com>
//  
//  Copyright (c) 2011 Levi Bard
// 
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU Lesser General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
// 
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU Lesser General Public License for more details.
// 
//  You should have received a copy of the GNU Lesser General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.

namespace Mercurial.Net
{
	/// <summary>
	/// Represents the result of a command
	/// </summary>
	public class CommandResult
	{
		/// <summary>
		/// The output from the command
		/// </summary>
		public string Output { get; private set; }
		
		/// <summary>
		/// The error output from the command
		/// </summary>
		public string Error { get; private set; }
		
		/// <summary>
		/// The result code from the command
		/// </summary>
		public int Result { get; private set; }

		internal CommandResult (string output, string error, int result)
		{
			Output = output;
			Error = error;
			Result = result;
		}
	}
}

