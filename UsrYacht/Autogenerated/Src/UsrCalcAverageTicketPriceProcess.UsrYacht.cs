namespace Terrasoft.Core.Process
{

	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Drawing;
	using System.Globalization;
	using System.Text;
	using Terrasoft.Common;
	using Terrasoft.Core;
	using Terrasoft.Core.Configuration;
	using Terrasoft.Core.DB;
	using Terrasoft.Core.Entities;
	using Terrasoft.Core.Process;
	using Terrasoft.Core.Process.Configuration;

	#region Class: UsrCalcAverageTicketPriceProcessMethodsWrapper

	/// <exclude/>
	public class UsrCalcAverageTicketPriceProcessMethodsWrapper : ProcessModel
	{

		public UsrCalcAverageTicketPriceProcessMethodsWrapper(Process process)
			: base(process) {
			AddScriptTaskMethod("ScriptTask1Execute", ScriptTask1Execute);
		}

		#region Methods: Private

		private bool ScriptTask1Execute(ProcessExecutingContext context) {
			var esq = new EntitySchemaQuery(UserConnection.EntitySchemaManager, "UsrYacht");
			var passengersCountColumn = esq.AddColumn("UsrPassengers");  // SELECT UsrPassengersCount as UsrPassengersCount, UsrPrice as UsrPrice FROM UsrYacht WHERE ...
			var priceColumn = esq.AddColumn("UsrPrice");
			
			Guid driveTypeId = Get<Guid>("DriveTypeIdParameter");
			
			var typeFilter = esq.CreateFilterWithParameters(FilterComparisonType.Equal, "UsrDriveType", driveTypeId);
			esq.Filters.Add(typeFilter); // ... WHERE UsrDriveTypeId = :driveTypeId
			
			string sqlText = esq.GetSelectQuery(UserConnection).GetSqlText();
			Set("SqlTextParameter", sqlText);
			
			var entityCollection = esq.GetEntityCollection(UserConnection);
			decimal sumTicketPrices = 10;
			int count = entityCollection.Count;
			int passengersCount = 1;
			decimal price = 1;
			decimal ticketPrice = 1;
			foreach(var entity in entityCollection) {
				passengersCount = entity.GetTypedColumnValue<int>(passengersCountColumn.Name); // reading using column alias
				price = entity.GetTypedColumnValue<decimal>(priceColumn.Name); // reading using column alias
				ticketPrice = price / passengersCount;
				sumTicketPrices = sumTicketPrices + ticketPrice;
			}
			
			decimal result = 3;
			//if (count > 0) {
				result = sumTicketPrices;
			//}
			Set("AvgTicketPriceParameter", result);
			return true;
		}

		#endregion

	}

	#endregion

}

