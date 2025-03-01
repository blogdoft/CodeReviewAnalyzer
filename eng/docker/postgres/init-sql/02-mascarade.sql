DO $$
BEGIN
    IF EXISTS (SELECT 1 FROM public."USERS" LIMIT 1) THEN
        RAISE NOTICE 'Oculting names';  
        -- 1
        UPDATE public."USERS"
        SET "NAME" = 'Maria Oliveira'
        WHERE "EXTERNAL_IDENTIFIER" = 'cdcf7863-0aa5-6b51-a799-5638240f3f4b';

        -- 2
        UPDATE public."USERS"
        SET "NAME" = 'João Silva'
        WHERE "EXTERNAL_IDENTIFIER" = '93688bea-85a8-6658-b620-e00b63496949';

        -- 3
        UPDATE public."USERS"
        SET "NAME" = 'Ana Santos'
        WHERE "EXTERNAL_IDENTIFIER" = '40afcddd-5544-4f22-bad7-dedbda22fdf4';

        -- 4
        UPDATE public."USERS"
        SET "NAME" = 'Pedro Santos'
        WHERE "EXTERNAL_IDENTIFIER" = '3c04c350-ef63-64ee-ba49-99fc02beb701';

        -- 5
        UPDATE public."USERS"
        SET "NAME" = 'Juliana Ferreira'
        WHERE "EXTERNAL_IDENTIFIER" = 'fae270e9-2d2f-63af-b6da-7fa3351ec793';

        -- 6
        UPDATE public."USERS"
        SET "NAME" = 'Lucas Oliveira'
        WHERE "EXTERNAL_IDENTIFIER" = 'cee59558-2596-4b72-9e20-5739b9f22270';

        -- 7
        UPDATE public."USERS"
        SET "NAME" = 'Fernanda Costa'
        WHERE "EXTERNAL_IDENTIFIER" = 'd1ca6a7b-871c-650e-acfb-e07b77df3c7e';

        -- 8
        UPDATE public."USERS"
        SET "NAME" = 'Matheus Costa'
        WHERE "EXTERNAL_IDENTIFIER" = '4f768894-c3aa-4317-a9bc-8e1e0959d3f4';

        -- 9
        UPDATE public."USERS"
        SET "NAME" = 'Camila Rodrigues'
        WHERE "EXTERNAL_IDENTIFIER" = 'e028e482-cd14-697a-a828-00e471729ce5';

        -- 10
        UPDATE public."USERS"
        SET "NAME" = 'Bruno Rodrigues'
        WHERE "EXTERNAL_IDENTIFIER" = 'ce5c7717-f3af-4fdf-95aa-8418e348fe56';

        -- 11
        UPDATE public."USERS"
        SET "NAME" = 'Patricia Almeida'
        WHERE "EXTERNAL_IDENTIFIER" = 'dc036e5f-6b4d-43c4-8f92-3a287179c544';

        -- 12
        UPDATE public."USERS"
        SET "NAME" = 'Gabriel Almeida'
        WHERE "EXTERNAL_IDENTIFIER" = 'de149bc6-874d-4ea4-989e-282b2fa33c09';

        -- 13
        UPDATE public."USERS"
        SET "NAME" = 'Leticia Lima'
        WHERE "EXTERNAL_IDENTIFIER" = 'd2cbd8ef-5242-4701-9c2f-417ed40ce395';

        -- 14
        UPDATE public."USERS"
        SET "NAME" = 'Felipe Lima'
        WHERE "EXTERNAL_IDENTIFIER" = '9b9f1cee-6487-6787-8951-c7405010e953';

        -- 15
        UPDATE public."USERS"
        SET "NAME" = 'Daniela Souza'
        WHERE "EXTERNAL_IDENTIFIER" = '6a102c01-3320-610b-924b-e07bc5be2a09';

        -- 16
        UPDATE public."USERS"
        SET "NAME" = 'Ricardo Souza'
        WHERE "EXTERNAL_IDENTIFIER" = '816a8b0c-337b-6e1e-bc4d-045c3243a00c';

        -- 17
        UPDATE public."USERS"
        SET "NAME" = 'Adriana Martins'
        WHERE "EXTERNAL_IDENTIFIER" = '9e5693fd-8b04-69ec-832a-9fa909e4cdda';

        -- 18
        UPDATE public."USERS"
        SET "NAME" = 'André Martins'
        WHERE "EXTERNAL_IDENTIFIER" = '2ea60914-fb45-6cea-af34-e0faa91245b0';

        -- 19
        UPDATE public."USERS"
        SET "NAME" = 'Beatriz Pereira'
        WHERE "EXTERNAL_IDENTIFIER" = 'c9be12cb-9f08-6181-95e4-eff471d0b74b';

        -- 20
        UPDATE public."USERS"
        SET "NAME" = 'Carlos Pereira'
        WHERE "EXTERNAL_IDENTIFIER" = 'ff2e6db4-8b5d-61e6-988d-10f231cc9d49';

        -- 21
        UPDATE public."USERS"
        SET "NAME" = 'Renata Gomes'
        WHERE "EXTERNAL_IDENTIFIER" = '5acf45d3-f138-6950-9c38-aad792333713';

        -- 22
        UPDATE public."USERS"
        SET "NAME" = 'Diego Gomes'
        WHERE "EXTERNAL_IDENTIFIER" = 'f7e7c9aa-d42f-6dbb-9d5f-871f3a3d76a5';

        -- 23
        UPDATE public."USERS"
        SET "NAME" = 'Tatiana Silva'
        WHERE "EXTERNAL_IDENTIFIER" = '57508f54-9810-6aa0-b648-a445a45a998b';

        -- 24
        UPDATE public."USERS"
        SET "NAME" = 'Rafael Silva'
        WHERE "EXTERNAL_IDENTIFIER" = '307fdf6c-3839-6f23-b1b1-e87d4de6f39c';

        -- 25
        UPDATE public."USERS"
        SET "NAME" = 'Carla Mendes'
        WHERE "EXTERNAL_IDENTIFIER" = '36db4570-7fb9-69dc-afbd-9ee1b7382745';

        -- 26
        UPDATE public."USERS"
        SET "NAME" = 'Vinícius Mendes'
        WHERE "EXTERNAL_IDENTIFIER" = 'c393bd83-69d5-6e36-a47d-00d91314b01c';

        -- 27
        UPDATE public."USERS"
        SET "NAME" = 'Vanessa Rocha'
        WHERE "EXTERNAL_IDENTIFIER" = '76ce5ba2-d485-6270-b3e9-fcaf9540c2fe';

        -- 28
        UPDATE public."USERS"
        SET "NAME" = 'Alexandre Rocha'
        WHERE "EXTERNAL_IDENTIFIER" = '8b0ef471-f8a3-6cf2-a882-d47d3c760620';

        -- 29
        UPDATE public."USERS"
        SET "NAME" = 'Aline Dias'
        WHERE "EXTERNAL_IDENTIFIER" = '43c750d8-c8a3-495a-80fc-1d33d395e375';

        -- 30
        UPDATE public."USERS"
        SET "NAME" = 'Leonardo Dias'
        WHERE "EXTERNAL_IDENTIFIER" = '2ede2744-4510-40ba-b79b-3127859e1323';

        -- 31
        UPDATE public."USERS"
        SET "NAME" = 'Mariana Barbosa'
        WHERE "EXTERNAL_IDENTIFIER" = '731473db-bbab-4f33-9700-9c9031736c83';

        -- 32
        UPDATE public."USERS"
        SET "NAME" = 'Thiago Barbosa'
        WHERE "EXTERNAL_IDENTIFIER" = '5b221bc1-4c7b-4d42-9f84-256834545c51';

        -- 33
        UPDATE public."USERS"
        SET "NAME" = 'Priscila Ribeiro'
        WHERE "EXTERNAL_IDENTIFIER" = '6824977e-9b62-690a-99b5-542f2ac174d5';

        -- 34
        UPDATE public."USERS"
        SET "NAME" = 'Daniel Ribeiro'
        WHERE "EXTERNAL_IDENTIFIER" = 'bff8b6ee-1097-6403-8629-ec6eeeeb772a';

        -- 35
        UPDATE public."USERS"
        SET "NAME" = 'Paula Cardoso'
        WHERE "EXTERNAL_IDENTIFIER" = 'a1b930f9-b185-6ba8-9082-3675185cc4e8';

        -- 36
        UPDATE public."USERS"
        SET "NAME" = 'Gustavo Cardoso'
        WHERE "EXTERNAL_IDENTIFIER" = '0e8f2ad6-a866-64c9-b630-636dfe512086';

        -- 37
        UPDATE public."USERS"
        SET "NAME" = 'Simone Moreira'
        WHERE "EXTERNAL_IDENTIFIER" = '21bca9ab-591b-461d-a832-aa4e64da856d';

        -- 38
        UPDATE public."USERS"
        SET "NAME" = 'Rodrigo Moreira'
        WHERE "EXTERNAL_IDENTIFIER" = '997b3c7e-3954-46fa-b2da-1a94be823ea0';

        -- 39
        UPDATE public."USERS"
        SET "NAME" = 'Ingrid Araújo'
        WHERE "EXTERNAL_IDENTIFIER" = '5fad3dd6-9f5a-4ff2-877d-b4f38de24ea9';

        -- 40
        UPDATE public."USERS"
        SET "NAME" = 'Marcelo Araújo'
        WHERE "EXTERNAL_IDENTIFIER" = '879fcccd-786d-6b15-b91d-1a5bcef7b771';

        -- 41
        UPDATE public."USERS"
        SET "NAME" = 'Rafaela Teixeira'
        WHERE "EXTERNAL_IDENTIFIER" = '235fa6ce-a70e-4a19-8ad8-4cd1ce05f10e';

        -- 42
        UPDATE public."USERS"
        SET "NAME" = 'Eduardo Teixeira'
        WHERE "EXTERNAL_IDENTIFIER" = '9e06cbfe-c407-655a-b041-bf229cc63281';

        -- 43
        UPDATE public."USERS"
        SET "NAME" = 'Luana Figueiredo'
        WHERE "EXTERNAL_IDENTIFIER" = '4838ae18-449b-663c-9a2b-54a1cb33a7cd';

        -- 44
        UPDATE public."USERS"
        SET "NAME" = 'Victor Figueiredo'
        WHERE "EXTERNAL_IDENTIFIER" = '746e8fef-443b-65c1-9aa2-5a28d2278b65';

        -- 45
        UPDATE public."USERS"
        SET "NAME" = 'Sabrina Correia'
        WHERE "EXTERNAL_IDENTIFIER" = 'f76304b6-d58d-603e-af55-bfafedc9fed6';

        -- 46
        UPDATE public."USERS"
        SET "NAME" = 'Samuel Correia'
        WHERE "EXTERNAL_IDENTIFIER" = '8faa854d-602b-63ec-967f-44098392cb31';

        -- 47
        UPDATE public."USERS"
        SET "NAME" = 'Gabriela Cunha'
        WHERE "EXTERNAL_IDENTIFIER" = 'e47e797e-c77e-6084-aba8-686b1db20913';

        -- 48
        UPDATE public."USERS"
        SET "NAME" = 'Marcos Cunha'
        WHERE "EXTERNAL_IDENTIFIER" = '5fb7a348-89ea-649b-85a1-c74e1f625cb4';
    END IF;

    IF EXISTS (SELECT 1 FROM public."REPOSITORIES" LIMIT 1) THEN
        RAISE NOTICE 'Oculting repositories';  

        WITH repos AS (
            SELECT id, ROW_NUMBER() OVER (ORDER BY id) AS rn
            FROM public."REPOSITORIES"
        )
        UPDATE public."REPOSITORIES" r
        SET "name" = 'Um repositório muito loko ' || repos.rn,
            remote_url = 'https://example.com/repo/' || repos.rn
        FROM repos
        WHERE r.id = repos.id;
    END IF;

    IF EXISTS (SELECT 1 FROM public."REPOSITORIES" LIMIT 1) THEN
        RAISE NOTICE 'Oculting repositories';  

        WITH pr AS (
            SELECT "ID", ROW_NUMBER() OVER (ORDER BY "ID") AS rn
            FROM public."PULL_REQUEST"
        )
        UPDATE public."PULL_REQUEST" r
        SET "TITLE" = 'Um pull request muito loko ' || pr.rn
          , "URL"='https://blogdoft.com.br'
        FROM pr
        WHERE r."ID" = pr."ID";
    END IF;

    IF EXISTS (select 1 from public."PULL_REQUEST_COMMENTS" LIMIT 1) THEN
        RAISE NOTICE 'Oculting comments';
        WITH prc AS (
            SELECT "ID", ROW_NUMBER() OVER (ORDER BY "ID") AS rn
            FROM public."PULL_REQUEST_COMMENTS"
        )
        UPDATE public."PULL_REQUEST_COMMENTS" r
        SET "COMMENT" = '[DICA] É sério isso? Acho que você precisa acessar o https://blogdoft.com.br pra entender melhor ' || pr.rn
        FROM prc
        WHERE r."ID" = prc."ID";
    END IF;
END $$;
