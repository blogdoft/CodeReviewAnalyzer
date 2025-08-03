DO $$
BEGIN
    IF EXISTS (SELECT FROM pg_tables WHERE tablename = 'tenants') THEN
        IF NOT EXISTS (SELECT 1 FROM public.tenants WHERE shared_key = '42681c98-67b3-4db8-b670-8a413590ff63') THEN
            RAISE NOTICE 'Initializing table tenants';
            INSERT INTO public.tenants(
                    shared_key
                  , "name"
                  , active
            ) VALUES (
                    '42681c98-67b3-4db8-b670-8a413590ff63'::uuid,
                    'My Inc.'     
                  , true);            
        END IF;
    END IF;

    IF EXISTS (SELECT FROM pg_tables WHERE tablename = 'data_sources') THEN
        IF NOT EXISTS (select from public.data_sources limit 1) THEN
            RAISE NOTICE 'Initializing table data_sources';

            INSERT INTO public.data_sources (
                    tenants_id
                  , "name"
                  , active
                  , integration_type
            ) VALUES (
                    (select id from tenants t where t.shared_key = '42681c98-67b3-4db8-b670-8a413590ff63')
                  , 'My Inc. DevOps'
                  , true
                  , 'AzureDevOps'
            );

            IF EXISTS (
                            SELECT 
                            FROM data_sources 
                            LIMIT 1
                      ) THEN
                INSERT INTO public.azure_devops(
                        id
                      , devops_url
                      , pat
                      , projects
                      , areas)
                VALUES(
                        (
                            SELECT ds.id 
                            FROM data_sources ds
                            LIMIT 1
                        )
                      , 'https://dev.azure.com/$ORGANIZATION'
                      , '$PAT'
                      , '[$PROJECT_NAME]'
                      , '[$AREA_PATH]'
                );
            END IF;
        END IF;
    END IF;


    IF EXISTS (SELECT FROM pg_tables WHERE schemaname = 'public' AND tablename = 'teams') THEN
        IF NOT EXISTS (SELECT 1 FROM public.teams LIMIT 1) THEN
            RAISE NOTICE 'Creating teams';

            INSERT INTO public.teams(
                    tenants_id
                  , shared_key
                  , external_id
                  , "name"
            ) VALUES (
                    (select id from tenants t where t.shared_key = '42681c98-67b3-4db8-b670-8a413590ff63')
                  , '6faa24df-aa99-46c1-8eeb-9e60eda8a9ed'
                  , '6faa24df-aa99-46c1-8eeb-9e60eda8a9ed'
                  , 'Whole Team');

            INSERT INTO public.teams(
                    tenants_id
                  , shared_key
                  , external_id
                  , "name"
            ) VALUES (
                    (select id from tenants t where t.shared_key = '42681c98-67b3-4db8-b670-8a413590ff63')
                  , '4374cbea-5d66-4594-98c4-6140ac670f4b'
                  , '4374cbea-5d66-4594-98c4-6140ac670f4b'
                  , 'Maintenance Team');

            INSERT INTO public.teams(
                    tenants_id
                  , shared_key
                  , external_id
                  , "name"
            ) VALUES (
                    (select id from tenants t where t.shared_key = '42681c98-67b3-4db8-b670-8a413590ff63')
                  , '7ea3f23b-0bc5-4b0c-9617-732193e88150'
                  , '7ea3f23b-0bc5-4b0c-9617-732193e88150'
                  , 'Evolution Team');

            INSERT INTO public.teams(
                    tenants_id
                  , shared_key
                  , external_id
                  , "name"
            ) VALUES (
                    (select id from tenants t where t.shared_key = '42681c98-67b3-4db8-b670-8a413590ff63')
                  , '911f3f4b-de4f-42ce-ae38-7bc5b7670d20'
                  , '911f3f4b-de4f-42ce-ae38-7bc5b7670d20'
                  , 'Migration team');
        END IF;
    END IF;
END $$;
