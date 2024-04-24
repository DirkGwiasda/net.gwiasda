import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { FinanceCategory } from './finance_category';

@Injectable({
  providedIn: 'root',
})
export class FiMaDataService {

  constructor(private http: HttpClient) { }

  getDefaultHeaders = () => new HttpHeaders()
    .set('Accept', 'application/json');

  async readCostCategories(): Promise<FinanceCategory[] | undefined> {
    const headers = this.getDefaultHeaders();
    return await this.http.get<FinanceCategory[]>('fima/GetCostCategories', { headers }).toPromise();
  }
  async readIncomeCategories(): Promise<FinanceCategory[] | undefined> {
    const headers = this.getDefaultHeaders();
    return await this.http.get<FinanceCategory[]>('fima/GetIncomeCategories', { headers }).toPromise();
  }
}
