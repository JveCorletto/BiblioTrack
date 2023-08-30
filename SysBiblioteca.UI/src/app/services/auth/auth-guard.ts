import { Injectable, inject } from '@angular/core';
import { SecurityService } from '../security.service';
import { CanActivateFn, Router } from '@angular/router';


@Injectable()
export class AuthGuard {

    constructor(
        private segurityService: SecurityService,
        public router: Router,
    ) { }

    canActivate(): boolean {
        if (this.segurityService.IsAuthorized) {
            return true
        } else {
            this.router.navigate(['/login']);
            return false
        }
    }
}

export const authGuard: CanActivateFn = (route, state) => {
    return inject(AuthGuard).canActivate();
};