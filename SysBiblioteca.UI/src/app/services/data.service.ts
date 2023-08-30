import { Observable } from "rxjs";
import { Injectable } from "@angular/core";
import { SecurityService } from "./security.service";
import { HttpClient, HttpResponse } from "@angular/common/http";

@Injectable({
  providedIn: 'root'
})

export class DataService {
  constructor(private http: HttpClient) { }

  GET<T>(url: string, httpParams?: any): Observable<HttpResponse<T>> {
    return this.http.get<T>(url, {
      params: httpParams,
      observe: 'response'
    });
  }

  POST<T>(url: string, data: any): Observable<HttpResponse<T>> {
    return this.http.post<T>(url, data, {
      observe: 'response'
    });
  }

  PUT<T>(url: string, data: any): Observable<HttpResponse<T>> {
    return this.http.put<T>(url, data, {
      observe: 'response'
    });
  }

  DELETE<T>(url: string, httpParams?: any): Observable<HttpResponse<T>> {
    return this.http.delete<T>(url, {
      params: httpParams,
      observe: 'response'
    });
  }
}
