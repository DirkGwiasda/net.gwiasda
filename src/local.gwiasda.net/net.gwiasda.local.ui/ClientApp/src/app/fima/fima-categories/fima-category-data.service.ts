import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { FinanceCategory } from './finance_category';
import { Observable, catchError } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class FiMaCategoryDataService {

  constructor(private http: HttpClient) { }

  getDefaultHeaders = () => new HttpHeaders()
    .set('Accept', 'application/json');

  write(category: FinanceCategory): Observable<FinanceCategory> {
    const headers = this.getDefaultHeaders();
    headers.set('Content-Type', 'application/json');
    const url = 'fimacategory/Save';

    return this.http.post<FinanceCategory>(url, category, { headers });
  }
  async readCostCategories(): Promise<FinanceCategory[] | undefined> {
    const headers = this.getDefaultHeaders();
    return await this.http.get<FinanceCategory[]>('fimacategory/GetCostCategories', { headers }).toPromise();
  }
  async readIncomeCategories(): Promise<FinanceCategory[] | undefined> {
    const headers = this.getDefaultHeaders();
    return await this.http.get<FinanceCategory[]>('fimacategory/GetIncomeCategories', { headers }).toPromise();
  }
  async delete(category: FinanceCategory): Promise<void> {
    const headers = this.getDefaultHeaders();
    headers.set('Content-Type', 'application/json');
    const url = 'fimacategory/Delete';

    await this.http.post(url, category, { headers }).toPromise().catch(error => console.log(error));
  }
}
