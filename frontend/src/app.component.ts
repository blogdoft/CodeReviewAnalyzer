import { Component, inject, OnInit } from '@angular/core';
import { RouterModule } from '@angular/router';
import Keycloak from 'keycloak-js';
import { MessageService } from 'primeng/api';
import { ToastModule } from 'primeng/toast';

@Component({
    selector: 'app-root',
    standalone: true,
    imports: [RouterModule, ToastModule],
    template: `<p-toast></p-toast><router-outlet></router-outlet>`,
    providers: [MessageService]
})
export class AppComponent implements OnInit {
    title = 'Pull Request Insights';

    private keycloak = inject(Keycloak);

    ngOnInit(): void {
        if (!this.keycloak.authenticated) this.keycloak.login();
    }
}
