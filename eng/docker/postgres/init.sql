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
            ) VALUES(
                '42681c98-67b3-4db8-b670-8a413590ff63'::uuid, 
                '$ORGANIZATION', 
                '$PROJECT_NAME', 
                '$PAT'
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

    IF EXISTS (SELECT FROM pg_tables where schemaname = 'public' AND tablename = 'USERS') THEN
        RAISE NOTICE 'Set users state';	
        
        UPDATE public."USERS" SET "ACTIVE"=false WHERE "EXTERNAL_IDENTIFIER" = 'ff2e6db4-8b5d-61e6-988d-10f231cc9d49';
        UPDATE public."USERS" SET "ACTIVE"=false WHERE "EXTERNAL_IDENTIFIER" = 'f7e7c9aa-d42f-6dbb-9d5f-871f3a3d76a5';
        UPDATE public."USERS" SET "ACTIVE"=false WHERE "EXTERNAL_IDENTIFIER" = '57508f54-9810-6aa0-b648-a445a45a998b';
        UPDATE public."USERS" SET "ACTIVE"=false WHERE "EXTERNAL_IDENTIFIER" = '307fdf6c-3839-6f23-b1b1-e87d4de6f39c';
        UPDATE public."USERS" SET "ACTIVE"=false WHERE "EXTERNAL_IDENTIFIER" = '76ce5ba2-d485-6270-b3e9-fcaf9540c2fe';
        UPDATE public."USERS" SET "ACTIVE"=false WHERE "EXTERNAL_IDENTIFIER" = '8b0ef471-f8a3-6cf2-a882-d47d3c760620';
        UPDATE public."USERS" SET "ACTIVE"=true WHERE "EXTERNAL_IDENTIFIER" = 'dc4e32d9-7c54-6281-a054-0327969abf3c';
        UPDATE public."USERS" SET "ACTIVE"=false WHERE "EXTERNAL_IDENTIFIER" = 'cdcf7863-0aa5-6b51-a799-5638240f3f4b';
        UPDATE public."USERS" SET "ACTIVE"=false WHERE "EXTERNAL_IDENTIFIER" = '93688bea-85a8-6658-b620-e00b63496949';
        UPDATE public."USERS" SET "ACTIVE"=false WHERE "EXTERNAL_IDENTIFIER" = '40afcddd-5544-4f22-bad7-dedbda22fdf4';
        UPDATE public."USERS" SET "ACTIVE"=false WHERE "EXTERNAL_IDENTIFIER" = '3c04c350-ef63-64ee-ba49-99fc02beb701';
        UPDATE public."USERS" SET "ACTIVE"=false WHERE "EXTERNAL_IDENTIFIER" = 'fae270e9-2d2f-63af-b6da-7fa3351ec793';
        UPDATE public."USERS" SET "ACTIVE"=true WHERE "EXTERNAL_IDENTIFIER" = 'b9bc9da4-38ea-65e2-86f0-8194b996726b';
        UPDATE public."USERS" SET "ACTIVE"=false WHERE "EXTERNAL_IDENTIFIER" = 'd1ca6a7b-871c-650e-acfb-e07b77df3c7e';
        UPDATE public."USERS" SET "ACTIVE"=false WHERE "EXTERNAL_IDENTIFIER" = '4f768894-c3aa-4317-a9bc-8e1e0959d3f4';
        UPDATE public."USERS" SET "ACTIVE"=false WHERE "EXTERNAL_IDENTIFIER" = 'e028e482-cd14-697a-a828-00e471729ce5';
        UPDATE public."USERS" SET "ACTIVE"=true WHERE "EXTERNAL_IDENTIFIER" = '14d9837c-d763-6981-98b7-64bb6b75d05b';
        UPDATE public."USERS" SET "ACTIVE"=true WHERE "EXTERNAL_IDENTIFIER" = 'e955dc19-0860-6d67-a712-f6793b57d193';
        UPDATE public."USERS" SET "ACTIVE"=true WHERE "EXTERNAL_IDENTIFIER" = '8b96c1fc-5860-6e77-8ab3-8397ea19a05b';
        UPDATE public."USERS" SET "ACTIVE"=true WHERE "EXTERNAL_IDENTIFIER" = '83dbcdca-02d5-6d07-81ea-d81b1e448af8';
        UPDATE public."USERS" SET "ACTIVE"=false WHERE "EXTERNAL_IDENTIFIER" = '9b9f1cee-6487-6787-8951-c7405010e953';
        UPDATE public."USERS" SET "ACTIVE"=false WHERE "EXTERNAL_IDENTIFIER" = '816a8b0c-337b-6e1e-bc4d-045c3243a00c';
        UPDATE public."USERS" SET "ACTIVE"=false WHERE "EXTERNAL_IDENTIFIER" = '9e5693fd-8b04-69ec-832a-9fa909e4cdda';
        UPDATE public."USERS" SET "ACTIVE"=false WHERE "EXTERNAL_IDENTIFIER" = '2ea60914-fb45-6cea-af34-e0faa91245b0';
        UPDATE public."USERS" SET "ACTIVE"=false WHERE "EXTERNAL_IDENTIFIER" = '5acf45d3-f138-6950-9c38-aad792333713';
        UPDATE public."USERS" SET "ACTIVE"=false WHERE "EXTERNAL_IDENTIFIER" = '36db4570-7fb9-69dc-afbd-9ee1b7382745';
        UPDATE public."USERS" SET "ACTIVE"=false WHERE "EXTERNAL_IDENTIFIER" = 'c393bd83-69d5-6e36-a47d-00d91314b01c';
        UPDATE public."USERS" SET "ACTIVE"=false WHERE "EXTERNAL_IDENTIFIER" = '2ede2744-4510-40ba-b79b-3127859e1323';
        UPDATE public."USERS" SET "ACTIVE"=true WHERE "EXTERNAL_IDENTIFIER" = '9f09dd3b-b69c-60aa-b081-38995f178a9f';
        UPDATE public."USERS" SET "ACTIVE"=true WHERE "EXTERNAL_IDENTIFIER" = '4e1834a6-2680-67fe-b878-62a28a35c4f2';
        UPDATE public."USERS" SET "ACTIVE"=false WHERE "EXTERNAL_IDENTIFIER" = '6824977e-9b62-690a-99b5-542f2ac174d5';
        UPDATE public."USERS" SET "ACTIVE"=false WHERE "EXTERNAL_IDENTIFIER" = 'a1b930f9-b185-6ba8-9082-3675185cc4e8';
        UPDATE public."USERS" SET "ACTIVE"=false WHERE "EXTERNAL_IDENTIFIER" = '0e8f2ad6-a866-64c9-b630-636dfe512086';
        UPDATE public."USERS" SET "ACTIVE"=false WHERE "EXTERNAL_IDENTIFIER" = '21bca9ab-591b-461d-a832-aa4e64da856d';
        UPDATE public."USERS" SET "ACTIVE"=true WHERE "EXTERNAL_IDENTIFIER" = '45cddc82-99ae-620a-81a8-3e86077e00a3';
        UPDATE public."USERS" SET "ACTIVE"=false WHERE "EXTERNAL_IDENTIFIER" = '5fad3dd6-9f5a-4ff2-877d-b4f38de24ea9';
        UPDATE public."USERS" SET "ACTIVE"=false WHERE "EXTERNAL_IDENTIFIER" = '879fcccd-786d-6b15-b91d-1a5bcef7b771';
        UPDATE public."USERS" SET "ACTIVE"=false WHERE "EXTERNAL_IDENTIFIER" = '235fa6ce-a70e-4a19-8ad8-4cd1ce05f10e';
        UPDATE public."USERS" SET "ACTIVE"=false WHERE "EXTERNAL_IDENTIFIER" = '9e06cbfe-c407-655a-b041-bf229cc63281';
        UPDATE public."USERS" SET "ACTIVE"=false WHERE "EXTERNAL_IDENTIFIER" = '66587de5-43b7-699a-8fbc-1dd7de21679b';
        UPDATE public."USERS" SET "ACTIVE"=false WHERE "EXTERNAL_IDENTIFIER" = '4838ae18-449b-663c-9a2b-54a1cb33a7cd';
        UPDATE public."USERS" SET "ACTIVE"=false WHERE "EXTERNAL_IDENTIFIER" = '746e8fef-443b-65c1-9aa2-5a28d2278b65';
        UPDATE public."USERS" SET "ACTIVE"=false WHERE "EXTERNAL_IDENTIFIER" = 'f76304b6-d58d-603e-af55-bfafedc9fed6';
        UPDATE public."USERS" SET "ACTIVE"=false WHERE "EXTERNAL_IDENTIFIER" = '8faa854d-602b-63ec-967f-44098392cb31';
        UPDATE public."USERS" SET "ACTIVE"=false WHERE "EXTERNAL_IDENTIFIER" = 'e47e797e-c77e-6084-aba8-686b1db20913';
        UPDATE public."USERS" SET "ACTIVE"=false WHERE "EXTERNAL_IDENTIFIER" = '5fb7a348-89ea-649b-85a1-c74e1f625cb4';
    END IF;
END $$;
