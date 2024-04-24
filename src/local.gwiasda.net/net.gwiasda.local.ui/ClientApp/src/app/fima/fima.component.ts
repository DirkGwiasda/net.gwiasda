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
  editedCategory: FinanceCategory = new FinanceCategory();

  ngOnInit() {
    this.readCostCategories();
    this.readIncomeCategories();
  }

  async handleSaved() {
    if (this.editedCategory.description == null || this.editedCategory.description == '')
      this.editedCategory.description = this.editedCategory.name;

    await this.dataService.write(this.editedCategory);
    await this.readCostCategories();
    await this.readIncomeCategories();

    this.editedCategory = new FinanceCategory();
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
