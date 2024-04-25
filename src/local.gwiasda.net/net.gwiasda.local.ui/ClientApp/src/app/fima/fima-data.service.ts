import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { FinanceCategory } from './fima-categories/finance_category';
import { Observable, catchError } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class FiMaDataService {

  constructor(private http: HttpClient) { }

  getDefaultHeaders = () => new HttpHeaders()
    .set('Accept', 'application/json');

  write(category: FinanceCategory): Observable<FinanceCategory> {
    const headers = this.getDefaultHeaders();
    headers.set('Content-Type', 'application/json');
    const url = 'fima/Save';

    return this.http.post<FinanceCategory>(url, category, { headers });
  }
  async readCostCategories(): Promise<FinanceCategory[] | undefined> {
    const headers = this.getDefaultHeaders();
    return await this.http.get<FinanceCategory[]>('fima/GetCostCategories', { headers }).toPromise();
  }
  async readIncomeCategories(): Promise<FinanceCategory[] | undefined> {
    const headers = this.getDefaultHeaders();
    return await this.http.get<FinanceCategory[]>('fima/GetIncomeCategories', { headers }).toPromise();
  }
  async delete(category: FinanceCategory): Promise<void> {
    const headers = this.getDefaultHeaders();
    headers.set('Content-Type', 'application/json');
    const url = 'fima/Delete';

    await this.http.post(url, category, { headers }).toPromise().catch(error => console.log(error));
  }
}
