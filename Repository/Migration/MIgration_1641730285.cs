using FluentMigrator;

namespace Repository.Migration
{
    [Migration(1641730285)]
    public class MIgration_1641730285 : FluentMigrator.Migration
    {
        public override void Up()
        {
            Create.Table("product")
                .WithColumn("id").AsInt64().PrimaryKey().Identity()
                .WithColumn("name").AsString().NotNullable()
                .WithColumn("price").AsInt64().Indexed().Nullable()
                .WithColumn("storename").AsString().NotNullable();
        }

        public override void Down()
        {
            Delete.Table("product");
        }
    }
}