# README

## Aplicações disponíbilizadas:

**Atenção**: A rede do docker compose está definida no modo `host`. Isso quer dizer que todos os containeres utilizarão as portas disponíveis no host. Se as portas que usamos já estiverem ocupadas, você terá problemas na execução.

- Frontend [http://localhost:8080](http://localhost:8080);
- Backend (via proxy)[http://localhost:8080/code-review/swagger/index.html](http://localhost:8080/code-review/swagger/index.html)
- Postgres (jdbc:postgresql://127.0.0.1:5432/)
- Keycloak: [https://localhost:9443](https://localhost:9443)
- Prometheus: [http://localhost:9090](http://localhost:9090)
- Loki: [http://localhost:3100](http://localhost:3100)
- Promtail: [http://localhost:9081](http://localhost:9081)


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

Foi adicionado ao sistema o suporte ao Keycloak como solução de SSO.

Para configurar o acesso ao Keycloak, você precisa alterar o arquivo `./frontend/src/app/shared/keycloak.config.ts`.

```typescript
export const provideKeycloakAngular = () =>
    provideKeycloak({
        config: {
            realm: 'blog-do-ft', //<- o realm configurado no keycloak
            url: 'https://localhost:9443', // <- URL onde o kc está disponível
            clientId: 'code-review-insight' // <- Nome da aplicação no kc
        },
```

### Configurando o Keycloak

As configurações do Keycloak estão no arquivo `./eng/docker/keycloak/kc.env`. As principais que você precisa ficar atento são:

- KC_BOOTSTRAP_ADMIN_USERNAME
- KC_BOOTSTRAP_ADMIN_PASSWORD

Elas são responsáveis por configurar o usuário padrão de administração do keycloak. Após o primeiro acesso, um novo usuário administrador deve ser criado e esse deletado.

- KC_HOSTNAME
- KC_HOSTNAME_ADMIN

URL em que o keycloak irá responder as chamadas. Inclua a porta de acesso também.

É óbvio, mas não custa repetir: não utilize essas configurações em produção. E preferencialmente, modifique as configurações padrão.

### Gerando certificados

O nosso arquivo `./start-container.sh` já está gerando os certificados por você. Mas para que sejam gerados corretamente, você precisa configurar corretamente o arquivo `keycloak.cnf.template`.

Os principais pontos de atenção são:

```text
[req_distinguished_name]
CN = localhost

[alt_names]
DNS.1 = localhost
IP.1 = <seu-ip-externo>
```

Desta forma, os certificados estão válidos apenas para chamadas internas. Para chamadas externas, você vai precisar o DNS e o IP. A não ser que possua um Servidor DNS local, precisará atualizar o seu arquivo hosts.

Com o arquivo `keycloak.cnf.template` configurado, altere o nome dele, removendo a palavra `template` e então execute o `./start-container.sh`.

### Fazendo o navegador confiar no seu certificado

Você deve estar recebendo mensagens de erro do navegador ao acessar o keycloak, ou durante o processo de login.

Isso acontece porque, mesmo com certificado, o navegador não confia em certificados auto-assinados. Para evitar problemas, você precisa importar e confiar no certificado gerado.

Para usuários do Windows, você vai precisar:

1. Clique duas vezes no arquivo `keycloak.crt`
2. Clique em **"Instalar Certificado..."**
3. Selecione **"Máquina Local"**
4. Se aparecer, clique em **"Sim" para continuar**
5. Escolha a opção:
   - **"Colocar todos os certificados no repositório a seguir"**
6. Clique em **"Procurar..."**
7. Escolha: **"Autoridades de Certificação Raiz Confiáveis"**
8. Finalize e clique em **"Concluir"**
9. Reinicie o navegador **Edge**

Se estiver no Linux:

```bash
## Copia o certificado para a pasta de certificados confiáveis
sudo cp keycloak.crt /usr/local/share/ca-certificates/keycloak.crt

## Atualiza a lista de certificados confiáveis
sudo update-ca-certificates
```

## Iniciando a aplicação

**ATENÇÃO!** o script `./start-container.sh` irá iniciar todos os serviços especificados no arquivo `./eng/docker/docker-compose.yaml`. Para facilitar a sua vida, eu adicionei alguns agregadores de log. Caso não queira que esses agregadores entrem em ação, basta comentá-los. Para saber como funcionam, configurar e utilizar, veja as sessões posteriores.

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

Nesta etapa do projeto, ainda não dispomos de amplas telas de configurações. E ficar abrindo o banco de dados e fazendo operações manualmente, pode ser uma operação cansativa.

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

## Agregação de logs

Para ficar mais simples de visualizar os logs das várias aplicações, adicionamos ao docker compose as aplicações: Loki, Grafana e Promtail.

Os arquivos de configuração já seguem no repositório. Altere-os conforme a necessidade.

### Promtail e Docker no WSL

Se você está utilizando o WSL, especialmente versões antigas, sem o suporte ao systemd, você pode ter problemas com o Promtail.

Para mostrar os nomes dos containers, o Promtail precisa se vincular o seu serviço de discovery ao socket do docker. Em geral, o socket fica disponível em `/var/run/docker.sock` ou até mesmo em `/run/docker.sock`.

Entretanto, para o caso de instalações manuais ou na versão antiga do Docker no WSL, a melhor forma de descobrir onde o socket está é rodando:

```bash
echo $DOCKER_HOME
```

Este comando deve escrever algo como: `unix:///mnt/wsl/shared-docker/docker.sock` indicando o caminho do socket no docker. Talvez no seu computador seja levemente diferente.

Procure nos arquivos de configuração o endereço: `/mnt/wsl/shared-docker/docker.sock` e o substitua pelo que você tive encontrado no computador.

### Visualizando logs no Grafana

Você terá disponível 3 endereços:

- [http://localhost:9081 (Promtail)](http://localhost:9081)
- [http://localhost:3100 (Loki)](http://localhost:3100)
- [http://localhost:3000 (Grafana)](http://localhost:3000)

Basicamente, o Promtail é responsável por coletar os logs. Na configuração atual, ele está capturando os logs do seu ambiente e os logs do container. Mas no Grafana estarão acessíveis apenas os dos containeres.

Já o Loki funciona como agregador dos log, servindo de banco de dados para o Grafana.

E por sua vez, o Grafana fica responsável por nos mostrar os logs em múltiplas formas.

Para começar a visualizar os logs:

1. Acesse o [Grafana](http://localhost:3000) com as credenciais iniciais `admin:admin`;
2. No menu lateral esquerdo, clique em "Connections";
3. Depois escolha Data sources;
4. Na caixa de pesquisa, procure por "Loki" e clique nele;
5. Na opção "Connection" entre com `http://localhost:3100`, que é o endereço local onde o Loki está disponível;
6. Role até o final e clique em **Save and Test**.
   1. Se der tudo certo, você configurou corretamente tudo até aqui.
7. Volte ao topo da página e clique em **Explore data**;
8. A interface irá te permitir digitar uma query. Eu recomendo que você clique em "Builder" no canto superior esquerdo. Assim você terá uma experiência mais visual;
9. Em **Label Filters** monte a query que você deseja;
10. Clique então em **Live** ou no botão **Run query** (que visualmente se parece com um botão de atualizar);

Se deu tudo certo, você estará visualizando os logs do container selecionado 😃!

## Dúvidas?

Me procure no e-mail [admin@blogdoft.com.br](mailto:admin@blogdoft.com.br).
