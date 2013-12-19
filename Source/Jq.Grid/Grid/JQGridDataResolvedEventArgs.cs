using System;
using System.Linq;
namespace Jq.Grid
{
	public class JQGridDataResolvedEventArgs : EventArgs
	{
		public IQueryable _currentData;
		public IQueryable _allData;
		public JQGrid _gridModel;
		public JQGrid GridModel
		{
			get
			{
				return this._gridModel;
			}
			set
			{
				this._gridModel = value;
			}
		}
		public IQueryable CurrentData
		{
			get
			{
				return this._currentData;
			}
			set
			{
				this._currentData = value;
			}
		}
		public IQueryable AllData
		{
			get
			{
				return this._allData;
			}
			set
			{
				this._allData = value;
			}
		}
		public JQGridDataResolvedEventArgs(JQGrid gridModel, IQueryable currentData, IQueryable allData)
		{
			this._currentData = currentData;
			this._allData = allData;
			this._gridModel = gridModel;
		}
	}
}
