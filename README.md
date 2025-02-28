# README

## Requisitos

Para rodar o Pull Request Insight, você precisa ter na sua máquina instalado o dotnet 9, angular 19 e docker (com o pluggin docker compose).

## Configurando

Você precisa adicionar configurações em dois arquivos `.env` dentro de `./eng/docker`. São eles:

- ./eng/docker/dev.env
- ./eng/docker/postgres/database.env

Você tem os arquivos `.template` para se basear em como criar. 

Os dados de PAT, Organization e ProjectName são necessários para pré-configurar o sistema, habilitando a coleta de dados.

## Coletando dados

Ainda não dispomos de uma automação, por isso você terá de fazer manualmente.

Rode o arquivo `./start-container.sh` no terminal.

Isso fará com que o backend e o frontend sejam compilados, e as imagens docker sejam geradas e os containeres inicializados.

Provavelmente o backend irá falhar no início. Se isso acontecer, reinicie o container e espere as migrations serem concluídas.

```bash
docker compose up -d backend
```

Feito isso, rode o container `init-db` para que as configurações iniciais sejam carregadas.

```bash
docker compose up -d init-db
```

Agora acesse `http://localhost:5031/swagger` e rode a api `/api/pull-request-crawler-job` ou se preferir, no terminal, digite:

```bash
curl -X 'GET' \
  'http://localhost:5031/api/pull-request-crawler-job?begin=2024-01-01&end=2025-02-28&api-version=1.0' \
  -H 'accept: */*'
```

Aguarde alguns minutos até que todos os dados sejam carregados. O tempo vai depender da quantidade de pull requestes do seu repositório.

## Customizações pós-execução

Após a conclusão, inicie novamente o container `init-db`. Dessa vez, os scripts irão criar os times e fazer os vínculos entre usuários, times e repositórios.

As configurações que estão no arquivo agora, satisfazem necessidades minhas. Ao baixar, construa os arquivos conforme você achar necessário.

Qualquer arquivo *.sql dentro da pasta `./eng/docker/postgres/init-sql` serão executados.

Eu só escrevi esses scripts para poupar tempo na restauração dos dados (precisei fazer várias vezes, né?). Depois de ajustar o script de leitura, isso não será mais necessário.
