/**
 * Representa um índice de tempo para algum indicador.
 */
export interface TimeIndex {
    /**
     * Período medido em minutos.
     */
    periodInMinutes: number;

    /**
     * Data de referência no formato ISO (date-time).
     */
    referenceDate: string;
}

/**
 * Representa o relatório de tamanho (número de arquivos) de um pull request.
 */
export interface PullRequestFileSize {
    /**
     * Quantidade média de arquivos.
     */
    meanFileCount: number;

    /**
     * Quantidade máxima de arquivos.
     */
    maxFileCount: number;

    /**
     * Quantidade mínima de arquivos.
     */
    minFileCount: number;

    /**
     * Data de referência no formato ISO (date).
     */
    referenceDate: string;
}

/**
 * Representa o relatório de tempo de um pull request.
 *
 * As propriedades são obrigatórias, mas podem vir com valor nulo,
 * conforme definido na documentação (nullable: true).
 */
export interface PullRequestTimeReport {
    /**
     * Tempo médio para iniciar a revisão.
     */
    meanTimeToStartReview: TimeIndex[] | null;

    /**
     * Tempo médio entre a abertura e a aprovação do pull request.
     */
    meanTimeToApprove: TimeIndex[] | null;

    /**
     * Tempo médio de revisão.
     */
    meanTimeToMerge: TimeIndex[] | null;

    /**
     * How many pull requests was closed in a period.
     */
    pullRequestCount: TimeIndex[] | null;

    /**
     * How many pull requests was closed without any comment in a period.
     */
    approvedOnFirstAttempt: TimeIndex[] | null;

    /**
     * Relatório sobre o tamanho dos pull requests (em número de arquivos).
     */
    pullRequestSize: PullRequestFileSize[] | null;
}

/**
 * Representa um erro conforme o padrão Problem Details do ASP.NET Core.
 */
export interface ProblemDetails {
    type?: string | null;
    title?: string | null;
    status?: number | null;
    detail?: string | null;
    instance?: string | null;
}
