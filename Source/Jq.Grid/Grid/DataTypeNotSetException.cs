using System;
namespace Jq.Grid
{
	internal class DataTypeNotSetException : Exception
	{
		public DataTypeNotSetException(string message) : base(message)
		{
		}
	}
}
