# README

## Requisitos

A forma mais fácil de rodar o Code Review Insight é através da construção das imagens docker. Para isso, você precisará ter o docker e o docker-compose (ou qualquer solução semelhante).

Contudo, se não possuir docker, ainda será possível compilar localmente na sua máquina, utilizando o dotnet sdk 9.0 e o Angular v19.x

## Configurando o sistema

As configurações do sistema ficam armazenadas em arquivos `.env`. Por motivos de segurança, esses arquivos não são versionados.

- ./eng/docker/dev.env
- ./eng/docker/postgres/database.env

Caso você tenha dúvidas sobre quais são as opções disponíveis, você tem os arquivos `.env.template` para se basear em como criar e configurar.

Os dados de PAT, Organization e ProjectName são necessários para pré-configurar o sistema, habilitando a coleta de dados.

## Keycloak

Foi adicionado ao sistema o suporte ao Keycloak como solução de sso.

Para configurar o acesso ao Keycloak, você precisa alterar o arquivo `./frontend/src/app/shared/keycloak.config.ts`.

```typescript
export const provideKeycloakAngular = () =>
    provideKeycloak({
        config: {
            realm: 'blog-do-ft', //<- o realm configurado no keycloak
            url: 'https://keycloak.local:8443', // <- URL onde o kc está disponível
            clientId: 'code-review-insight' // <- Nome da aplicação no kc
        },
```

## Iniciando a aplicação

Você pode rodar o script `./start-container.sh`. Ele irá construir as imagens docker e subir o docker compose.

Caso você não esteja utilizando o WSL, considere utilizar.

Em todo caso, o arquivo `./start-container.sh` contém um passo à passo sobre como levantar o sistema. Eu apenas aconselharia alterar o comando

```bash
docker compose up
```

para

```bash
docker compose up -d
```

A opção `-d` não vincula a saída do terminal automaticamente aos containers, deixando o terminal livre pra você.

Para ver os logs de cada container, digite:

```bash
docker container ls
```

E com o nome (ou id) do container, digite:

```bash
docker logs -f <nome-ou-id-do-container>
```

E observe os logs. `CTRL+C`cancela o vínculo e te devolve o terminal sem terminar as aplicações.

## Rodando pela primeira vez

A primeira vez que você inicializar os container, o backend irá morrer. Isso acontece porque o banco não fica pronto rápido o bastante e o backend está ávido por rodar as migrations.

Se isso acontecer, vá até o terminal e digite:

```bash
docker compose up -d backend
```

Você pode observar que agora sim as tabelas do sistema serão criadas.

### Inicializando com dados ou configurações

Nesta estapa do projeto, ainda não dispomos de amplas telas de configurações. E ficar abrindo o banco de dados e fazendo operações manualmente, pode ser uma operação cansativa.

Pensando nisso, dispomos a pasta `./eng/docker/postgres/init-sql`. Todos os arquivos dessa pasta serão rodados pelo container `init-db`.

Caso precise gravar dados padrão, manipulação de dados repetitivas ou qualquer outra manipulação sql, altere ou adicione arquivos nesta pasta.

Para rodar a inicialização, vá até o terminal na pasta `./eng/docker` e execute o comando:

```bash
docker compose up init-db
```

O container tem um ciclo de vida curto, durando apenas enquanto os comandos estão em execução.

Observe, portanto, na saída dos logs qualquer mensagem de erro - ou sucesso - que a execução possa gerar.

## Coletando dados

Ainda não dispomos de uma automação, por isso você terá de fazer a coleta de dados da azure manualmente.

Rode o arquivo `./start-container.sh` no terminal (ou suba a aplicação por outros meios).

Se for a primeira vez que está rodando o sistema, consulte a sessão [Rodando pela primeira vez](#rodando-pela-primeira-vez).

Agora acesse `http://localhost:5031/swagger` e rode a api `/api/pull-request-crawler-job` ou se preferir, no terminal, digite:

```bash
curl -X 'GET' \
  'http://localhost:5031/api/pull-request-crawler-job?begin=2024-01-01&end=2025-02-28&api-version=1.0' \
  -H 'accept: */*'
```

**Atenção**: Este endpoint ainda não está autenticado. Quando isso mudar, você terá que gerar um token para executar a chamada.

Aguarde alguns minutos até que todos os dados sejam carregados. O tempo vai depender da quantidade de pull requests dos repositórios do seu projeto.

### Customizações pós-execução

Após a conclusão, você pode executar novamente o container `init-db`. Esse é um ótimo momento para você criar times, vincular projetos, vincular pessoas e repositórios e etc.

As configurações que estão no arquivo agora, satisfazem necessidades minhas. Ao baixar, construa os arquivos conforme você achar necessário.

Qualquer arquivo `*.sql` dentro da pasta `./eng/docker/postgres/init-sql` será executado.

Eu só escrevi esses scripts para poupar tempo na restauração dos dados (precisei fazer várias vezes, né?). Depois de ajustar o script de leitura e criar interfaces de configuração, isso não será mais necessário.

## Dúvidas?

Me procure no e-mail admin@blogdoft.com.br.
