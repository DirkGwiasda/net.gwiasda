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

  ngOnInit() {
    this.readCostCategories();
  }

  async handleSaved() {
    await this.readCostCategories();
  }

  async readCostCategories(): Promise<void> {
    this.dataService.readAll()
      .then(costCategories => {
        this.costCategories = costCategories ?? [];
      });
  }
  //async delete(id: string) {
  //  console.log("delete: " + id);

  //  await this.dataService.delete(id);
  //  await this.readAppointments();
  //}
}
