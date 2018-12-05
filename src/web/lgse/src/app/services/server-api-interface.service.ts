import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { T } from 'blob';
import { UserRoleSelected } from '../models/ui/user.role.selected';

@Injectable()
export class ServerApiInterfaceService {

    constructor(private http: HttpClient) { }

    getAccessToken(): string {
        const accessToken = localStorage.getItem('accessToken');
        console.log('accesstoken', accessToken);
        if (accessToken) {
            return accessToken;
        }
        else {
            return '';
        }
    }
    getAccessRole(): string {
        // var accessToken = localStorage.getItem('userRole');
        // console.log('userRole::', accessToken);
        let localStorageUserRole = localStorage.getItem('userRole');
        if (localStorageUserRole) {
        let localStoreUserRole : UserRoleSelected = JSON.parse(localStorageUserRole);
        let selecteUserRole = new UserRoleSelected();
        selecteUserRole.roleId =  localStoreUserRole.roleId;
      var  userRoleId=selecteUserRole.roleId;
        console.log("RoleID:::",userRoleId);
       
            return userRoleId;
        }
        else {
            return '';
        }
    };
    post<T>(url: string, request: any): Observable<T> {
        let localHeader = this.getHeader();
        return this.http.post<T>(url, request, { headers: localHeader })
    }

    delete<T>(url: string): Observable<T> {
        let localHeader = this.getHeader();
        return this.http.delete<T>(url, { headers: localHeader })
    }
    patch<T>(url: string, request: any): Observable<T> {

        let localHeader = this.getHeader();
        return this.http.patch<T>(url, request, { headers: localHeader })
    }
    get<T>(url: string, queryParams?: HttpParams): Observable<T> {
        let localHeader = this.getHeader();
        return this.http.get<T>(url, {
            headers: localHeader,
            params: queryParams
        });
    }
    getProperty<T>(url: string,accessFlag:string, queryParams?: HttpParams): Observable<T> {
        let localHeader = this.getHeaderProperty(accessFlag);
        return this.http.get<T>(url, {
            headers: localHeader,
            params: queryParams
        });
    }
    download(url: string, queryParams?: HttpParams): Observable<T> {
        let localHeader = this.getHeader();
        return this.http.get(url, {
            headers: localHeader,
            params: queryParams,
           responseType: 'blob',
        });
    }
    getHeader(): HttpHeaders {
        let httpHeaders = new HttpHeaders({
            'Content-Type': 'application/json',
            'Accept': 'application/json',
            'Access-Control-Allow-Origin': '*',
            'Access-Control-Allow-Headers': 'X-Requested-With, Content-Type, Accept, Origin, Authorization',
            'Access-Control-Allow-Methods': '*',
            'ZUMO-API-VERSION': '2.0.0',
            'X-ZUMO-AUTH': this.getAccessToken(),
            'X-ACCESS-ROLE':this.getAccessRole()
        });
        console.log(httpHeaders);
        return httpHeaders;
    }
    getHeaderProperty(accessFlag): HttpHeaders {
        let httpHeaders = new HttpHeaders({
            'Content-Type': 'application/json',
            'Accept': 'application/json',
            'Access-Control-Allow-Origin': '*',
            'Access-Control-Allow-Headers': 'X-Requested-With, Content-Type, Accept, Origin, Authorization',
            'Access-Control-Allow-Methods': '*',
            'ZUMO-API-VERSION': '2.0.0',
            'X-ACCESS-FLAG': accessFlag,
            'X-ZUMO-AUTH': this.getAccessToken(),           
            'X-ACCESS-ROLE':this.getAccessRole()
        });
        console.log(httpHeaders);
        return httpHeaders;
    } // end of fucntion


    // resource func

    getResources<T>(url: string, queryParams?: HttpParams): Observable<T> {
     
        const localHeader1 = this.getHeaderProperty('1');
        console.log('localHeader1::');
        console.log(localHeader1);
        return this.http.get<T>(url, {
            headers: localHeader1,
            params: queryParams
        });
    } // end of fucn
}

