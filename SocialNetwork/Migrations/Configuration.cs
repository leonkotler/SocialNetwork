namespace SocialNetwork.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<SocialNetwork.DAL.NetworkContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
            ContextKey = "SocialNetwork.DAL.NetworkContext";
        }

        protected override void Seed(SocialNetwork.DAL.NetworkContext context)
        {
            
        }
    }
}
