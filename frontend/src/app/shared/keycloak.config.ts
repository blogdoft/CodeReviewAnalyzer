import {
    provideKeycloak,
    createInterceptorCondition,
    IncludeBearerTokenCondition,
    INCLUDE_BEARER_TOKEN_INTERCEPTOR_CONFIG,
    withAutoRefreshToken,
    AutoRefreshTokenService,
    UserActivityService
} from 'keycloak-angular';

const localhostCondition =
    createInterceptorCondition<IncludeBearerTokenCondition>({
        urlPattern: /^(http:\/\/localhost:5031)(\/.*)?$/i
    });

export const provideKeycloakAngular = () =>
    provideKeycloak({
        config: {
            realm: 'blog-do-ft',
            url: 'https://localhost:9443',
            clientId: 'code-review-insight'
        },
        initOptions: {
            onLoad: 'check-sso',
            silentCheckSsoRedirectUri:
                window.location.origin + '/silent-check-sso.html'
        },
        features: [
            withAutoRefreshToken({
                onInactivityTimeout: 'logout',
                sessionTimeout: 60000
            })
        ],
        providers: [
            AutoRefreshTokenService,
            UserActivityService,
            {
                provide: INCLUDE_BEARER_TOKEN_INTERCEPTOR_CONFIG,
                useValue: [localhostCondition]
            }
        ]
    });
