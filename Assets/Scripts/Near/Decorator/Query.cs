namespace Near.Decorator
{
    public abstract class Query
    {
        //public static string QueryStart = "tokens(where:{"; 
        public static string QueryEnd = "}) {\n    \n    id\n    title\n    media\n    reality\n    stats\n    nationality\n    birthday\n    number\n    hand\n    player_role\n    native_position\n    player_type\n    rarity\n    issued_at\n    tokenId\n    ownerId\n    perpetual_royalties\n    marketplace_data {\n      id\n    }\n  }\n\n}";
        public string QueryFilter { get; set; }
        
        public abstract string GetFinalQuery();
        
        public Query(string n)
        {
            this.QueryFilter += n;
        }
    }
}