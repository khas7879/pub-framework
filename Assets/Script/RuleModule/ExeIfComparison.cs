using UnityEngine;
using System.Collections;

using CJC.Framework.Rule.Base;
using System;

namespace CJC.Framework.Rule
{
	public class ExeIfComparison : RuleExcuter
	{
		public ExeIfComparison() : base() { IsMultiRoute = true; }

		protected override void OnExecute(IModelData target)
		{
			float param1 = ToolParser.FloatParse(Attributes[ERuleKey.Param1]);
			float param2 = ToolParser.FloatParse(Attributes[ERuleKey.Param2]);
			ECompareSymbol symbol = (ECompareSymbol)Enum.Parse(typeof(ECompareSymbol), Attributes[ERuleKey.CompareSymbol]);
			if (Compare(symbol, param1, param2))
				DoRoute(target, ERuleKey.RouteIf);
			else
				DoRoute(target, ERuleKey.RouteElse);
		}

		private bool Compare(ECompareSymbol symbol, float param1, float param2)
		{
			switch(symbol)
			{
				case ECompareSymbol.Equals:
					return param1.Equals(param2);
				case ECompareSymbol.NotEquals:
					return !param1.Equals(param2);
				case ECompareSymbol.LessThan:
					return param1 < param2;
				case ECompareSymbol.LessThanEquals:
					return param1 <= param2;
				case ECompareSymbol.MoreThan:
					return param1 > param2;
				case ECompareSymbol.MoreThanEquals:
					return param1 >= param2;
			}
			return false;
		}
	}

	public enum ECompareSymbol
	{
		Equals,
		NotEquals,
		LessThan,
		LessThanEquals,
		MoreThan,
		MoreThanEquals,
	}
}
