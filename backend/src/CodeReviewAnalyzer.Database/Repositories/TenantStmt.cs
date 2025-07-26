namespace CodeReviewAnalyzer.Database.Repositories;

internal static class TenantStmt
{
    public const string UpdateTenant =
        """
            UPDATE public."Tenants"
            SET "name"=@name, active=@Active
            WHERE "sharedKey"=@Id;
        
        """;

    public const string DeleteAllDataSources =
        """
            delete from "AzureDevops" az
            where exists (
                            select 1 
                            from "DataSource" ds 
                                join "Tenants" t on t.id = ds.tenant_id
                            where ds.id = az.id 
                            and t."sharedKey" = @id
                        );
            delete from "DataSource" ds
            where exists (
                            select 1
                            from "Tenants" t
                            where t."sharedKey" = @id
                            and ds.tenant_id = t.id
                        );
        """;

    public const string DataSourceInsert =
        """
            INSERT INTO public."DataSource"
            (tenant_id, "name", active, "integrationType")
            VALUES((select id from "Tenants" where "sharedKey" = @sharedKey), @name, @active, @IntegrationType)
            returning id;

        """;

    public const string InsertAzureDevOps =
        """
            INSERT INTO public."AzureDevops"
            (id, "devopsUrl", pat, projects, areas)
            VALUES(@id, @devopsUrl, @pat, @projects, @areas);

        """;
}
