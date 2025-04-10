DO $$
BEGIN
    IF EXISTS (SELECT FROM pg_tables WHERE tablename = 'CONFIGURATION') THEN
        IF NOT EXISTS (SELECT FROM public."CONFIGURATION" WHERE "CONFIGURATION_ID" = '42681c98-67b3-4db8-b670-8a413590ff63') THEN
            RAISE NOTICE 'Initializing table CONFIGURATION';
            INSERT INTO public."CONFIGURATION" (
                  "CONFIGURATION_ID"
                , "ORGANIZATION"
                , "PROJECT_NAME"
                , "PERSONAL_ACCESS_TOKEN"
                , "AREA_PATH"
            ) VALUES(
                '42681c98-67b3-4db8-b670-8a413590ff63'::uuid, 
                '$ORGANIZATION', 
                '$PROJECT_NAME', 
                '$PAT',
                '$AREA_PATH'
            );
        END IF;
    END IF;
    IF EXISTS (SELECT FROM pg_tables WHERE schemaname = 'public' AND tablename = 'DAY_OFF') THEN
        IF NOT EXISTS (SELECT FROM public."DAY_OFF" limit 1) THEN
        
            RAISE NOTICE 'Initializing table DAY_OFF';
            
            INSERT INTO public."DAY_OFF" ("EXTERNAL_ID", "DESCRIPTION", "DATE", "YEAR", "MONTH") VALUES
                    ('657a279a-7eb0-4223-a882-8f345b860c14'::uuid, 'Natal', '2024-12-25', 2024, 12),
                    ('ae871a45-e244-4b02-9e7a-cf9c04cdc9f6'::uuid, 'Ano Novo', '2025-01-01', 2025, 1),
                    ('470206ec-edec-442f-befd-40be143d24df'::uuid, 'Concessão CIA', '2024-12-27', 2024, 12),
                    ('2df4531a-5a3d-44dd-9ff0-a72d8dafa684'::uuid, 'Concessão CIA', '2024-12-28', 2024, 12),
                    ('1251aff2-69b9-477a-b02c-5fca595d98ba'::uuid, 'Concessão CIA', '2024-12-29', 2024, 12),
                    ('c834e349-942d-434b-b56e-f447d408e1c1'::uuid, 'Concessão CIA', '2024-12-30', 2024, 12),
                    ('4f565bc5-ae44-4693-9e52-2e248e07737f'::uuid, 'Concessão CIA', '2024-12-31', 2024, 12),
                    ('e59eebef-cb29-482c-8382-e55627d1d9d8'::uuid, 'Confraternização Universal', '2024-01-01', 2024, 1),
                    ('e64221f6-67ed-49da-ad2d-731831b1b687'::uuid, 'Carnaval', '2024-02-12', 2024, 2),
                    ('4447860f-734c-4b51-a3f2-b0d53c659d68'::uuid, 'Carnaval', '2024-02-13', 2024, 2),
                    ('dd9cfa99-1b2a-4277-9ec7-01a419d88209'::uuid, 'Quarta de cinzas', '2024-02-14', 2024, 2),
                    ('c81faf9a-c940-4144-862d-7dd365387046'::uuid, 'Paixão de Cristo', '2024-03-29', 2024, 3),
                    ('66732ad4-0461-449e-b5be-eb8517c25787'::uuid, 'Tiradentes', '2024-04-21', 2024, 4),
                    ('095abae0-5270-490a-b30f-a37977547ba0'::uuid, 'Dia do trabalho', '2024-05-01', 2024, 5),
                    ('cd38d23e-980b-483e-a417-8e7e2b4e528c'::uuid, 'Dia do trabalho - emenda', '2024-05-02', 2024, 5),
                    ('b887fc0e-8398-43b1-89b9-7e121471a5df'::uuid, 'Dia do trabalho - emenda', '2024-05-03', 2024, 5),
                    ('90f9e76b-f4c3-4e6b-ab8c-726eb0de40e2'::uuid, 'Corpus Christi', '2024-05-30', 2024, 5),
                    ('dde3c2bf-134c-4b6c-8b10-984522c8977a'::uuid, 'Corpus Christi - emenda', '2024-05-31', 2024, 5),
                    ('9f08a58e-4a89-44be-8d12-efc98e6962f1'::uuid, 'Independência do Brasil', '2024-09-07', 2024, 9),
                    ('438de62f-9212-4e7e-8739-9317ce08a2f2'::uuid, 'Padroeira do Brasil', '2024-10-12', 2024, 10),
                    ('d4d19200-d274-4d2c-8d1a-f0aa9254f76f'::uuid, 'Finados', '2024-11-02', 2024, 11),
                    ('5e9c1478-4201-459f-af5f-5b2075fdcbbd'::uuid, 'Proclamação da república', '2024-11-15', 2024, 11),
                    ('e89b46ab-d0b6-420b-b1eb-88014ec0abc0'::uuid, 'Carnaval', '2025-03-03', 2025, 3),
                    ('287b90d5-52a9-4eda-ab31-d3ba50b8c378'::uuid, 'Carnaval', '2025-03-04', 2025, 3),
                    ('87aa1e83-5777-43ad-b04c-f2ac33dab694'::uuid, 'Quarta-feira de cinzas', '2025-03-05', 2025, 3),
                    ('20c15c7f-e15e-454b-a193-4822eb822f1f'::uuid, 'Paixão de Cristo', '2025-04-18', 2025, 4),
                    ('c52a507d-454a-40f8-8ac0-be9c4a462d7f'::uuid, 'Tiradentes', '2025-04-21', 2025, 4),
                    ('fad835d7-4069-4a29-adc8-4b3b08b15bb0'::uuid, 'Dia do trabalho', '2025-05-01', 2025, 5),
                    ('791441ca-ea50-40b1-82cd-aee0ee207f4b'::uuid, 'Dia do trabalho - Emenda', '2025-05-02', 2025, 5),
                    ('ed1516c8-7224-4fbc-84b0-a448cf8e9c6a'::uuid, 'Corpus Christi', '2025-06-19', 2025, 6),
                    ('83879e63-8e20-4682-957d-635482cff5f1'::uuid, 'Corpus Christi - Emenda', '2025-06-20', 2025, 6),
                    ('c42a2514-7c29-4ac5-9606-905ac1168f6f'::uuid, 'Independência do Brasil', '2025-09-07', 2025, 9),
                    ('7b1f4f61-c6a8-4893-97ea-49d615bd63ce'::uuid, 'Padroeira do Brasil', '2025-10-12', 2025, 10),
                    ('33ca4e3b-579d-49fa-986b-abeb8b77da74'::uuid, 'Finados', '2025-11-02', 2025, 11),
                    ('5836feea-07d3-447a-8971-9fcfbfb4a66d'::uuid, 'Proclamação da República', '2025-11-15', 2025, 11),
                    ('83a110bc-fced-4945-8617-a79ef9c7a45d'::uuid, 'Dia Nacional de Zumbi e Consciência Negra', '2025-11-20', 2025, 11),
                    ('24663821-1fe0-4745-a95f-63376580ff35'::uuid, 'Dia Nacional de Zumbi e Consciência Negra - emenda', '2025-11-21', 2025, 11),
                    ('b840e2bb-eb69-4d87-9614-bd6205ddfee6'::uuid, 'Natal', '2025-12-25', 2025, 12),
                    ('cd7ab1b3-0fb9-44fa-b238-a7c39bd05c0d'::uuid, 'Natal - emenda', '2025-12-26', 2025, 12)
                    ON CONFLICT("EXTERNAL_ID") DO NOTHING;
        END IF;
    END IF;

    IF EXISTS (SELECT FROM pg_tables WHERE schemaname = 'public' AND tablename = 'TEAMS') THEN
        IF EXISTS (SELECT 1 FROM public."USERS" LIMIT 1) THEN
            RAISE NOTICE 'Creating teams';

            -- Configure TEAM
            INSERT INTO public."TEAMS" (external_id, "name", name_sh, description, active) VALUES ('fee52a41-f3ae-4ad6-a71c-b7981613f5bc', 'The Team', 'THE TEAM', 'All developers allocated into Squad 1 and Squad 2', true) ON CONFLICT DO NOTHING;
            INSERT INTO public."TEAMS" (external_id, "name", name_sh, description, active) VALUES ('04b7833d-a02c-4ff5-aea7-f3e2eeac9768', 'Squad 1', 'SQUAD 1', 'All developers allocated into Squad 1', true) ON CONFLICT DO NOTHING;
            INSERT INTO public."TEAMS" (external_id, "name", name_sh, description, active) VALUES ('b6809428-4890-4053-bed4-04174b6044ac', 'Squad 2', 'SQUAD 2', 'All developers allocated into Squad 2', true) ON CONFLICT DO NOTHING;

            -- Configure BMS full team
            INSERT INTO public."TEAM_USER" (team_id, user_id, role_in_team, joined_at_utc) VALUES ((select t.id from "TEAMS" t where t.external_id = 'fee52a41-f3ae-4ad6-a71c-b7981613f5bc'), (select u."ID" from "USERS" u where u."EXTERNAL_IDENTIFIER" = 'cee59558-2596-4b72-9e20-5739b9f22270'), 'Tech Lead', CURRENT_TIMESTAMP) ON CONFLICT DO NOTHING;
            INSERT INTO public."TEAM_USER" (team_id, user_id, role_in_team, joined_at_utc) VALUES ((select t.id from "TEAMS" t where t.external_id = 'fee52a41-f3ae-4ad6-a71c-b7981613f5bc'), (select u."ID" from "USERS" u where u."EXTERNAL_IDENTIFIER" = 'ce5c7717-f3af-4fdf-95aa-8418e348fe56'), 'Tech Lead', CURRENT_TIMESTAMP) ON CONFLICT DO NOTHING;
            INSERT INTO public."TEAM_USER" (team_id, user_id, role_in_team, joined_at_utc) VALUES ((select t.id from "TEAMS" t where t.external_id = 'fee52a41-f3ae-4ad6-a71c-b7981613f5bc'), (select u."ID" from "USERS" u where u."EXTERNAL_IDENTIFIER" = 'dc036e5f-6b4d-43c4-8f92-3a287179c544'), 'Develper', CURRENT_TIMESTAMP) ON CONFLICT DO NOTHING;
            INSERT INTO public."TEAM_USER" (team_id, user_id, role_in_team, joined_at_utc) VALUES ((select t.id from "TEAMS" t where t.external_id = 'fee52a41-f3ae-4ad6-a71c-b7981613f5bc'), (select u."ID" from "USERS" u where u."EXTERNAL_IDENTIFIER" = 'de149bc6-874d-4ea4-989e-282b2fa33c09'), 'Develper', CURRENT_TIMESTAMP) ON CONFLICT DO NOTHING;
            INSERT INTO public."TEAM_USER" (team_id, user_id, role_in_team, joined_at_utc) VALUES ((select t.id from "TEAMS" t where t.external_id = 'fee52a41-f3ae-4ad6-a71c-b7981613f5bc'), (select u."ID" from "USERS" u where u."EXTERNAL_IDENTIFIER" = 'd2cbd8ef-5242-4701-9c2f-417ed40ce395'), 'Develper', CURRENT_TIMESTAMP) ON CONFLICT DO NOTHING;
            INSERT INTO public."TEAM_USER" (team_id, user_id, role_in_team, joined_at_utc) VALUES ((select t.id from "TEAMS" t where t.external_id = 'fee52a41-f3ae-4ad6-a71c-b7981613f5bc'), (select u."ID" from "USERS" u where u."EXTERNAL_IDENTIFIER" = '43c750d8-c8a3-495a-80fc-1d33d395e375'), 'Develper', CURRENT_TIMESTAMP) ON CONFLICT DO NOTHING;
            INSERT INTO public."TEAM_USER" (team_id, user_id, role_in_team, joined_at_utc) VALUES ((select t.id from "TEAMS" t where t.external_id = 'fee52a41-f3ae-4ad6-a71c-b7981613f5bc'), (select u."ID" from "USERS" u where u."EXTERNAL_IDENTIFIER" = '731473db-bbab-4f33-9700-9c9031736c83'), 'Develper', CURRENT_TIMESTAMP) ON CONFLICT DO NOTHING;
            INSERT INTO public."TEAM_USER" (team_id, user_id, role_in_team, joined_at_utc) VALUES ((select t.id from "TEAMS" t where t.external_id = 'fee52a41-f3ae-4ad6-a71c-b7981613f5bc'), (select u."ID" from "USERS" u where u."EXTERNAL_IDENTIFIER" = '5b221bc1-4c7b-4d42-9f84-256834545c51'), 'Develper', CURRENT_TIMESTAMP) ON CONFLICT DO NOTHING;
            INSERT INTO public."TEAM_USER" (team_id, user_id, role_in_team, joined_at_utc) VALUES ((select t.id from "TEAMS" t where t.external_id = 'fee52a41-f3ae-4ad6-a71c-b7981613f5bc'), (select u."ID" from "USERS" u where u."EXTERNAL_IDENTIFIER" = '997b3c7e-3954-46fa-b2da-1a94be823ea0'), 'Develper', CURRENT_TIMESTAMP) ON CONFLICT DO NOTHING;

            -- Configure BMS Brewhouse
            INSERT INTO public."TEAM_USER" (team_id, user_id, role_in_team, joined_at_utc) VALUES ((select t.id from "TEAMS" t where t.external_id = '04b7833d-a02c-4ff5-aea7-f3e2eeac9768'), (select u."ID" from "USERS" u where u."EXTERNAL_IDENTIFIER" = 'cee59558-2596-4b72-9e20-5739b9f22270'), 'Tech Lead', CURRENT_TIMESTAMP) ON CONFLICT DO NOTHING;
            INSERT INTO public."TEAM_USER" (team_id, user_id, role_in_team, joined_at_utc) VALUES ((select t.id from "TEAMS" t where t.external_id = '04b7833d-a02c-4ff5-aea7-f3e2eeac9768'), (select u."ID" from "USERS" u where u."EXTERNAL_IDENTIFIER" = 'ce5c7717-f3af-4fdf-95aa-8418e348fe56'), 'Tech Lead', CURRENT_TIMESTAMP) ON CONFLICT DO NOTHING;
            INSERT INTO public."TEAM_USER" (team_id, user_id, role_in_team, joined_at_utc) VALUES ((select t.id from "TEAMS" t where t.external_id = '04b7833d-a02c-4ff5-aea7-f3e2eeac9768'), (select u."ID" from "USERS" u where u."EXTERNAL_IDENTIFIER" = 'dc036e5f-6b4d-43c4-8f92-3a287179c544'), 'Developer', CURRENT_TIMESTAMP) ON CONFLICT DO NOTHING;
            INSERT INTO public."TEAM_USER" (team_id, user_id, role_in_team, joined_at_utc) VALUES ((select t.id from "TEAMS" t where t.external_id = '04b7833d-a02c-4ff5-aea7-f3e2eeac9768'), (select u."ID" from "USERS" u where u."EXTERNAL_IDENTIFIER" = 'd2cbd8ef-5242-4701-9c2f-417ed40ce395'), 'Developer', CURRENT_TIMESTAMP) ON CONFLICT DO NOTHING;
            INSERT INTO public."TEAM_USER" (team_id, user_id, role_in_team, joined_at_utc) VALUES ((select t.id from "TEAMS" t where t.external_id = '04b7833d-a02c-4ff5-aea7-f3e2eeac9768'), (select u."ID" from "USERS" u where u."EXTERNAL_IDENTIFIER" = '731473db-bbab-4f33-9700-9c9031736c83'), 'Developer', CURRENT_TIMESTAMP) ON CONFLICT DO NOTHING;
            INSERT INTO public."TEAM_USER" (team_id, user_id, role_in_team, joined_at_utc) VALUES ((select t.id from "TEAMS" t where t.external_id = '04b7833d-a02c-4ff5-aea7-f3e2eeac9768'), (select u."ID" from "USERS" u where u."EXTERNAL_IDENTIFIER" = '997b3c7e-3954-46fa-b2da-1a94be823ea0'), 'Developer', CURRENT_TIMESTAMP) ON CONFLICT DO NOTHING;

            -- Configure BMS Filtration
            INSERT INTO public."TEAM_USER" (team_id, user_id, role_in_team, joined_at_utc) VALUES ((select t.id from "TEAMS" t where t.external_id = 'b6809428-4890-4053-bed4-04174b6044ac'), (select u."ID" from "USERS" u where u."EXTERNAL_IDENTIFIER" = 'de149bc6-874d-4ea4-989e-282b2fa33c09'), 'Tech Lead', CURRENT_TIMESTAMP) ON CONFLICT DO NOTHING;
            INSERT INTO public."TEAM_USER" (team_id, user_id, role_in_team, joined_at_utc) VALUES ((select t.id from "TEAMS" t where t.external_id = 'b6809428-4890-4053-bed4-04174b6044ac'), (select u."ID" from "USERS" u where u."EXTERNAL_IDENTIFIER" = '43c750d8-c8a3-495a-80fc-1d33d395e375'), 'Developer', CURRENT_TIMESTAMP) ON CONFLICT DO NOTHING;
            INSERT INTO public."TEAM_USER" (team_id, user_id, role_in_team, joined_at_utc) VALUES ((select t.id from "TEAMS" t where t.external_id = 'b6809428-4890-4053-bed4-04174b6044ac'), (select u."ID" from "USERS" u where u."EXTERNAL_IDENTIFIER" = '5b221bc1-4c7b-4d42-9f84-256834545c51'), 'Developer', CURRENT_TIMESTAMP) ON CONFLICT DO NOTHING;

            -- Configure Team Repo Full Brewhouse
            INSERT INTO public."TEAM_REPOSITORY" (team_id, repository_id) VALUES ((select t.id from "TEAMS" t where t.external_id = 'fee52a41-f3ae-4ad6-a71c-b7981613f5bc'), (select r.id from "REPOSITORIES" r where r.external_id = '3ea54a12-3421-4709-babf-03c8fcf584ec')) ON CONFLICT DO NOTHING;
            INSERT INTO public."TEAM_REPOSITORY" (team_id, repository_id) VALUES ((select t.id from "TEAMS" t where t.external_id = 'fee52a41-f3ae-4ad6-a71c-b7981613f5bc'), (select r.id from "REPOSITORIES" r where r.external_id = 'ddc07503-985c-4af5-a574-d1232ee5e90a')) ON CONFLICT DO NOTHING;
            INSERT INTO public."TEAM_REPOSITORY" (team_id, repository_id) VALUES ((select t.id from "TEAMS" t where t.external_id = 'fee52a41-f3ae-4ad6-a71c-b7981613f5bc'), (select r.id from "REPOSITORIES" r where r.external_id = '43c5f0e7-6336-494d-9fef-d0485b60460d')) ON CONFLICT DO NOTHING;
            INSERT INTO public."TEAM_REPOSITORY" (team_id, repository_id) VALUES ((select t.id from "TEAMS" t where t.external_id = 'fee52a41-f3ae-4ad6-a71c-b7981613f5bc'), (select r.id from "REPOSITORIES" r where r.external_id = '0320e563-00e4-429f-b684-ae435ab7fc09')) ON CONFLICT DO NOTHING;
            INSERT INTO public."TEAM_REPOSITORY" (team_id, repository_id) VALUES ((select t.id from "TEAMS" t where t.external_id = 'fee52a41-f3ae-4ad6-a71c-b7981613f5bc'), (select r.id from "REPOSITORIES" r where r.external_id = 'dc674c94-e884-46fa-ab1a-4c70631d78dc')) ON CONFLICT DO NOTHING;
            INSERT INTO public."TEAM_REPOSITORY" (team_id, repository_id) VALUES ((select t.id from "TEAMS" t where t.external_id = 'fee52a41-f3ae-4ad6-a71c-b7981613f5bc'), (select r.id from "REPOSITORIES" r where r.external_id = '7ea841f0-cc0e-449d-a602-8319ea88923f')) ON CONFLICT DO NOTHING;
            INSERT INTO public."TEAM_REPOSITORY" (team_id, repository_id) VALUES ((select t.id from "TEAMS" t where t.external_id = 'fee52a41-f3ae-4ad6-a71c-b7981613f5bc'), (select r.id from "REPOSITORIES" r where r.external_id = 'be02a6dc-2d96-45a0-a10b-ae8066b98172')) ON CONFLICT DO NOTHING;
            INSERT INTO public."TEAM_REPOSITORY" (team_id, repository_id) VALUES ((select t.id from "TEAMS" t where t.external_id = 'fee52a41-f3ae-4ad6-a71c-b7981613f5bc'), (select r.id from "REPOSITORIES" r where r.external_id = 'ef20d9e7-a5e1-4549-afab-9e6ec0bada79')) ON CONFLICT DO NOTHING;
            INSERT INTO public."TEAM_REPOSITORY" (team_id, repository_id) VALUES ((select t.id from "TEAMS" t where t.external_id = 'fee52a41-f3ae-4ad6-a71c-b7981613f5bc'), (select r.id from "REPOSITORIES" r where r.external_id = '1286c4e4-c1a0-48f2-8470-cabd670a1120')) ON CONFLICT DO NOTHING;
            INSERT INTO public."TEAM_REPOSITORY" (team_id, repository_id) VALUES ((select t.id from "TEAMS" t where t.external_id = 'fee52a41-f3ae-4ad6-a71c-b7981613f5bc'), (select r.id from "REPOSITORIES" r where r.external_id = '90464669-b939-49e7-881f-d3245e61bcec')) ON CONFLICT DO NOTHING;
            INSERT INTO public."TEAM_REPOSITORY" (team_id, repository_id) VALUES ((select t.id from "TEAMS" t where t.external_id = 'fee52a41-f3ae-4ad6-a71c-b7981613f5bc'), (select r.id from "REPOSITORIES" r where r.external_id = 'aaf200f3-2acb-4bf6-89f0-3fdb89f60862')) ON CONFLICT DO NOTHING;
            INSERT INTO public."TEAM_REPOSITORY" (team_id, repository_id) VALUES ((select t.id from "TEAMS" t where t.external_id = 'fee52a41-f3ae-4ad6-a71c-b7981613f5bc'), (select r.id from "REPOSITORIES" r where r.external_id = 'e92eeb9b-8197-4690-b23b-6d20a4ffc904')) ON CONFLICT DO NOTHING;
            INSERT INTO public."TEAM_REPOSITORY" (team_id, repository_id) VALUES ((select t.id from "TEAMS" t where t.external_id = 'fee52a41-f3ae-4ad6-a71c-b7981613f5bc'), (select r.id from "REPOSITORIES" r where r.external_id = '4540e5c4-34a9-4e51-bece-1734fecb7817')) ON CONFLICT DO NOTHING;
            INSERT INTO public."TEAM_REPOSITORY" (team_id, repository_id) VALUES ((select t.id from "TEAMS" t where t.external_id = 'fee52a41-f3ae-4ad6-a71c-b7981613f5bc'), (select r.id from "REPOSITORIES" r where r.external_id = '5a810775-1359-47b9-bfff-b75a212aecd1')) ON CONFLICT DO NOTHING;
            INSERT INTO public."TEAM_REPOSITORY" (team_id, repository_id) VALUES ((select t.id from "TEAMS" t where t.external_id = 'fee52a41-f3ae-4ad6-a71c-b7981613f5bc'), (select r.id from "REPOSITORIES" r where r.external_id = '0b3ddb66-8085-430f-8c93-e71a3563d12a')) ON CONFLICT DO NOTHING;
            INSERT INTO public."TEAM_REPOSITORY" (team_id, repository_id) VALUES ((select t.id from "TEAMS" t where t.external_id = 'fee52a41-f3ae-4ad6-a71c-b7981613f5bc'), (select r.id from "REPOSITORIES" r where r.external_id = 'e0c09ab6-4cc7-45b6-b7ce-7d96d49f2a53')) ON CONFLICT DO NOTHING;
            INSERT INTO public."TEAM_REPOSITORY" (team_id, repository_id) VALUES ((select t.id from "TEAMS" t where t.external_id = 'fee52a41-f3ae-4ad6-a71c-b7981613f5bc'), (select r.id from "REPOSITORIES" r where r.external_id = 'f511c43f-ed81-4935-a7a1-5a58c3678cc9')) ON CONFLICT DO NOTHING;
            INSERT INTO public."TEAM_REPOSITORY" (team_id, repository_id) VALUES ((select t.id from "TEAMS" t where t.external_id = 'fee52a41-f3ae-4ad6-a71c-b7981613f5bc'), (select r.id from "REPOSITORIES" r where r.external_id = '5679156b-b2fd-48d5-ba7b-52249389666f')) ON CONFLICT DO NOTHING;
            INSERT INTO public."TEAM_REPOSITORY" (team_id, repository_id) VALUES ((select t.id from "TEAMS" t where t.external_id = 'fee52a41-f3ae-4ad6-a71c-b7981613f5bc'), (select r.id from "REPOSITORIES" r where r.external_id = '0c81d5d5-53eb-4ced-a15a-a11e7c3d059f')) ON CONFLICT DO NOTHING;
            INSERT INTO public."TEAM_REPOSITORY" (team_id, repository_id) VALUES ((select t.id from "TEAMS" t where t.external_id = 'fee52a41-f3ae-4ad6-a71c-b7981613f5bc'), (select r.id from "REPOSITORIES" r where r.external_id = '806ea31f-d066-40a7-9812-8ead9f388203')) ON CONFLICT DO NOTHING;
            INSERT INTO public."TEAM_REPOSITORY" (team_id, repository_id) VALUES ((select t.id from "TEAMS" t where t.external_id = 'fee52a41-f3ae-4ad6-a71c-b7981613f5bc'), (select r.id from "REPOSITORIES" r where r.external_id = 'c06e0449-f5ee-40f5-98cd-04a40864712f')) ON CONFLICT DO NOTHING;
            INSERT INTO public."TEAM_REPOSITORY" (team_id, repository_id) VALUES ((select t.id from "TEAMS" t where t.external_id = 'fee52a41-f3ae-4ad6-a71c-b7981613f5bc'), (select r.id from "REPOSITORIES" r where r.external_id = '47c97a07-8877-42ec-923f-c2a3d7f37bab')) ON CONFLICT DO NOTHING;
            INSERT INTO public."TEAM_REPOSITORY" (team_id, repository_id) VALUES ((select t.id from "TEAMS" t where t.external_id = 'fee52a41-f3ae-4ad6-a71c-b7981613f5bc'), (select r.id from "REPOSITORIES" r where r.external_id = '3b8a4715-d294-425b-97d1-107727e07aef')) ON CONFLICT DO NOTHING;
            INSERT INTO public."TEAM_REPOSITORY" (team_id, repository_id) VALUES ((select t.id from "TEAMS" t where t.external_id = 'fee52a41-f3ae-4ad6-a71c-b7981613f5bc'), (select r.id from "REPOSITORIES" r where r.external_id = '5c13de39-3f34-4075-92d4-012a3ddda62a')) ON CONFLICT DO NOTHING;
            INSERT INTO public."TEAM_REPOSITORY" (team_id, repository_id) VALUES ((select t.id from "TEAMS" t where t.external_id = 'fee52a41-f3ae-4ad6-a71c-b7981613f5bc'), (select r.id from "REPOSITORIES" r where r.external_id = '9444c5b8-3334-4c11-b988-590e778c6ab2')) ON CONFLICT DO NOTHING;
            INSERT INTO public."TEAM_REPOSITORY" (team_id, repository_id) VALUES ((select t.id from "TEAMS" t where t.external_id = 'fee52a41-f3ae-4ad6-a71c-b7981613f5bc'), (select r.id from "REPOSITORIES" r where r.external_id = '1368f23a-7c5f-47a5-b0bf-31b23b17f5ae')) ON CONFLICT DO NOTHING;

            -- Configure Team repo Brewhouse
            INSERT INTO public."TEAM_REPOSITORY" (team_id, repository_id) VALUES ((select t.id from "TEAMS" t where t.external_id = '04b7833d-a02c-4ff5-aea7-f3e2eeac9768'), (select r.id from "REPOSITORIES" r where r.external_id = '3ea54a12-3421-4709-babf-03c8fcf584ec')) ON CONFLICT DO NOTHING;
            INSERT INTO public."TEAM_REPOSITORY" (team_id, repository_id) VALUES ((select t.id from "TEAMS" t where t.external_id = '04b7833d-a02c-4ff5-aea7-f3e2eeac9768'), (select r.id from "REPOSITORIES" r where r.external_id = 'ddc07503-985c-4af5-a574-d1232ee5e90a')) ON CONFLICT DO NOTHING;
            INSERT INTO public."TEAM_REPOSITORY" (team_id, repository_id) VALUES ((select t.id from "TEAMS" t where t.external_id = '04b7833d-a02c-4ff5-aea7-f3e2eeac9768'), (select r.id from "REPOSITORIES" r where r.external_id = '43c5f0e7-6336-494d-9fef-d0485b60460d')) ON CONFLICT DO NOTHING;
            INSERT INTO public."TEAM_REPOSITORY" (team_id, repository_id) VALUES ((select t.id from "TEAMS" t where t.external_id = '04b7833d-a02c-4ff5-aea7-f3e2eeac9768'), (select r.id from "REPOSITORIES" r where r.external_id = '0320e563-00e4-429f-b684-ae435ab7fc09')) ON CONFLICT DO NOTHING;
            INSERT INTO public."TEAM_REPOSITORY" (team_id, repository_id) VALUES ((select t.id from "TEAMS" t where t.external_id = '04b7833d-a02c-4ff5-aea7-f3e2eeac9768'), (select r.id from "REPOSITORIES" r where r.external_id = 'dc674c94-e884-46fa-ab1a-4c70631d78dc')) ON CONFLICT DO NOTHING;
            INSERT INTO public."TEAM_REPOSITORY" (team_id, repository_id) VALUES ((select t.id from "TEAMS" t where t.external_id = '04b7833d-a02c-4ff5-aea7-f3e2eeac9768'), (select r.id from "REPOSITORIES" r where r.external_id = '7ea841f0-cc0e-449d-a602-8319ea88923f')) ON CONFLICT DO NOTHING;
            INSERT INTO public."TEAM_REPOSITORY" (team_id, repository_id) VALUES ((select t.id from "TEAMS" t where t.external_id = '04b7833d-a02c-4ff5-aea7-f3e2eeac9768'), (select r.id from "REPOSITORIES" r where r.external_id = 'be02a6dc-2d96-45a0-a10b-ae8066b98172')) ON CONFLICT DO NOTHING;
            INSERT INTO public."TEAM_REPOSITORY" (team_id, repository_id) VALUES ((select t.id from "TEAMS" t where t.external_id = '04b7833d-a02c-4ff5-aea7-f3e2eeac9768'), (select r.id from "REPOSITORIES" r where r.external_id = 'ef20d9e7-a5e1-4549-afab-9e6ec0bada79')) ON CONFLICT DO NOTHING;
            INSERT INTO public."TEAM_REPOSITORY" (team_id, repository_id) VALUES ((select t.id from "TEAMS" t where t.external_id = '04b7833d-a02c-4ff5-aea7-f3e2eeac9768'), (select r.id from "REPOSITORIES" r where r.external_id = '90464669-b939-49e7-881f-d3245e61bcec')) ON CONFLICT DO NOTHING;
            INSERT INTO public."TEAM_REPOSITORY" (team_id, repository_id) VALUES ((select t.id from "TEAMS" t where t.external_id = '04b7833d-a02c-4ff5-aea7-f3e2eeac9768'), (select r.id from "REPOSITORIES" r where r.external_id = 'aaf200f3-2acb-4bf6-89f0-3fdb89f60862')) ON CONFLICT DO NOTHING;
            INSERT INTO public."TEAM_REPOSITORY" (team_id, repository_id) VALUES ((select t.id from "TEAMS" t where t.external_id = '04b7833d-a02c-4ff5-aea7-f3e2eeac9768'), (select r.id from "REPOSITORIES" r where r.external_id = 'e92eeb9b-8197-4690-b23b-6d20a4ffc904')) ON CONFLICT DO NOTHING;
            INSERT INTO public."TEAM_REPOSITORY" (team_id, repository_id) VALUES ((select t.id from "TEAMS" t where t.external_id = '04b7833d-a02c-4ff5-aea7-f3e2eeac9768'), (select r.id from "REPOSITORIES" r where r.external_id = '4540e5c4-34a9-4e51-bece-1734fecb7817')) ON CONFLICT DO NOTHING;
            INSERT INTO public."TEAM_REPOSITORY" (team_id, repository_id) VALUES ((select t.id from "TEAMS" t where t.external_id = '04b7833d-a02c-4ff5-aea7-f3e2eeac9768'), (select r.id from "REPOSITORIES" r where r.external_id = '5a810775-1359-47b9-bfff-b75a212aecd1')) ON CONFLICT DO NOTHING;
            INSERT INTO public."TEAM_REPOSITORY" (team_id, repository_id) VALUES ((select t.id from "TEAMS" t where t.external_id = '04b7833d-a02c-4ff5-aea7-f3e2eeac9768'), (select r.id from "REPOSITORIES" r where r.external_id = '0b3ddb66-8085-430f-8c93-e71a3563d12a')) ON CONFLICT DO NOTHING;
            INSERT INTO public."TEAM_REPOSITORY" (team_id, repository_id) VALUES ((select t.id from "TEAMS" t where t.external_id = '04b7833d-a02c-4ff5-aea7-f3e2eeac9768'), (select r.id from "REPOSITORIES" r where r.external_id = 'e0c09ab6-4cc7-45b6-b7ce-7d96d49f2a53')) ON CONFLICT DO NOTHING;
            INSERT INTO public."TEAM_REPOSITORY" (team_id, repository_id) VALUES ((select t.id from "TEAMS" t where t.external_id = '04b7833d-a02c-4ff5-aea7-f3e2eeac9768'), (select r.id from "REPOSITORIES" r where r.external_id = 'f511c43f-ed81-4935-a7a1-5a58c3678cc9')) ON CONFLICT DO NOTHING;
            INSERT INTO public."TEAM_REPOSITORY" (team_id, repository_id) VALUES ((select t.id from "TEAMS" t where t.external_id = '04b7833d-a02c-4ff5-aea7-f3e2eeac9768'), (select r.id from "REPOSITORIES" r where r.external_id = '5679156b-b2fd-48d5-ba7b-52249389666f')) ON CONFLICT DO NOTHING;
            INSERT INTO public."TEAM_REPOSITORY" (team_id, repository_id) VALUES ((select t.id from "TEAMS" t where t.external_id = '04b7833d-a02c-4ff5-aea7-f3e2eeac9768'), (select r.id from "REPOSITORIES" r where r.external_id = '0c81d5d5-53eb-4ced-a15a-a11e7c3d059f')) ON CONFLICT DO NOTHING;
            INSERT INTO public."TEAM_REPOSITORY" (team_id, repository_id) VALUES ((select t.id from "TEAMS" t where t.external_id = '04b7833d-a02c-4ff5-aea7-f3e2eeac9768'), (select r.id from "REPOSITORIES" r where r.external_id = '806ea31f-d066-40a7-9812-8ead9f388203')) ON CONFLICT DO NOTHING;
            INSERT INTO public."TEAM_REPOSITORY" (team_id, repository_id) VALUES ((select t.id from "TEAMS" t where t.external_id = '04b7833d-a02c-4ff5-aea7-f3e2eeac9768'), (select r.id from "REPOSITORIES" r where r.external_id = 'c06e0449-f5ee-40f5-98cd-04a40864712f')) ON CONFLICT DO NOTHING;
            INSERT INTO public."TEAM_REPOSITORY" (team_id, repository_id) VALUES ((select t.id from "TEAMS" t where t.external_id = '04b7833d-a02c-4ff5-aea7-f3e2eeac9768'), (select r.id from "REPOSITORIES" r where r.external_id = '47c97a07-8877-42ec-923f-c2a3d7f37bab')) ON CONFLICT DO NOTHING;
            INSERT INTO public."TEAM_REPOSITORY" (team_id, repository_id) VALUES ((select t.id from "TEAMS" t where t.external_id = '04b7833d-a02c-4ff5-aea7-f3e2eeac9768'), (select r.id from "REPOSITORIES" r where r.external_id = '3b8a4715-d294-425b-97d1-107727e07aef')) ON CONFLICT DO NOTHING;
            INSERT INTO public."TEAM_REPOSITORY" (team_id, repository_id) VALUES ((select t.id from "TEAMS" t where t.external_id = '04b7833d-a02c-4ff5-aea7-f3e2eeac9768'), (select r.id from "REPOSITORIES" r where r.external_id = '5c13de39-3f34-4075-92d4-012a3ddda62a')) ON CONFLICT DO NOTHING;
            INSERT INTO public."TEAM_REPOSITORY" (team_id, repository_id) VALUES ((select t.id from "TEAMS" t where t.external_id = '04b7833d-a02c-4ff5-aea7-f3e2eeac9768'), (select r.id from "REPOSITORIES" r where r.external_id = '9444c5b8-3334-4c11-b988-590e778c6ab2')) ON CONFLICT DO NOTHING;
            INSERT INTO public."TEAM_REPOSITORY" (team_id, repository_id) VALUES ((select t.id from "TEAMS" t where t.external_id = '04b7833d-a02c-4ff5-aea7-f3e2eeac9768'), (select r.id from "REPOSITORIES" r where r.external_id = '1368f23a-7c5f-47a5-b0bf-31b23b17f5ae')) ON CONFLICT DO NOTHING;

            -- Configure Tem repo Filtration
            INSERT INTO public."TEAM_REPOSITORY" (team_id, repository_id) VALUES ((select t.id from "TEAMS" t where t.external_id = 'b6809428-4890-4053-bed4-04174b6044ac'), (select r.id from "REPOSITORIES" r where r.external_id = 'ddc07503-985c-4af5-a574-d1232ee5e90a')) ON CONFLICT DO NOTHING;
            INSERT INTO public."TEAM_REPOSITORY" (team_id, repository_id) VALUES ((select t.id from "TEAMS" t where t.external_id = 'b6809428-4890-4053-bed4-04174b6044ac'), (select r.id from "REPOSITORIES" r where r.external_id = '0320e563-00e4-429f-b684-ae435ab7fc09')) ON CONFLICT DO NOTHING;
            INSERT INTO public."TEAM_REPOSITORY" (team_id, repository_id) VALUES ((select t.id from "TEAMS" t where t.external_id = 'b6809428-4890-4053-bed4-04174b6044ac'), (select r.id from "REPOSITORIES" r where r.external_id = '7ea841f0-cc0e-449d-a602-8319ea88923f')) ON CONFLICT DO NOTHING;
            INSERT INTO public."TEAM_REPOSITORY" (team_id, repository_id) VALUES ((select t.id from "TEAMS" t where t.external_id = 'b6809428-4890-4053-bed4-04174b6044ac'), (select r.id from "REPOSITORIES" r where r.external_id = '1286c4e4-c1a0-48f2-8470-cabd670a1120')) ON CONFLICT DO NOTHING;
            INSERT INTO public."TEAM_REPOSITORY" (team_id, repository_id) VALUES ((select t.id from "TEAMS" t where t.external_id = 'b6809428-4890-4053-bed4-04174b6044ac'), (select r.id from "REPOSITORIES" r where r.external_id = '5679156b-b2fd-48d5-ba7b-52249389666f')) ON CONFLICT DO NOTHING;
            INSERT INTO public."TEAM_REPOSITORY" (team_id, repository_id) VALUES ((select t.id from "TEAMS" t where t.external_id = 'b6809428-4890-4053-bed4-04174b6044ac'), (select r.id from "REPOSITORIES" r where r.external_id = '806ea31f-d066-40a7-9812-8ead9f388203')) ON CONFLICT DO NOTHING;
            INSERT INTO public."TEAM_REPOSITORY" (team_id, repository_id) VALUES ((select t.id from "TEAMS" t where t.external_id = 'b6809428-4890-4053-bed4-04174b6044ac'), (select r.id from "REPOSITORIES" r where r.external_id = '47c97a07-8877-42ec-923f-c2a3d7f37bab')) ON CONFLICT DO NOTHING;
            INSERT INTO public."TEAM_REPOSITORY" (team_id, repository_id) VALUES ((select t.id from "TEAMS" t where t.external_id = 'b6809428-4890-4053-bed4-04174b6044ac'), (select r.id from "REPOSITORIES" r where r.external_id = '3b8a4715-d294-425b-97d1-107727e07aef')) ON CONFLICT DO NOTHING;
            INSERT INTO public."TEAM_REPOSITORY" (team_id, repository_id) VALUES ((select t.id from "TEAMS" t where t.external_id = 'b6809428-4890-4053-bed4-04174b6044ac'), (select r.id from "REPOSITORIES" r where r.external_id = '5c13de39-3f34-4075-92d4-012a3ddda62a')) ON CONFLICT DO NOTHING;
            INSERT INTO public."TEAM_REPOSITORY" (team_id, repository_id) VALUES ((select t.id from "TEAMS" t where t.external_id = 'b6809428-4890-4053-bed4-04174b6044ac'), (select r.id from "REPOSITORIES" r where r.external_id = '9444c5b8-3334-4c11-b988-590e778c6ab2')) ON CONFLICT DO NOTHING;
            INSERT INTO public."TEAM_REPOSITORY" (team_id, repository_id) VALUES ((select t.id from "TEAMS" t where t.external_id = 'b6809428-4890-4053-bed4-04174b6044ac'), (select r.id from "REPOSITORIES" r where r.external_id = '1368f23a-7c5f-47a5-b0bf-31b23b17f5ae')) ON CONFLICT DO NOTHING;
        END IF;

    END IF;
END $$;
