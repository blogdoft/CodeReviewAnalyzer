namespace CodeReviewAnalyzer.Database.Repositories;

internal static class TenantStmt
{
    public const string UpdateTenant =
        """
            UPDATE public."tenants"
            SET "name"=@name, active=@Active
            WHERE "shared_key"=@Id;
        
        """;

    public const string DeleteAllDataSources =
        """
            delete from "azure_devops" az
            where exists (
                            select 1 
                            from "data_sources" ds 
                                join "tenants" t on t.id = ds.tenants_id
                            where ds.id = az.id 
                            and t."shared_key" = @id
                        );
            delete from "data_sources" ds
            where exists (
                            select 1
                            from "tenants" t
                            where t."shared_key" = @id
                            and ds.tenants_id = t.id
                        );
        """;

    public const string DataSourceInsert =
        """
            INSERT INTO public."data_sources"
            (tenants_id, "name", active, "integration_type")
            VALUES((select id from "tenants" where "shared_key" = @sharedKey), @name, @active, @IntegrationType)
            returning id;

        """;

    public const string InsertAzureDevOps =
        """
            INSERT INTO public."azure_devops"
            (id, "devops_url", pat, projects, areas)
            VALUES(@id, @devopsUrl, @pat, @projects, @areas);

        """;
}
