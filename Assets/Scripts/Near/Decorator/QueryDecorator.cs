namespace Near.Decorator
{
    public class QueryDecorator : Query
    {
        public Query query;

        public QueryDecorator(string n, Query query) : base(n)
        {
            this.query = query;
        }


        public override string GetFinalQuery()
        {
            throw new System.NotImplementedException();
        }
    }
}