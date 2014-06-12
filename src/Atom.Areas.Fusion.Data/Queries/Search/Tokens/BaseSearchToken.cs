using System.Collections.Generic;
using System.Linq;
using BeValued.Utilities.Extensions;

namespace Atom.Areas.Fusion.Data.Queries.Search.Tokens
{
	public abstract class BaseSearchToken
	{
		private readonly string _tokenName;
		private readonly string _regEx;
		protected string _autocompleteValue;
		private readonly int _split;
		protected object _value;

		protected BaseSearchToken(string regex, int split, string tokenName, string autoCompleteValue)
		{
			_regEx = regex;
			_split = split;
			_tokenName = tokenName;
			_value = autoCompleteValue;
		}

		protected string RegExShort()
		{
			return _split == 0 ? _regEx : _regEx.Left(_split);
		}

		/// <summary>
		/// 1. Set Value
		/// 2. Set Aliases
		/// 3. Set Disjunctions
		/// 4. Set Conjunctions
		/// </summary>
		/// <param name="searchCriteria"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		public virtual SearchQuery CreateCriteria(SearchQuery searchCriteria, string value)
		{
			return searchCriteria;
		}

		public abstract string DisplayText(string[] values);

		protected string RegExLong()
		{
			return _split == 0 ? _regEx : _regEx.Right(_regEx.Length - _split);
		}

		public string TokenName()
		{
			return _tokenName;
		}

		public string AutoCompleteText()
		{
			return _autocompleteValue;
		}

		public IList<SearchExample> Examples()
		{
			return (new List<SearchExample>
			        	{
			        		new SearchExample{search=_regEx+ ":", text = DisplayText(new [] {_value.ToString()}).Trim()}, 
			        		new SearchExample{search=RegExShort()+":", text = DisplayText(new [] {_value.ToString()}).Trim()}
			        	}).Distinct().ToList();
		}
	}
}