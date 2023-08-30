import { Injectable } from '@angular/core';
import { StorageService } from './storage.service';
import { Subject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})

export class SecurityService {
  IsAuthorized: any;
  private authSource = new Subject<boolean>();
  authChallenge$ = this.authSource.asObservable();

  constructor(private storeService: StorageService) {
    if (this.storeService.retrieve('IsAuthorized') !== '') {
      this.IsAuthorized = this.storeService.retrieve('IsAuthorized');
      this.authSource.next(true);
    }
  }

  public getToken(): any {
    return this.storeService.retrieve('userToken');
  }

  public getUserName(): any {
    return this.storeService.retrieve('userName');
  }

  public resetAuthData() {
    this.storeService.store('userName', '');
    this.storeService.store('userToken', '');
    this.IsAuthorized = false;
    this.storeService.store('IsAuthorized', false);
  }
  
  public setAuthData(user: any, token: any) {
    this.storeService.store('userName', user);
    this.storeService.store('userToken', token);
    this.IsAuthorized = true;
    this.storeService.store('IsAuthorized', true);

    this.authSource.next(true);
  }

  public logOut() {
    this.resetAuthData();
    this.authSource.next(true);
  }
}