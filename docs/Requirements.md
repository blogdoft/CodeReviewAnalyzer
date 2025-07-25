# Requirements

## Geral
Precisamos analisar a performance do processo de desenvolvimento de times e indivíduos, olhando para os números gerados pelo processo de codificação, abertura de pull request, code review, bugs abertos por code review.

O produto deve:

- Criar meios de aferir a efetividade do processo de code review;
- Medir a aderência aos padrões processuais para desenvolvimento;
- Criar insights que permitam perceber o entrosamento de indivíduos aos processos de desenvolvimento do time;
- Permitir análises de performance de times e indivíduos, fornecendo insights para melhoria.

O produto deve ser capaz de extrair dados do Azure DevOps. Porém, deve ser capaz de extrair os mesmos dados de outras plataformas.

### Entidades

- Tentant: É a empresa que está utilizando o sistema. 
- DataSource: Qual é a origem dos dados de análise. Pode ser azure DevOps, Bit Bucket, Jira e afins.
- Pessoas: Pode ser coordenadores, gestores, desenvolvedores, tech leaders, testers e afins;
- Times: Grupo de pessoas no seu menor nível granular (Squad);
- Pull Requests: Propostas de mudança de codígo, aberta pelos desenvolvedores;
- Cards: Itens de trabalho que contém as especificações técnicas e de negócio de cada alteração de código. Podem ser bugs, features, stories e afins;
- Usuário: Pessoa utilizando o sistema. Essa pessoa pode ter papéis:
  - SysAdmin: pode navegar entre todos os times de um tenant;
  - Manager: pode navegar entre todos os timeas e pessoas que gerencia;
  - Pawns: pode ver a performance dos times em que faz parte e a sua própria performance;

```mermaid
erDiagram
   t[Tenant] {
        int id PK
        uuid sharedKey UK
        string name
        bool active
    }

    ds[DataSource] {
        int id PK
        uuid sharedKey UK
        string name
        string integrationType 
    }

    azdv[AzureDevops]{
        int id PK,FK "related to DataSource.id"
        string devOpsURL
        string pat
        json projects
        json areas
    }

    p[Person] {
        int id PK
        uuid sharedKey UK
        string externalId "Id used in datasource origin"
        string name
        string avatarUri
        Team[] Teams
    }

    tm[Team] {
        int id PK
        uuid sharedKey UK
        string externalId
        string name
        Person[] People
        Repository[] Repositories
    }

    repo[Repository] {
        int id PK
        uuid sharedKey UK
        string name
        string url
        Team[] Teams
    }


    pr[PullRequest] {
        int id PK
        uuid sharedKey UK
        string externalId
        string title
        string url
        Person createdBy FK
        datetime createdAt
        datetime closedAt
        datetime firstCommentAt
        datetime lastApprovedAt
        int firstCommentWaitingTimeMinutes
        int revisionWaitingTimeMinutes
        int mergeWaitingTimeMinutes
        string[100] mergeMode
        int fileCount
        int threadCount
        PullRequestReviewer[] Reviews
    }

    prc[PullRequestComment]{
        int id PK
        PullRequest pullRequest FK
        int commentIndex
        int threadId
        Person person
        datetime commentDate
        text comment
        datetime resolvedDate
    }

    prr[PullRequestReviewer] {
        PullRequest pullRequest FK,UK
        Person person FK,UK
        string vote
    }

    prwi[PullRequestWorkItem] {
        PullRequest pullRequest
        WorkIitem workItem
    }

    wi[WorkItem] {
        int id PK
        uuid sharedKey UK
        string externalId
        WorkItemType workItemType
        string areaPath
        string title
        dateTime createdAt
        dateTime closedAt
        bool canceled
    }

    df[Defect]

    t 1 to 0+ ds: "load data from"
    t 1 to 0+ p: employes
    t 1 to 0+ tm: has
    tm many to many p: "part of"
    tm many to many repo: has
    p one to zero or more wi: "works on"
    p one to zero or more df: "works on" 
    p one to zero or more pr: "opened a" 
    pr one to one repo: "opened against to"
    pr one to zero or more prc: "Commented by" 
    pr one to zero or more prr: "Reviewed by"
    pr one to one or more prwi: "Related to"
    prwi zero or more to one wi: "coded by"
    df one optionally to one wi: "kind of"
    ds one optionally to one azdv: "kind of" 
    wi one to one or more df: "related to"
```