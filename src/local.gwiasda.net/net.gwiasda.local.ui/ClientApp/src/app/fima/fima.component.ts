import { Component, OnInit } from '@angular/core';
import { FinanceCategory } from './finance_category';
import { FiMaDataService } from './fima-data.service';

@Component({
  selector: 'app-fima',
  templateUrl: './fima.component.html',
  styleUrls: ['./fima.component.css']
})
export class FiMaComponent {

  constructor(dataService: FiMaDataService) { this.dataService = dataService; }

  dataService: FiMaDataService;
  costCategories: FinanceCategory[] = [];
  incomeCategories: FinanceCategory[] = [];

  ngOnInit() {
    this.readCostCategories();
    this.readIncomeCategories();
  }

  async handleSaved() {
    await this.readCostCategories();
    await this.readIncomeCategories();
  }

  async readCostCategories(): Promise<void> {
    this.dataService.readCostCategories()
      .then(costCategories => {
        this.costCategories = costCategories ?? [];
      });
  }
  async readIncomeCategories(): Promise<void> {
    this.dataService.readIncomeCategories()
      .then(incomeCategories => {
        this.incomeCategories = incomeCategories ?? [];
      });
  }
  //async delete(id: string) {
  //  console.log("delete: " + id);

  //  await this.dataService.delete(id);
  //  await this.readAppointments();
  //}
}
