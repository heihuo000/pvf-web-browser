using System.Text.RegularExpressions;
using pvfUtility.Model;

namespace pvfUtility.Action.Search
{
    internal class SearchSetting
    {
        public FileCollectionData FileCollection;
        public bool IsStartMatch;
        public bool IsUseLikeSearchPath;
        public bool IsUseRegexInScriptSearch;
        public string Keyword;
        public Regex Regex;
        public SearchPresenter.SearchMethod SearchMethod;
        public SearchPresenter.SearchNormalUsing SearchNormalUsing;
        public string SearchPath;
        public SearchPresenter.SearchType SearchType;
    }
}