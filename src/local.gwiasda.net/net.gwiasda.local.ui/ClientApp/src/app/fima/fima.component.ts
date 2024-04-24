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

  generateGUID(): string {
    return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
      var r = Math.random() * 16 | 0,
        v = c === 'x' ? r : (r & 0x3 | 0x8);
      return v.toString(16);
    });
  }

  async handleSaved() {
    if (this.editedCategory.description == null || this.editedCategory.description == '')
      this.editedCategory.description = this.editedCategory.name;

    if(this.editedCategory.id == null || this.editedCategory.id == '')
      this.editedCategory.id = this.generateGUID();

    this.dataService.write(this.editedCategory).subscribe(category => {
        this.readCostCategories();
        this.readIncomeCategories();
      });
  }

  async handleCancelled() {
    this.editedCategory = new FinanceCategory();
  }

  async handleDeleted() {

    if (this.editedCategory.id == null || this.editedCategory.id == '')
      return;
    await this.dataService.delete(this.editedCategory);
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
  getNameWithSpaces(category: FinanceCategory): string {
    let result = '';
    let count = 0;
    for (let i = 0; i < category.hierarchy; i++) {
      result += '---';
      count++;
    }
    return result + " " + category.name;
  }
  editCostCategory(category: FinanceCategory) {
    this.editedCategory = category;
  }
  editIncomeCategory(category: FinanceCategory) {
    this.editedCategory = category;
  }
}
