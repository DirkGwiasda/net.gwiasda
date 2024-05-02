import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Link } from './link';
import { Observable, catchError } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class LinkDataService {

  constructor(private http: HttpClient) { }

  getDefaultHeaders = () => new HttpHeaders()
    .set('Accept', 'application/json');

  read(): Observable<Link[] | undefined> {
    const headers = this.getDefaultHeaders();
    const url = 'link/GetLinks';
    return this.http.get<Link[]>(url, { headers });
  }
  async write(link: Link): Promise<void> {
    const headers = this.getDefaultHeaders();
    headers.set('Content-Type', 'application/json');
    const url = 'link/Save';

    await this.http.post(url, link, { headers }).toPromise();
  }

  async delete(link: Link): Promise<void> {
    const url = 'link/Delete?id=' + link.id;
    const headers = this.getDefaultHeaders();
    await this.http.get(url, { headers }).toPromise();
  }
}
