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

  async write(category: FinanceCategory): Promise<void> {
    if (!category) return;

    console.log(category);

    const headers = this.getDefaultHeaders();
    const url = 'fima/Save';

    await this.http.post(url, category, { headers }).toPromise();
  }
  async readCostCategories(): Promise<FinanceCategory[] | undefined> {
    const headers = this.getDefaultHeaders();
    return await this.http.get<FinanceCategory[]>('fima/GetCostCategories', { headers }).toPromise();
  }
  async readIncomeCategories(): Promise<FinanceCategory[] | undefined> {
    const headers = this.getDefaultHeaders();
    return await this.http.get<FinanceCategory[]>('fima/GetIncomeCategories', { headers }).toPromise();
  }
}
