import {
    provideHttpClient,
    withFetch,
    withInterceptors
} from '@angular/common/http';
import { ApplicationConfig, provideZoneChangeDetection } from '@angular/core';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';
import {
    provideRouter,
    withEnabledBlockingInitialNavigation,
    withInMemoryScrolling
} from '@angular/router';
import Aura from '@primeng/themes/aura';
import { includeBearerTokenInterceptor } from 'keycloak-angular';
import { providePrimeNG } from 'primeng/config';

import { appRoutes } from './app.routes';
import { provideKeycloakAngular } from './app/shared/keycloak.config';

export const appConfig: ApplicationConfig = {
    providers: [
        provideKeycloakAngular(),
        provideZoneChangeDetection({ eventCoalescing: true }),
        provideRouter(
            appRoutes,
            withInMemoryScrolling({
                anchorScrolling: 'enabled',
                scrollPositionRestoration: 'enabled'
            }),
            withEnabledBlockingInitialNavigation()
        ),
        provideHttpClient(
            withFetch(),
            withInterceptors([includeBearerTokenInterceptor])
        ),
        provideAnimationsAsync(),
        providePrimeNG({
            theme: { preset: Aura, options: { darkModeSelector: '.app-dark' } }
        })
    ]
};
