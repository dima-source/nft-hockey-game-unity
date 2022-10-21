using System;

namespace Near.Models.Tokens.Filters
{
    public class Pagination
    {
        public int skip { get; set; } = 0;
        public int first { get; set; } = 30;
        public string orderBy { get; set; }
        public Enum orderDirection { get; set; }
    }

    public enum OrderDirection
    {
        // Ascending
        asc,
        
        // Descending
        desc
    }
}